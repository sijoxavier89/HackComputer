using System;
using System.IO;

namespace HackVMTranslater
{
    class VMTranslater
    {
        private const string C_PUSH = "C_PUSH";
        private const string C_POP = "C_POP";
        private const string C_ARITHMETIC = "C_ARITHMETIC";
        private const string C_LABEL = "C_LABEL";
        private const string C_GOTO = "C_GOTO";
        private const string C_IF = "C_IF";
        private const string C_FUNCTION = "C_FUNCTION";
        private const string C_RETURN = "C_RETURN";
        private const string C_CALL = "C_CALL";
        static void Main(string[] args)
        {
            Console.WriteLine("VM Translater started");

            string file = args[0];
            string outFile = file.Split('.')[0];

            CodeWriter codeWriter;
            Parser parser;
            StreamReader streamReader;
            string filepath;

            if (file.EndsWith(".vm")) // file
            {
                Console.WriteLine("Transalting file:" + file);

                codeWriter = new CodeWriter(file);
                 filepath = file;
                 streamReader = new StreamReader(filepath);

                 parser = new Parser(streamReader);


                // process file line by line

                WriteAssembly(parser, codeWriter);
            }
            else // directory
            {
                Console.WriteLine("Transalting directory:"+ file);
                codeWriter = new CodeWriter(outFile);

                var files = Directory.GetFiles(file, "*.vm");

                // Bootstrap
                codeWriter.WriteInit();

                foreach (var item in files)
                {
                    string fileItem = Path.GetFileName(item);
                    codeWriter.SetFileName(fileItem);
                    streamReader = new StreamReader(item);

                    parser = new Parser(streamReader);

                    // process file line by line

                   WriteAssembly(parser, codeWriter);
                }
                 
            }

           
            // close
             codeWriter.Close();
             Console.WriteLine("VM Translater ended");
        }

        private static void WriteAssembly(Parser parser, CodeWriter codeWriter)
        {
            while (parser.HasMoreCommands())
            {
                parser.Advance();

                if (parser.CommandType().Equals(C_PUSH) || parser.CommandType().Equals(C_POP))
                {
                    codeWriter.WritePushPop(parser.CommandType(), parser.Arg1(), parser.Arg2());
                }
                else if (parser.CommandType().Equals(C_ARITHMETIC)) // Arithmetic
                {
                    codeWriter.WriteArithmetic(parser.Arg1());
                }
                else if (parser.CommandType().Equals(C_LABEL))
                {
                    codeWriter.WriteLabel(parser.Arg1());
                }
                else if (parser.CommandType().Equals(C_IF))
                {
                    codeWriter.WriteIf(parser.Arg1());
                }
                else if (parser.CommandType().Equals(C_GOTO))
                {
                    codeWriter.WriteGoto(parser.Arg1());
                }
                else if (parser.CommandType().Equals(C_CALL))
                {
                    codeWriter.WriteCall(parser.Arg1(), parser.Arg2());
                }
                else if (parser.CommandType().Equals(C_FUNCTION))
                {
                    codeWriter.WriteFunction(parser.Arg1(),parser.Arg2());
                }
                else if (parser.CommandType().Equals(C_RETURN))
                {
                    codeWriter.WriteReturn();
                }
            }
        }
    }
}
