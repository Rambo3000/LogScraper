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
using LogScraper.Log.Collection;

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
                        LogContentProperties = [],
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
                    foreach (LogContentProperty property in layout.LogContentProperties)
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
                        layoutNew.LogContentProperties.Add(newProperty);
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
                    string.IsNullOrEmpty(layout.RemoveMetaDataCriteria.AfterPhrase))
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

                foreach (LogContentProperty property in layout.LogContentProperties)
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
                LogContentProperties = []
            };
            layout.LogTransformers = [];
            layout.LogMetadataProperties.Add(CreateLogMetadataProperty());
            layout.LogContentProperties.Add(CreateLogContentProperty());

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

            BtnMetadataRemove.Enabled = LstMetadata.Items.Count > 0;
            BtnMetadataUp.Enabled = LstMetadata.SelectedIndex > 0;
            BtnMetadataDown.Enabled = LstMetadata.SelectedIndex != -1 && LstMetadata.SelectedIndex < (LstMetadata.Items.Count - 1);

            if (LstMetadata.Items.Count == 0)
            {
                TxtMetadataDescription.Text = string.Empty;
                TxtMetadataBeforePhrase.Text = string.Empty;
                TxtMetadataAfterPhrase.Text = string.Empty;
            }
            TxtMetadataDescription.Enabled = LstMetadata.Items.Count > 0;
            TxtMetadataBeforePhrase.Enabled = LstMetadata.Items.Count > 0;
            TxtMetadataAfterPhrase.Enabled = LstMetadata.Items.Count > 0;

            BtnContentRemove.Enabled = LstContent.Items.Count > 0;
            BtnContentUp.Enabled = LstContent.SelectedIndex > 0;
            BtnContentDown.Enabled = LstContent.SelectedIndex != -1 && LstContent.SelectedIndex < (LstContent.Items.Count - 1);

            if (LstContent.Items.Count == 0)
            {
                TxtContentDescription.Text = string.Empty;
                TxtContentBeforePhrase.Text = string.Empty;
                TxtContentAfterPhrase.Text = string.Empty;
            }
            TxtContentDescription.Enabled = LstContent.Items.Count > 0;
            TxtContentBeforePhrase.Enabled = LstContent.Items.Count > 0;
            TxtContentAfterPhrase.Enabled = LstContent.Items.Count > 0;
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
            BtnCopy.Enabled = LstLayouts.SelectedItem != null;
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
                foreach (LogContentProperty property in selected.LogContentProperties)
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

            LstContent.SelectedIndex = -1;
            LstContent.SelectedIndex = LstContent.Items.Count - 1;
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentProperties = [.. _contentProperties];
            UpdateButtons();
        }

        private void BtnContentRemove_Click(object sender, EventArgs e)
        {
            ButtonRemove(LstContent, _contentProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentProperties = [.. _contentProperties];
        }

        private void BtnContentUp_Click(object sender, EventArgs e)
        {
            ButtonUp(LstContent, _contentProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentProperties = [.. _contentProperties];
        }

        private void BtnContentDown_Click(object sender, EventArgs e)
        {
            ButtonDown(LstContent, _contentProperties);
            if (LstLayouts.SelectedItem is LogLayout selected) selected.LogContentProperties = [.. _contentProperties];
        }

        private void BtnMetadataAdd_Click(object sender, EventArgs e)
        {
            LogMetadataProperty property = CreateLogMetadataProperty();
            _metadataProperties.Add(property);

            LstMetadata.SelectedIndex = -1;
            LstMetadata.SelectedIndex = LstMetadata.Items.Count - 1;
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
                            TxtJsonPath.Text = string.Empty;
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

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is LogLayout logLayout) // Ensure 'logLayout' is defined
            {
                LogCollection logCollection = new();
                try
                {
                    if (TxtExampleLogLine.Text == string.Empty)
                    {
                        throw new Exception("Voer een logregel op om te testen hoe deze geinterpreteerd wordt.");
                    }

                    LogReader.ReadIntoLogCollection([TxtExampleLogLine.Text], logCollection, logLayout);
                    LogLineClassifier.ClassifyLogLineMetadataProperties(logLayout, logCollection);
                    LogLineClassifier.ClassifyLogLineContentProperties(logLayout, logCollection);

                    string information = string.Empty;
                    LogLine logLine = logCollection.LogLines[0];

                    information += $"Datum tijd: {logLine.TimeStamp}" + Environment.NewLine;
                    information += Environment.NewLine;
                    information += $"Log regel zonder metadata:" + Environment.NewLine;
                    information += $"   {LogExportDataCreator.RemoveTextBasedOnCriteria(logLine.Line, logLayout.RemoveMetaDataCriteria, logLayout.StartPosition)}" + Environment.NewLine;
                    information += Environment.NewLine;
                    information += "Metadata:" + Environment.NewLine;
                    foreach (var property in logLayout.LogMetadataProperties)
                    {
                        logLine.LogMetadataPropertiesWithStringValue.TryGetValue(property, out string value);
                        information += $"   {property.Description}: {value ??= "<niet gevonden>"}" + Environment.NewLine;
                    }
                    information += Environment.NewLine;
                    information += "Content begin en eind filters:" + Environment.NewLine;
                    foreach (var property in logLayout.LogContentProperties)
                    {
                        logLine.LogContentProperties.TryGetValue(property, out string value);
                        information += $"   {property.Description}: {value ??= "<niet gevonden>"}" + Environment.NewLine;
                    }
                    TxtTestResponse.ForeColor = System.Drawing.Color.Black;
                    TxtTestResponse.Text = information;

                }
                catch (Exception exception)
                {
                    TxtTestResponse.ForeColor = System.Drawing.Color.DarkRed;
                    TxtTestResponse.Text = $"Error: {exception.Message}";
                }
            }
            else
            {
                MessageBox.Show("Please select a layout before testing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is not LogLayout logLayout) return;
            LogLayout logLayoutCopy = new()
            {
                Description = logLayout.Description + " (kopie)",
                DateTimeFormat = logLayout.DateTimeFormat,
                RemoveMetaDataCriteria = new()
                {
                    AfterPhrase = logLayout.RemoveMetaDataCriteria.AfterPhrase,
                    BeforePhrase = logLayout.RemoveMetaDataCriteria.BeforePhrase,
                },
                LogMetadataProperties = [],
                LogContentProperties = [],
                LogTransformers = []
            };
            foreach (LogMetadataProperty property in logLayout.LogMetadataProperties)
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
                logLayoutCopy.LogMetadataProperties.Add(newProperty);
            }
            foreach (LogContentProperty property in logLayout.LogContentProperties)
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
                logLayoutCopy.LogContentProperties.Add(newProperty);
            }
            foreach (ILogTransformer transformer in logLayout.LogTransformers)
            {
                if (transformer is OrderReversalTransformer)
                {
                    ILogTransformer newTransformer = new OrderReversalTransformer();
                    logLayoutCopy.LogTransformers.Add(newTransformer);
                }
                if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
                {
                    ILogTransformer newTransformer = new JsonPathExtractionTranformer(jsonPathExtractionTranformer.JsonPath);
                    logLayoutCopy.LogTransformers.Add(newTransformer);
                }
            }
            _layouts.Add(logLayoutCopy);
        }
    }
}