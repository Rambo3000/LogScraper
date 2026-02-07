using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LogScraper.Sources.Adapters;

namespace LogScraper.LogProviders.File
{
    public partial class UserControlFileLogProvider : UserControl, ILogProviderUserControl
    {
        private const string PlaceholderText = "Geen bestand geselecteerd";
        private readonly Color PlaceholderColor = Color.Gray;
        private readonly Color NormalTextColor = SystemColors.WindowText;

        public event EventHandler SourceSelectionChanged;

        public event Action<string, bool> StatusUpdate;

        public UserControlFileLogProvider()
        {
            InitializeComponent();
            if (Debugger.IsAttached)
            {
                txtFilePath.Text = "Stubs/JSONInvertedExample.log";
                txtFilePath.ForeColor = NormalTextColor;
            }
            else
            {
                SetPlaceholder();
            }
        }

        private void SetPlaceholder()
        {
            txtFilePath.Text = PlaceholderText;
            txtFilePath.ForeColor = PlaceholderColor;
        }

        private void SetNormalText(string text)
        {
            txtFilePath.Text = text;
            txtFilePath.ForeColor = NormalTextColor;
        }

        private bool IsPlaceholderActive()
        {
            return txtFilePath.Text == PlaceholderText && txtFilePath.ForeColor == PlaceholderColor;
        }

        public ISourceAdapter GetSourceAdapter()
        {
            // Don't return adapter if placeholder is active
            if (IsPlaceholderActive())
            {
                return null;
            }
            return SourceAdapterFactory.CreateFileSourceAdapter(txtFilePath.Text);
        }

        protected virtual void OnSourceSelectionChanged()
        {
            SourceSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "Text Files (*.txt;*.csv;*.tsv;*.log;*.xml;*.json)|*.txt;*.csv;*.tsv;*.log;*.xml;*.json|All Files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = GetDefaultOpenFolder()
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string selectedFilePath = openFileDialog.FileName;
            SetNormalText(selectedFilePath);

            if (!ValidateFile(selectedFilePath, out string errorMessage))
            {
                LblFileSizeValue.Text = "-";
                LblModificationDateTimeValue.Text = "-";
                PctWarning.Visible = false;
                OnStatusUpdate(errorMessage, false);
                return;
            }

            try
            {
                FileInfo fileInfo = new(selectedFilePath);
                LblFileSizeValue.Text = FormatFileSize(fileInfo.Length) + " " + FormatEstimatedLineCount(selectedFilePath);
                LblModificationDateTimeValue.Text = FormatModified(fileInfo.LastWriteTimeUtc);
                bool sizeIsTooLarge = fileInfo.Length > 500L * 1024 * 1024;
                PctWarning.Visible = sizeIsTooLarge;
                if (!sizeIsTooLarge) OnSourceSelectionChanged();
            }
            catch (Exception ex)
            {
                LblFileSizeValue.Text = $"Error reading metadata: {ex.Message}";
                LblModificationDateTimeValue.Text = string.Empty;
                PctWarning.Visible = false;
            }

            OnStatusUpdate("OK", true);
        }

        private static bool ValidateFile(string filePath, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!System.IO.File.Exists(filePath))
            {
                errorMessage = "File does not exist";
                return false;
            }

            try
            {
                // Check file size first
                FileInfo fileInfo = new(filePath);
                if (fileInfo.Length == 0)
                {
                    errorMessage = "File is empty";
                    return false;
                }

                // Try to open and read to verify access
                using FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Read first chunk to validate it's text content
                const int sampleSize = 8192; // 8KB sample
                int bytesToRead = (int)Math.Min(sampleSize, fileInfo.Length);
                byte[] buffer = new byte[bytesToRead];
                int bytesRead = stream.Read(buffer, 0, bytesToRead);

                if (!IsTextContent(buffer, bytesRead, out string contentError))
                {
                    errorMessage = contentError;
                    return false;
                }

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                errorMessage = "Access denied - insufficient permissions";
                return false;
            }
            catch (IOException ex)
            {
                errorMessage = $"File is locked or inaccessible: {ex.Message}";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error accessing file: {ex.Message}";
                return false;
            }
        }

        private static bool IsTextContent(byte[] buffer, int length, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (length == 0)
            {
                errorMessage = "File is empty";
                return false;
            }

            int nullBytes = 0;
            int controlChars = 0;
            int printableChars = 0;

            for (int i = 0; i < length; i++)
            {
                byte b = buffer[i];

                // Count null bytes (strong indicator of binary)
                if (b == 0x00)
                {
                    nullBytes++;
                    continue;
                }

                // Allow common text control characters
                if (b == '\r' || b == '\n' || b == '\t')
                {
                    printableChars++;
                    continue;
                }

                // Count other control characters (0x01-0x1F, excluding allowed ones)
                if (b < 0x20)
                {
                    controlChars++;
                    continue;
                }

                // Count printable ASCII and extended characters
                if ((b >= 0x20 && b <= 0x7E) || b >= 0x80)
                {
                    printableChars++;
                }
            }

            // If more than 1% null bytes, it's likely binary
            if (nullBytes > length / 100)
            {
                errorMessage = "File appears to be binary (contains null bytes)";
                return false;
            }

            // If more than 30% control characters (excluding common text ones), likely binary
            if (controlChars > length * 0.3)
            {
                errorMessage = "File appears to be binary (too many control characters)";
                return false;
            }

            // If less than 70% printable characters, likely binary
            if (printableChars < length * 0.7)
            {
                errorMessage = "File does not appear to contain text content";
                return false;
            }

            return true;
        }

        private void UpdateFileMetadata(string filePath)
        {
        }

        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            StatusUpdate?.Invoke(message, isSucces);
        }

        private static string FormatFileSize(long fileSizeBytes)
        {
            const long kiloByte = 1024;
            const long megaByte = kiloByte * 1024;
            const long gigaByte = megaByte * 1024;

            if (fileSizeBytes >= gigaByte)
            {
                return $"{fileSizeBytes / (double)gigaByte:0.#}GB";
            }

            if (fileSizeBytes >= megaByte)
            {
                return $"{fileSizeBytes / (double)megaByte:0.#}MB";
            }

            return $"{fileSizeBytes / (double)kiloByte:0}KB";
        }
        private static string FormatEstimatedLineCount(string filePath)
        {
            int? estimatedLines = EstimateLineCount(filePath);

            if (!estimatedLines.HasValue)
            {
                return "? lines";
            }

            int value = estimatedLines.Value;

            if (value >= 1_000_000)
            {
                return $"≈ {value / 1_000_000.0:0.#}M lines";
            }

            if (value >= 1_000)
            {
                return $"≈ {value / 1_000.0:0.#}k lines";
            }

            return $"≈ {value} lines";
        }
        private static int? EstimateLineCount(string filePath)
        {
            const int sampleBytes = 64 * 1024;

            FileInfo fileInfo = new(filePath);
            if (fileInfo.Length == 0)
            {
                return 0;
            }

            byte[] buffer = new byte[sampleBytes];
            int newlineCount = 0;
            int bytesReadTotal = 0;
            using (FileStream fileStream = new(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite))
            {
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                for (int index = 0; index < bytesRead; index++)
                {
                    bytesReadTotal++;
                    if (buffer[index] == '\n')
                    {
                        newlineCount++;
                    }
                }
            }

            if (newlineCount == 0)
            {
                return null;
            }
            if (bytesReadTotal < sampleBytes) return newlineCount;

            double averageBytesPerLine = (double)sampleBytes / newlineCount;
            return (int)(fileInfo.Length / averageBytesPerLine);
        }
        private static string FormatModified(DateTime modifiedUtc)
        {
            return modifiedUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        }
        private string GetDefaultOpenFolder()
        {
            string downloadsFolder;
            if (!string.IsNullOrWhiteSpace(txtFilePath.Text) && !IsPlaceholderActive())
            {
                downloadsFolder = Path.GetDirectoryName(txtFilePath.Text);
                if (Directory.Exists(downloadsFolder))
                {
                    return downloadsFolder;
                }
            }
            downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (Directory.Exists(downloadsFolder))
            {
                return downloadsFolder;
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }
    }
}