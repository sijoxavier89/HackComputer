using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    public class SymbolTranslater
    {
        
        private int counter = -1;
        private int varcounter = 16;
        Parser parser;
        StreamReader streamReader;
        SymbolTable st;
        string path;
        public SymbolTranslater(string path, SymbolTable st)
        {
            this.st = st;
             InitializeST();
            this.path = path;
            FirstPass();
            SecondPass();
        }

        /// <summary>
        /// initialize symbol table with
        /// predefined values
        /// </summary>
        private void InitializeST()
        {
           
            st.AddEntry("SP", 0);
            st.AddEntry("LCL",1);
            st.AddEntry("ARG", 2);
            st.AddEntry("THIS", 3);
            st.AddEntry("THAT", 4);
            st.AddEntry("R0", 0);
            st.AddEntry("R1", 1);
            st.AddEntry("R2", 2);
            st.AddEntry("R3", 3);
            st.AddEntry("R4", 4);
            st.AddEntry("R5", 5);
            st.AddEntry("R6", 6);
            st.AddEntry("R7", 7);
            st.AddEntry("R8", 8);
            st.AddEntry("R9", 9);
            st.AddEntry("R10", 10);
            st.AddEntry("R11", 11);
            st.AddEntry("R12", 12);
            st.AddEntry("R13", 13);
            st.AddEntry("R14", 14);
            st.AddEntry("R15", 15);
            st.AddEntry("SCREEN", 16384);
            st.AddEntry("KBD", 24576);
        }

        /// <summary>
        /// translate symbols
        /// </summary>
        private void FirstPass()
        {
            parser = new Parser(new StreamReader(path));
            while (parser.HasMoreCommands())
            {
                parser.Advance();               
                string cType = parser.CommandType();
                
                if (cType.Equals("L"))
                {
                    st.AddEntry(parser.Symbol(), counter+1);
                }

                if (cType.Equals("C") | cType.Equals("A"))
                {
                    counter++;

                }
            }
        }

        /// <summary>
        /// translate variables
        /// </summary>
        private void SecondPass()
        {
            parser = new Parser(new StreamReader(path));
            while (parser.HasMoreCommands())
            {
                parser.Advance();
                string cType = parser.CommandType();

                if (cType.Equals("A") && !IsNumber(parser.Symbol()))
                {
                   if(!st.Contains(parser.Symbol()))
                    {
                        st.AddEntry(parser.Symbol(), varcounter);
                        varcounter++;
                    }
                }

               
            }
        }

        private bool IsNumber(string symbol)
        {
            return int.TryParse(symbol, out int value);
        }

      
    }
}
