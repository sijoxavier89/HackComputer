using System;
using System.IO;

namespace JackCompilerFinal
{
    class JackCompiler
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Compiler started");

            string file = args[0];
          
            CompilationEngine compiler;
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
                    string outputFIle = item.Split('.')[0] + ".xml";
                    compiler = new CompilationEngine(streamInput, outputFIle);
                    compiler.CompileClass();

                }

            }

            Console.WriteLine("Compilation completed");
        }
    }
    
}
