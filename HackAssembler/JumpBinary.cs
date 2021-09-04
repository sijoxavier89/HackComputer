using System.Collections.Generic;

namespace HackAssembler
{
    class JumpBinary
    {
       
         Dictionary<string, string> binary;
        public JumpBinary()
        {
            binary = new Dictionary<string, string>()
        {
            { "null", "000"},
            { "JGT", "001"},
            { "JEQ", "010"},
            { "JGE", "011"},
            { "JLT", "100"},
            { "JNE", "101"},
            { "JLE", "110"},
            { "JMP", "111"}

        };
        }


        public Dictionary<string, string> Binary
        {
            get
            {
                return binary;
            }
        }

    }
}
