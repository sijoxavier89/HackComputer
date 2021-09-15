using System;
using System.Collections.Generic;
using System.IO;

namespace JackCompiler
{
    public class JackAnalyzer
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Tokenizer started");

            string file = args[0];
            string outFile = file.Split('.')[0];

            JackTokenizer tokenizer;
            CompilationEngine compiler;
            StreamReader streamReader;
            StreamReader streamInput; // test compiler
            string filepath;

            if (file.EndsWith(".jack")) // file
            {
                Console.WriteLine("Transalting file:" + file);

                
                filepath = file;
               // compiler
                streamInput = new StreamReader(filepath);
                string outputFIle = file.Split('.')[0] + ".xml";
                compiler = new CompilationEngine(streamInput, outputFIle);
                compiler.CompileClass();
                
            }
            else // directory
            {
                Console.WriteLine("Transalting directory:" + file);

                var files = Directory.GetFiles(file, "*.jack");


                foreach (var item in files)
                {

                    string fileItem = Path.GetFileName(item);
                    streamInput = new StreamReader(item);

                    // compiler
                    string outputFIle = item.Split('.')[0]+".xml";
                    compiler = new CompilationEngine(streamInput, outputFIle);
                    compiler.CompileClass();

                    // Token -- will be removed
                    string outputTokenFIle = fileItem.Split('.')[0] + "T.xml";

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
       
    }
}
