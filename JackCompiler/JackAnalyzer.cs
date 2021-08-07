using System;
using System.Collections.Generic;
using System.IO;

namespace JackCompiler
{
    public class JackAnalyzer
    {
        private static Dictionary<string, string> xmltoken = new Dictionary<string, string>()
        {
            { "&", "&amp;" },
            { "<", "&lt;" },
            { ">", "&gt;" }

        };
        static void Main(string[] args)
        {
            Console.WriteLine("Tokenizer started");

            string file = args[0];
            string outFile = file.Split('.')[0];

            JackTokenizer tokenizer;
            StreamReader streamReader;
            string filepath;

            if (file.EndsWith(".jack")) // file
            {
                Console.WriteLine("Transalting file:" + file);

                
                filepath = file;
                streamReader = new StreamReader(filepath);

                tokenizer = new JackTokenizer(streamReader);


                // process file line by line
                string outputFIle = outFile + "T.xml";
                WriteXML(outputFIle, tokenizer);
            }
            else // directory
            {
                Console.WriteLine("Transalting directory:" + file);

                var files = Directory.GetFiles(file, "*.jack");


                foreach (var item in files)
                {
                    string fileItem = Path.GetFileName(item);
                    streamReader = new StreamReader(item);
                    tokenizer = new JackTokenizer(streamReader);
                    string outputFIle = fileItem.Split('.')[0] + "T.xml";
                    // process file line by line
                    WriteXML(outputFIle, tokenizer);
                }

            }

            Console.WriteLine("Tokenizer ended");
        }

        public static void WriteXML(string outputFile, JackTokenizer tokenizer)
        {
            // Create an XmlWriterSettings object with the correct options.
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    "; //  "\t";
            settings.OmitXmlDeclaration = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Auto;
      

            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputFile, settings))
            {

                writer.WriteStartDocument();

                string tokenType;
                string tokenTypeElement = string.Empty;
                string tokenValue = string.Empty;

                writer.WriteStartElement("tokens");
                while (tokenizer.HasMoreTokens()) 
                {
                    tokenizer.Advance();

                    tokenType = tokenizer.TokenType().ToLower();

                    if(tokenType.Equals("keyword"))
                    {
                        tokenTypeElement = "keyword";
                        tokenValue = tokenizer.KeyWord().ToLower();
                    }
                    else if(tokenType.Equals("symbol"))
                    {
                        tokenTypeElement = "symbol";
                        tokenValue = tokenizer.Symbol().ToString();//ModifyTokenValue(tokenizer.Symbol().ToString());
                    }
                    else if (tokenType.Equals("identifier"))
                    {
                        tokenTypeElement = "identifier";
                        tokenValue = tokenizer.Identifier();
                    }
                    else if (tokenType.Equals("int_constant"))
                    {
                        tokenTypeElement = "integerConstant";
                        tokenValue = Convert.ToString(tokenizer.IntVal());
                    }
                    else if (tokenType.Equals("string_constant"))
                    {
                        tokenTypeElement = "stringConstant";
                        tokenValue = tokenizer.StringVal(); //ModifyTokenValue(tokenizer.StringVal().ToString(), true);
                    }

                  
                    writer.WriteElementString(tokenTypeElement, tokenValue);
                  
                }
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            } // End Using writer 

           

        }
        private static string ModifyTokenValue(string tokenValue, bool isStringConst = false)
        {
           
            if (!isStringConst && xmltoken.ContainsKey(tokenValue))
            {
                return xmltoken[tokenValue];
            }

            return tokenValue.Replace("\"", "&quote;");
        }
    }
}
