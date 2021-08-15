using System.Text;

namespace JackCompiler
{

    public class JackXmlWriter
    {
        System.Xml.XmlWriter writer;
       
        public JackXmlWriter(string outputFile)
        {
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = true,
                Encoding = System.Text.Encoding.UTF32,
                ConformanceLevel = System.Xml.ConformanceLevel.Auto
            };

            writer = System.Xml.XmlTextWriter.Create(outputFile, settings);
            writer.WriteStartDocument();
        }

        public System.Xml.XmlWriter Writer
        {
            get
            {
                return writer;
            }
        }

        public void AddElement(string element, string value)
        {
            writer.WriteElementString(element, value);
        }

        public void WriteRaw(string data)
        {
            writer.WriteRaw(data);
        }
        public void AddStartElement(string element)
        {
            writer.WriteStartElement(element);
        }

        public void CloseElement()
        {
            writer.WriteFullEndElement();
        }
        public void Close()
        {
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }
    }
}
