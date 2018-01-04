using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Core.Builder.Internal;
using Core.Models;
using Core.Util;

namespace Core.IO.Internal
{
    [Serializable]
    public class XmlValidationException : Exception
    {
        public XmlValidationException() { }
        public XmlValidationException(string message) : base(message) { }
        public XmlValidationException(string message, Exception inner) : base(message, inner) { }
        protected XmlValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class XmlParsingException : Exception
    {
        public XmlParsingException() { }
        public XmlParsingException(string message) : base(message) { }
        public XmlParsingException(string message, Exception inner) : base(message, inner) { }
        protected XmlParsingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [SheetReader("(.mxl|.xml)", nameof(XMLSheetReader))]
    public class XMLSheetReader : ISheetReader
    {
        /// <summary>Path to the MIDI File</summary>
        string filePath;

        public Task<Sheet> ReadFromFileAsync()
        {
            var builder = new XMLSheetBuilder();
            var file = Open();
            
            builder.AddXPathNavigator(file.CreateNavigator());
            return Task.FromResult(builder.Build());
        }

        public void SetFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException("The given path does not exist", nameof(path));

            filePath = path;
        }

        XDocument Open()
        {
            bool isZipped = filePath.EndsWith(".mxl", StringComparison.InvariantCultureIgnoreCase);
            Stream file = null;
            XmlReader reader = null;
            XDocument doc = null;

            try
            {
                if (isZipped)
                {
                    file = Zip.OpenStreamFromArchiveWhere(
                        filePath,
                        x =>
                        {
                            return x.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase)
                                && !x.StartsWith("META-INF", StringComparison.InvariantCultureIgnoreCase);
                        }
                    );
                }
                else
                {
                    file = new FileStream(filePath, FileMode.Open);
                }

                reader = XmlReader.Create(file, new XmlReaderSettings() { Async = true, DtdProcessing = DtdProcessing.Parse });

                doc = XDocument.Load(reader);
            }
            catch (Exception ex)
            {
                throw new XmlParsingException("An error occured while parsing", ex);
            } finally
            {
                file?.Close();
                reader?.Close();
            }

            return doc;
        }
    }
}
