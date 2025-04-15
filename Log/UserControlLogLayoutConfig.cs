using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Metadata;
using LogScraper.Log.Content;
using LogScraper.Log.Filter;
using LogScraper.LogTransformers;
using LogScraper.LogTransformers.Implementations;

namespace LogScraper.Log
{
    public partial class UserControlLogLayoutConfig : UserControl
    {
        private readonly BindingList<LogLayout> _layouts = [];
        private readonly BindingList<LogMetadataProperty> _metadataProperties = [];
        private readonly BindingList<LogContentProperty> _contentProperties = [];

        public UserControlLogLayoutConfig()
        {
            InitializeComponent();
            LstLayouts.DataSource = _layouts;
            LstMetadata.DataSource = _metadataProperties;
            LstContent.DataSource = _contentProperties;
            LstLayouts.DisplayMember = "Description";
            LstContent.DisplayMember = "Description";
            LstMetadata.DisplayMember = "Description";
        }
        internal void SetLogLayoutsConfig(List<LogLayout> layouts)
        {
            // Copy instances so we dont mix them with the ones already in the config
            if (layouts != null && layouts.Count > 0)
            {
                foreach (var layout in layouts)
                {
                    LogLayout layoutNew = new()
                    {
                        Description = layout.Description,
                        DateTimeFormat = layout.DateTimeFormat,
                        RemoveMetaDataCriteria = new()
                        {
                            AfterPhrase = layout.RemoveMetaDataCriteria.AfterPhrase,
                            BeforePhrase = layout.RemoveMetaDataCriteria.BeforePhrase,
                        },
                        LogMetadataProperties = [],
                        LogContentBeginEndFilters = [],
                        LogTransformers = [],
                    };

                    foreach (LogMetadataProperty property in layout.LogMetadataProperties)
                    {
                        LogMetadataProperty newProperty = new()
                        {
                            Description = property.Description,
                            Criteria = new FilterCriteria()
                            {
                                BeforePhrase = property.Criteria.BeforePhrase,
                                AfterPhrase = property.Criteria.AfterPhrase
                            }
                        };
                        layoutNew.LogMetadataProperties.Add(newProperty);
                    }
                    foreach (LogContentProperty property in layout.LogContentBeginEndFilters)
                    {
                        LogContentProperty newProperty = new()
                        {
                            Description = property.Description,
                            Criteria = new FilterCriteria()
                            {
                                BeforePhrase = property.Criteria.BeforePhrase,
                                AfterPhrase = property.Criteria.AfterPhrase
                            }
                        };
                        layoutNew.LogContentBeginEndFilters.Add(newProperty);
                    }
                    if (layout.LogTransformers != null)
                    {
                        foreach (ILogTransformer transformer in layout.LogTransformers)
                        {
                            if (transformer is OrderReversalTransformer)
                            {
                                ILogTransformer newTransformer = new OrderReversalTransformer();
                                layoutNew.LogTransformers.Add(newTransformer);
                            }
                            if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
                            {
                                ILogTransformer newTransformer = new JsonPathExtractionTranformer(jsonPathExtractionTranformer.JsonPath);
                                layoutNew.LogTransformers.Add(newTransformer);
                            }
                        }
                    }
                    _layouts.Add(layoutNew);
                }
                if (layouts.Count > 0) LstLayouts.SelectedIndex = 0;
            }
        }
        internal bool TryGetLogLayoutsConfig(out LogLayoutsConfig config)
        {
            List<string> errorMessages = [];

            foreach (LogLayout layout in _layouts)
            {
                if (string.IsNullOrWhiteSpace(layout.Description) ||
                    string.IsNullOrWhiteSpace(layout.DateTimeFormat) ||
                    string.IsNullOrWhiteSpace(layout.RemoveMetaDataCriteria.AfterPhrase))
                {
                    errorMessages.Add($"Layout '{layout.Description}' is niet compleet ingevuld.");
                }

                foreach (LogMetadataProperty property in layout.LogMetadataProperties)
                {
                    if (string.IsNullOrWhiteSpace(property.Description) ||
                        string.IsNullOrEmpty(property.Criteria.BeforePhrase) ||
                        string.IsNullOrEmpty(property.Criteria.AfterPhrase))
                    {
                        errorMessages.Add($"Layout '{layout.Description}' en metadata '{property.Description}' is niet compleet ingevuld.");
                    }
                }

                foreach (LogContentProperty property in layout.LogContentBeginEndFilters)
                {
                    if (string.IsNullOrWhiteSpace(property.Description) ||
                        string.IsNullOrWhiteSpace(property.Criteria.BeforePhrase))
                    {
                        errorMessages.Add($"Layout '{layout.Description}' en content item '{property.Description}' is niet compleet ingevuld.");
                    }
                }
                foreach (ILogTransformer transformer in layout.LogTransformers)
                {
                    if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer && string.IsNullOrWhiteSpace(jsonPathExtractionTranformer.JsonPath))
                    {
                        errorMessages.Add($"Voor layout '{layout.Description}' is de JSON transformer geselecteerd maar er is geen JSON path ingevuld");
                    }
                }
            }

            config = null;
            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            config = new LogLayoutsConfig
            {
                layouts = [.. _layouts]
            };

            foreach (LogLayout layout in config.layouts)
            {
                layout.LogTransformersConfig = [];
                foreach (ILogTransformer transformer in layout.LogTransformers)
                {
                    layout.LogTransformersConfig.Add(transformer.CreateTransformerConfig());
                }
            }

            return true;
        }

        private void BtnAddLayout_Click(object sender, EventArgs e)
        {
            LogLayout logLayout = CreateLayout();
            _layouts.Add(logLayout);

            LstLayouts.SelectedItem = logLayout;
            UpdateButtons();
        }
        private static LogLayout CreateLayout()
        {
            LogLayout layout = new()
            {
                Description = "Nieuwe layout",
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ss,fff",
                RemoveMetaDataCriteria = new(),
                LogMetadataProperties = [],
                LogContentBeginEndFilters = []
            };
            layout.LogMetadataProperties.Add(CreateLogMetadataProperty());
            layout.LogContentBeginEndFilters.Add(CreateLogContentProperty());

            return layout;
        }
        private static LogMetadataProperty CreateLogMetadataProperty()
        {
            return new() { Description = "Nieuwe metadata", Criteria = new() };
        }
        private static LogContentProperty CreateLogContentProperty()
        {
            return new() { Description = "Nieuwe metadata", Criteria = new() };
        }

        private void BtnRemoveLayout_Click(object sender, EventArgs e)
        {
            ButtonRemove(LstLayouts, _layouts);
        }

        private void UpdateButtons()
        {
            BtnLayoutRemove.Enabled = LstLayouts.Items.Count > 1;
            BtnLayoutUp.Enabled = LstLayouts.SelectedIndex > 0;
            BtnLayoutDown.Enabled = LstLayouts.SelectedIndex != -1 && LstLayouts.SelectedIndex < (LstLayouts.Items.Count - 1);

            BtnMetadataRemove.Enabled = LstMetadata.Items.Count > 1;
            BtnMetadataUp.Enabled = LstMetadata.SelectedIndex > 0;
            BtnMetadataDown.Enabled = LstMetadata.SelectedIndex != -1 && LstMetadata.SelectedIndex < (LstMetadata.Items.Count - 1);

            BtnContentRemove.Enabled = LstContent.Items.Count > 1;
            BtnContentUp.Enabled = LstContent.SelectedIndex > 0;
            BtnContentDown.Enabled = LstContent.SelectedIndex != -1 && LstContent.SelectedIndex < (LstContent.Items.Count - 1);

        }
        private void ButtonRemove<T>(ListBox listbox, BindingList<T> bindingList)
        {
            if (listbox.SelectedItem is T selected)
            {
                bindingList.Remove(selected);
                UpdateButtons();
            }
        }
        private void ButtonUp<T>(ListBox listbox, BindingList<T> bindingList)
        {
            if (listbox.SelectedItem is not T selected) return;

            int index = bindingList.IndexOf(selected);
            if (index > 0)
            {
                bindingList.RemoveAt(index);
                bindingList.Insert(index - 1, selected);
                listbox.SelectedIndex = index - 1;
            }
            UpdateButtons();
        }
        private void ButtonDown<T>(ListBox listbox, BindingList<T> bindingList)
        {
            if (listbox.SelectedItem is not T selected) return;

            int index = bindingList.IndexOf(selected);
            if (index < bindingList.Count - 1)
            {
                bindingList.RemoveAt(index);
                bindingList.Insert(index + 1, selected);
                listbox.SelectedIndex = index + 1;
            }
            UpdateButtons();
        }
        private void BtnUpLayout_Click(object sender, EventArgs e)
        {
            ButtonUp(LstLayouts, _layouts);
        }

        private void BtnDownLayout_Click(object sender, EventArgs e)
        {
            ButtonDown(LstLayouts, _layouts);
        }

        private bool UpdatingInformation = false;
        private void LstLogLayouts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is LogLayout selected)
            {
                UpdatingInformation = true;
                TxtDescription.Text = selected.Description;
                TxtDateTimeFormat.Text = selected.DateTimeFormat;
                TxtMetadataBegin.Text = selected.RemoveMetaDataCriteria.BeforePhrase;
                TxtMetadataEnd.Text = selected.RemoveMetaDataCriteria.AfterPhrase;

                _metadataProperties.Clear();
                foreach (LogMetadataProperty property in selected.LogMetadataProperties)
                {
                    _metadataProperties.Add(property);
                }
                if (_metadataProperties.Count > 0) LstMetadata_SelectedIndexChanged(null, null);

                _contentProperties.Clear();
                foreach (LogContentProperty property in selected.LogContentBeginEndFilters)
                {
                    _contentProperties.Add(property);
                }
                if (_contentProperties.Count > 0) LstContent_SelectedIndexChanged(null, null);

                chkTransformReverse.Checked = false;
                chkTransformJson.Checked = false;
                TxtJsonPath.Text = "";
                if (selected.LogTransformers != null)
                {
                    foreach (ILogTransformer transformer in selected.LogTransformers)
                    {
                        if (transformer is OrderReversalTransformer)
                        {
                            chkTransformReverse.Checked = true;
                        }
                        if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
                        {
                            chkTransformJson.Checked = true;
                            TxtJsonPath.Text = jsonPathExtractionTranformer.JsonPath;
                        }
                    }
                }

                UpdatingInformation = false;
            }
            UpdateButtons();
        }

        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout selected) selected.Description = TxtDescription.Text;

            LstLayouts.DisplayMember = ""; // Force update
            LstLayouts.DisplayMember = "Description";
        }

        private void TxtDateTimeFormat_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout selected) selected.DateTimeFormat = TxtDateTimeFormat.Text;
        }

        private void LstMetadata_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstMetadata.SelectedItem is LogMetadataProperty selected)
            {
                TxtMetadataDescription.Text = selected.Description;
                TxtMetadataBeforePhrase.Text = selected.Criteria.BeforePhrase;
                TxtMetadataAfterPhrase.Text = selected.Criteria.AfterPhrase;
            }
            UpdateButtons();
        }

        private void LstContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstContent.SelectedItem is LogContentProperty selected)
            {
                TxtContentDescription.Text = selected.Description;
                TxtContentBeforePhrase.Text = selected.Criteria.BeforePhrase;
                TxtContentAfterPhrase.Text = selected.Criteria.AfterPhrase;
            }
            UpdateButtons();
        }

        private void BtnContentAdd_Click(object sender, EventArgs e)
        {
            LogContentProperty property = CreateLogContentProperty();
            _contentProperties.Add(property);

            LstContent.SelectedItem = property;
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentBeginEndFilters = [.. _contentProperties];
            UpdateButtons();
        }

        private void BtnContentRemove_Click(object sender, EventArgs e)
        {
            ButtonRemove(LstContent, _contentProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentBeginEndFilters = [.. _contentProperties];
        }

        private void BtnContentUp_Click(object sender, EventArgs e)
        {
            ButtonUp(LstContent, _contentProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentBeginEndFilters = [.. _contentProperties];
        }

        private void BtnContentDown_Click(object sender, EventArgs e)
        {
            ButtonDown(LstContent, _contentProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentBeginEndFilters = [.. _contentProperties];
        }

        private void BtnMetadataAdd_Click(object sender, EventArgs e)
        {
            LogMetadataProperty property = CreateLogMetadataProperty();
            _metadataProperties.Add(property);

            LstMetadata.SelectedItem = property;
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogMetadataProperties = [.. _metadataProperties];
            UpdateButtons();
        }

        private void BtnMetadataRemove_Click(object sender, EventArgs e)
        {
            ButtonRemove(LstMetadata, _metadataProperties);

            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogMetadataProperties = [.. _metadataProperties];
        }

        private void BtnMetadataUp_Click(object sender, EventArgs e)
        {
            ButtonUp(LstMetadata, _metadataProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogMetadataProperties = [.. _metadataProperties];
        }

        private void BtnMetadataDown_Click(object sender, EventArgs e)
        {
            ButtonDown(LstMetadata, _metadataProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogMetadataProperties = [.. _metadataProperties];
        }

        private void TxtMetadataDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstMetadata.SelectedItem is LogMetadataProperty selected)
            {
                selected.Description = TxtMetadataDescription.Text;
                LstMetadata.DisplayMember = "";
                LstMetadata.DisplayMember = "Description";
            }

        }

        private void TxtMetadataBeforePhrase_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstMetadata.SelectedItem is LogMetadataProperty selected) selected.Criteria.BeforePhrase = TxtMetadataBeforePhrase.Text;
        }

        private void TxtMetadataAfterPhrase_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstMetadata.SelectedItem is LogMetadataProperty selected) selected.Criteria.AfterPhrase = TxtMetadataAfterPhrase.Text;

        }

        private void TxtContentDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstContent.SelectedItem is LogContentProperty selected)
            {
                selected.Description = TxtContentDescription.Text;
                LstContent.DisplayMember = "";
                LstContent.DisplayMember = "Description";
            }

        }

        private void TxtContentBeforePhrase_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstContent.SelectedItem is LogContentProperty selected) selected.Criteria.BeforePhrase = TxtContentBeforePhrase.Text;
        }

        private void TxtContentAfterPhrase_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstContent.SelectedItem is LogContentProperty selected) selected.Criteria.AfterPhrase = TxtContentAfterPhrase.Text;
        }

        private void TxtMetadataBegin_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout selected) selected.RemoveMetaDataCriteria.BeforePhrase = TxtMetadataBegin.Text;
        }

        private void TxtMetadataEnd_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout selected) selected.RemoveMetaDataCriteria.AfterPhrase = TxtMetadataEnd.Text;

        }

        private void ChkTransformReverse_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout layout)
            {
                bool transformerFound = false;
                foreach (ILogTransformer transformer in layout.LogTransformers)
                {
                    if (transformer is OrderReversalTransformer)
                    {
                        transformerFound = true;
                        if (chkTransformReverse.Checked == false)
                        {
                            layout.LogTransformers.Remove(transformer);
                            return;
                        }
                    }
                }

                if (chkTransformReverse.Checked && transformerFound == false)
                {
                    ILogTransformer newTransformer = new OrderReversalTransformer();
                    layout.LogTransformers.Add(newTransformer);
                }
            }
        }

        private void ChkTransformJson_CheckedChanged(object sender, EventArgs e)
        {
            TxtJsonPath.Enabled = chkTransformJson.Checked;
            TxtJsonPath.IsRequired = chkTransformJson.Checked;

            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout layout)
            {
                bool transformerFound = false;
                foreach (ILogTransformer transformer in layout.LogTransformers)
                {
                    if (transformer is JsonPathExtractionTranformer)
                    {
                        transformerFound = true;
                        if (chkTransformJson.Checked == false)
                        {
                            layout.LogTransformers.Remove(transformer);
                            return;
                        }
                    }
                }

                if (chkTransformJson.Checked && transformerFound == false)
                {
                    ILogTransformer newTransformer = new JsonPathExtractionTranformer(string.Empty);
                    layout.LogTransformers.Add(newTransformer);
                }
            }
        }

        private void TxtJsonPath_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingInformation) return;

            if (LstLayouts.SelectedItem is LogLayout layout)
            {
                foreach (ILogTransformer transformer in layout.LogTransformers)
                {
                    if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
                    {
                        jsonPathExtractionTranformer.JsonPath = TxtJsonPath.Text;
                        return;
                    }
                }
            }
        }
    }
}