
using System.IO;
using System.Text;
using System.Xml;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors.Implementations
{
    /// <summary>
    /// Provides functionality to detect and pretty-print XML content within log entries.
    /// </summary>
    /// <remarks>The XmlPostProcessor attempts to identify XML fragments in log entries and format them for
    /// improved readability. It implements the ILogPostProcessor interface, allowing integration with logging
    /// frameworks that support post-processing of log data. This class is typically used to enhance the presentation of
    /// XML data in logs, making it easier to review and debug complex log output.</remarks>
    public class XmlPostProcessor : ILogPostProcessor
    {
        /// <summary>
        /// Attempts to extract and pretty-print an XML fragment from the specified log entry.
        /// </summary>
        /// <remarks>This method searches for an XML fragment within the log entry's content, starting at
        /// the specified index. If a suitable fragment is found, it is formatted for readability. The method does not
        /// throw exceptions for invalid or missing XML; instead, it returns false and sets the output parameter to
        /// null.</remarks>
        /// <param name="logEntry">The log entry to process. Must not be null. The method examines the entry's content to locate and format an
        /// XML fragment.</param>
        /// <param name="prettyPrintedXml">When this method returns, contains the pretty-printed XML fragment if extraction and formatting succeed;
        /// otherwise, null.</param>
        /// <returns>true if a valid XML fragment is found and successfully pretty-printed; otherwise, false.</returns>
        public bool TryProcess(LogEntry logEntry, out string prettyPrintedXml)
        {
            prettyPrintedXml = null;

            string text = logEntry.Entry;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            int possibleStartIndex = text.IndexOf('<', logEntry.StartIndexContent);
            if (possibleStartIndex < 0) return false;

            int possibleEndIndex = text.LastIndexOf('>');
            if (possibleEndIndex < possibleStartIndex) return false;

            int possibleBackSlash = logEntry.Entry.IndexOf('/', possibleStartIndex + 1, possibleEndIndex - possibleStartIndex - 2);
            if (possibleBackSlash < 0) return false;

            return TryPrettyPrint(text[possibleStartIndex..(possibleEndIndex + 1)], out prettyPrintedXml);
        }

        /// <summary>
        /// Provides default settings for XML writers that produce indented XML without an XML declaration.
        /// </summary>
        /// <remarks>These settings configure XML output to use tab characters for indentation, omit the
        /// XML declaration, and replace new line characters. Intended for scenarios where human-readable,
        /// declaration-free XML is required.</remarks>
        private static readonly XmlWriterSettings writerSettings = new()
        {
            Indent = true,
            IndentChars = "\t",
            OmitXmlDeclaration = true,
            NewLineHandling = NewLineHandling.Replace
        };

        /// <summary>
        /// Attempts to format the specified XML string with indentation and line breaks for improved readability.
        /// </summary>
        /// <remarks>This method does not throw exceptions for invalid XML input. If the input cannot be
        /// parsed as XML, the method returns false and the output parameter is set to null.</remarks>
        /// <param name="text">The XML string to format. This can be a complete XML document or a fragment.</param>
        /// <param name="prettyPrintedXml">When this method returns, contains the formatted XML string if formatting was successful; otherwise, null.</param>
        /// <returns>true if the XML string was successfully formatted; otherwise, false.</returns>
        private static bool TryPrettyPrint(string text, out string prettyPrintedXml)
        {
            prettyPrintedXml = null;

            try
            {
                XmlDocument xmlDocument = new();

                try
                {
                    // First try full document
                    xmlDocument.LoadXml(text);
                }
                catch (XmlException)
                {
                    // Fallback to fragment
                    LoadFragment(xmlDocument, text);
                }

                StringBuilder stringBuilder = new();

                using (StringWriter stringWriter = new(stringBuilder))
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
                {
                    xmlDocument.Save(xmlWriter);
                }

                prettyPrintedXml = stringBuilder.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads an XML fragment from the specified text and appends its nodes to a new root element in the provided
        /// XmlDocument.
        /// </summary>
        /// <remarks>The method creates a new root element named "Fragment" in the XmlDocument and appends
        /// all nodes parsed from the input text as its children. Existing content in the XmlDocument will be
        /// replaced.</remarks>
        /// <param name="xmlDocument">The XmlDocument to which the parsed XML fragment will be added. Must not be null.</param>
        /// <param name="text">The string containing the XML fragment to load. Cannot be null.</param>
        private static void LoadFragment(XmlDocument xmlDocument, string text)
        {
            XmlReaderSettings settings = new()
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using StringReader stringReader = new(text);
            using XmlReader reader = XmlReader.Create(stringReader, settings);
            XmlElement root = xmlDocument.CreateElement("Fragment");
            xmlDocument.AppendChild(root);

            while (reader.Read())
            {
                XmlNode node = xmlDocument.ReadNode(reader);

                if (node != null) root.AppendChild(node);
            }
        }
    }
}
