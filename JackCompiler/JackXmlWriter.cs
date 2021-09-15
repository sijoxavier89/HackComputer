using System.Text;
using System.Xml;

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
                Encoding = new UTF8Encoding(false),
                ConformanceLevel = System.Xml.ConformanceLevel.Auto,
              
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
 
            //doc.Save(writer);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

       
    }
}
