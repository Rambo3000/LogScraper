
using System;
using System.IO;
using System.Text;
using System.Xml;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors.Implementations
{
    public class XmlPostProcessor : ILogPostProcessor
    {
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

            return TryPrettyPrint(text[possibleStartIndex..possibleEndIndex], out prettyPrintedXml);
        }
        private static readonly XmlReaderSettings readerSettings = new()
        {
            ConformanceLevel = ConformanceLevel.Fragment,
            IgnoreComments = false,
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Ignore
        };

        private static readonly XmlWriterSettings writerSettings = new()
        {
            Indent = true,
            IndentChars = "  ",
            OmitXmlDeclaration = true,
            NewLineHandling = NewLineHandling.Replace
        };

        private static bool TryPrettyPrint(string text, out string prettyPrintedXml)
        {
            prettyPrintedXml = null;

            try
            {
                StringBuilder stringBuilder = new();

                using (StringReader stringReader = new(text))
                using (XmlReader xmlReader = XmlReader.Create(stringReader, readerSettings))
                using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, writerSettings))
                {
                    while (xmlReader.Read())
                    {
                        xmlWriter.WriteNode(xmlReader, true);
                    }
                }

                prettyPrintedXml = stringBuilder.ToString();
                return true;
            }
            catch (XmlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
