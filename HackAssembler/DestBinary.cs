using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    class DestBinary
    {
         Dictionary<string, string> binary;
        public DestBinary()
        {
            binary = new Dictionary<string, string>()
        {
            { "null", "000"},
            { "M", "001"},
            { "D", "010"},
            { "MD", "011"},
            { "A", "100"},
            { "AM", "101"},
            { "AD", "110"},
            { "AMD", "111"}

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
