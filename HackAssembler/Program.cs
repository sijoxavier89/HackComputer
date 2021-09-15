using System;
using System.IO;
using System.Threading.Tasks;

namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Trnslation started");
            string file = "Rect";//args[0];
            string filepath = @"C:\Users\sijox\source\repos\HackAssembler\InputFiles\" + file+".asm";
            StreamReader streamReader = new StreamReader(filepath);

            // Symbol traslation
            var st = new SymbolTable();
            var translater = new SymbolTranslater(filepath, st);

            StreamReader streamReader1 = new StreamReader(filepath);

            Parser parser = new Parser(streamReader1);
            Code code = new Code();

            // clear file
            string outfile = @"C:\Users\sijox\source\repos\HackAssembler\Output\" + file + ".hack";
            File.WriteAllText(outfile, string.Empty);


            while (parser.HasMoreCommands())
            {
                parser.Advance();
                string result;
                string cType = parser.CommandType();
                string  symbol;
                string comp;
                string dest;
                string jump;

                if (cType.Equals("A"))
                {
                    result = parser.Symbol();
                    var address = string.Empty;
                    if(!int.TryParse(result, out int value))
                    {
                        address = st.Getaddress(result).ToString();
                    }
                    else
                    {
                        address = result;
                    }
                    var instruction = AInstruction(address);
                    WriteFileAsync(instruction,outfile).Wait();
                }

                if (cType.Equals("C"))
                {
                    dest = code.Dest(parser.Dest());
                    comp = code.Comp(parser.Comp());
                    jump = code.Jump(parser.Jump());


                    var cinstruction = CInstruction(comp, dest, jump);
                    WriteFileAsync(cinstruction,outfile).Wait();
                   
                }
            }

            Console.WriteLine("Trnslation completed");
        }

        //write
        public static async Task WriteFileAsync(string code, string path)
        {
           

            using StreamWriter file = new(path, append: true);
         
            await file.WriteLineAsync(code);
        }

        public static string AInstruction(string label)
        {
            string zeros = "0000000000000000";
            Int32.TryParse(label, out int num);

            string binaryString = Convert.ToString(num, 2);
            string prefix = zeros.Substring(0, 16 - binaryString.Length);
            string a = prefix + binaryString;
            return a;
        }

        public static string CInstruction(string comp, string dest, string jump= "000")
        {
            string prefix = "111";
            return prefix + comp + dest+jump;
        }
    }
}
