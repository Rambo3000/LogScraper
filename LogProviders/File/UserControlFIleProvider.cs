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
        private const int SampleSizeForValidation = 8192; // 8KB
        private const int SampleSizeForLineEstimate = 64 * 1024; // 64KB
        private const long MaxFileSizeBytes = 500L * 1024 * 1024; // 500MB

        private static readonly Color PlaceholderColor = Color.Gray;
        private static readonly Color NormalTextColor = SystemColors.WindowText;

        public event EventHandler SourceSelectionChanged;
        public event Action<string, bool> StatusUpdate;

        public UserControlFileLogProvider()
        {
            InitializeComponent();
            InitializeFilePath();
        }

        public ISourceAdapter GetSourceAdapter()
        {
            return IsPlaceholderActive() ? null : SourceAdapterFactory.CreateFileSourceAdapter(txtFilePath.Text);
        }

        protected virtual void OnSourceSelectionChanged()
        {
            SourceSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStatusUpdate(string message, bool isSuccess)
        {
            StatusUpdate?.Invoke(message, isSuccess);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            string selectedFilePath = ShowFileDialog();
            if (string.IsNullOrEmpty(selectedFilePath))
                return;

            SetNormalText(selectedFilePath);

            if (!ValidateFile(selectedFilePath, out string errorMessage))
            {
                HandleInvalidFile(errorMessage);
                return;
            }

            UpdateFileMetadataDisplay(selectedFilePath);
            OnStatusUpdate("OK", true);
        }

        #region File Path Management

        private void InitializeFilePath()
        {
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

        #endregion

        #region File Dialog

        private string ShowFileDialog()
        {
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "Text Files (*.txt;*.csv;*.tsv;*.log;*.xml;*.json)|*.txt;*.csv;*.tsv;*.log;*.xml;*.json|All Files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = GetDefaultOpenFolder()
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
        }

        private string GetDefaultOpenFolder()
        {
            if (!string.IsNullOrWhiteSpace(txtFilePath.Text) && !IsPlaceholderActive())
            {
                string currentDirectory = Path.GetDirectoryName(txtFilePath.Text);
                if (Directory.Exists(currentDirectory))
                    return currentDirectory;
            }

            string downloadsFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            return Directory.Exists(downloadsFolder)
                ? downloadsFolder
                : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        #endregion

        #region File Validation

        private static bool ValidateFile(string filePath, out string errorMessage)
        {
            if (!System.IO.File.Exists(filePath))
            {
                errorMessage = "File does not exist";
                return false;
            }

            try
            {
                FileInfo fileInfo = new(filePath);

                if (fileInfo.Length == 0)
                {
                    errorMessage = "File is empty";
                    return false;
                }

                if (IsZipArchive(filePath))
                {
                    errorMessage = "ZIP archives are not supported";
                    return false;
                }

                return ValidateFileContent(filePath, fileInfo.Length, out errorMessage);
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

        private static bool IsZipArchive(string filePath)
        {
            try
            {
                using FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                // ZIP files start with PK signature (0x50 0x4B)
                // Common ZIP signatures:
                // 0x50 0x4B 0x03 0x04 - Local file header
                // 0x50 0x4B 0x05 0x06 - End of central directory
                // 0x50 0x4B 0x07 0x08 - Spanned archive

                if (stream.Length < 4)
                    return false;

                byte[] header = new byte[4];
                stream.ReadExactly(header, 0, 4);

                // Check for PK signature
                return header[0] == 0x50 && header[1] == 0x4B;
            }
            catch
            {
                // If we can't read the file, let other validation handle it
                return false;
            }
        }

        private static bool ValidateFileContent(string filePath, long fileLength, out string errorMessage)
        {
            using FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            int bytesToRead = (int)Math.Min(SampleSizeForValidation, fileLength);
            byte[] buffer = new byte[bytesToRead];
            int bytesRead = stream.Read(buffer, 0, bytesToRead);

            return IsTextContent(buffer, bytesRead, out errorMessage);
        }

        private static bool IsTextContent(byte[] buffer, int length, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (length == 0)
            {
                errorMessage = "File is empty";
                return false;
            }

            var (NullBytes, ControlChars, PrintableChars) = AnalyzeBufferContent(buffer, length);

            if (NullBytes > length / 100)
            {
                errorMessage = "File appears to be binary (contains null bytes)";
                return false;
            }

            if (ControlChars > length * 0.3)
            {
                errorMessage = "File appears to be binary (too many control characters)";
                return false;
            }

            if (PrintableChars < length * 0.7)
            {
                errorMessage = "File does not appear to contain text content";
                return false;
            }

            return true;
        }

        private static (int NullBytes, int ControlChars, int PrintableChars) AnalyzeBufferContent(byte[] buffer, int length)
        {
            int nullBytes = 0;
            int controlChars = 0;
            int printableChars = 0;

            for (int i = 0; i < length; i++)
            {
                byte b = buffer[i];

                if (b == 0x00)
                {
                    nullBytes++;
                }
                else if (b == '\r' || b == '\n' || b == '\t')
                {
                    printableChars++;
                }
                else if (b < 0x20)
                {
                    controlChars++;
                }
                else if ((b >= 0x20 && b <= 0x7E) || b >= 0x80)
                {
                    printableChars++;
                }
            }

            return (nullBytes, controlChars, printableChars);
        }

        #endregion

        #region UI Updates

        private void HandleInvalidFile(string errorMessage)
        {
            LblFileSizeValue.Text = "-";
            LblModificationDateTimeValue.Text = "-";
            PctWarning.Visible = false;
            OnStatusUpdate(errorMessage, false);
        }

        private void UpdateFileMetadataDisplay(string filePath)
        {
            try
            {
                FileInfo fileInfo = new(filePath);

                LblFileSizeValue.Text = FormatFileSize(fileInfo.Length) + " " + FormatEstimatedLineCount(filePath);
                LblModificationDateTimeValue.Text = FormatModified(fileInfo.LastWriteTimeUtc);

                bool sizeIsTooLarge = fileInfo.Length > MaxFileSizeBytes;
                PctWarning.Visible = sizeIsTooLarge;

                if (!sizeIsTooLarge)
                    OnSourceSelectionChanged();
            }
            catch (Exception ex)
            {
                LblFileSizeValue.Text = $"Error reading metadata: {ex.Message}";
                LblModificationDateTimeValue.Text = string.Empty;
                PctWarning.Visible = false;
            }
        }

        #endregion

        #region Formatting

        private static string FormatFileSize(long fileSizeBytes)
        {
            const long kiloByte = 1024;
            const long megaByte = kiloByte * 1024;
            const long gigaByte = megaByte * 1024;

            if (fileSizeBytes >= gigaByte)
                return $"{fileSizeBytes / (double)gigaByte:0.#}GB";

            if (fileSizeBytes >= megaByte)
                return $"{fileSizeBytes / (double)megaByte:0.#}MB";

            return $"{fileSizeBytes / (double)kiloByte:0}KB";
        }

        private static string FormatEstimatedLineCount(string filePath)
        {
            int? estimatedLines = EstimateLineCount(filePath);

            if (!estimatedLines.HasValue)
                return "? lines";

            int value = estimatedLines.Value;

            if (value >= 1_000_000)
                return $"≈ {value / 1_000_000.0:0.#}M lines";

            if (value >= 1_000)
                return $"≈ {value / 1_000.0:0.#}k lines";

            return $"≈ {value} lines";
        }

        private static int? EstimateLineCount(string filePath)
        {
            FileInfo fileInfo = new(filePath);
            if (fileInfo.Length == 0)
                return 0;

            byte[] buffer = new byte[SampleSizeForLineEstimate];
            int newlineCount = 0;
            int bytesReadTotal = 0;

            using (FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                for (int index = 0; index < bytesRead; index++)
                {
                    bytesReadTotal++;
                    if (buffer[index] == '\n')
                        newlineCount++;
                }
            }

            if (newlineCount == 0)
                return null;

            if (bytesReadTotal < SampleSizeForLineEstimate)
                return newlineCount;

            double averageBytesPerLine = (double)SampleSizeForLineEstimate / newlineCount;
            return (int)(fileInfo.Length / averageBytesPerLine);
        }

        private static string FormatModified(DateTime modifiedUtc)
        {
            return modifiedUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        }

        #endregion
    }
}