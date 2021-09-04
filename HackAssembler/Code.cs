using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    public class Code
    {
        DestBinary destbinary;
        CompBinary compbinary;
        JumpBinary jumpbinary;
        public Code()
        {
            destbinary = new DestBinary();
            compbinary = new CompBinary();
            jumpbinary = new JumpBinary();
        }

        /// <summary>
        /// returns binary string of dest
        /// </summary>
        /// <returns></returns>
        public string Dest(string dest)
        {

            return destbinary.Binary[dest];
        }

        public string Comp(string comp)
        {
            return compbinary.Binary[comp];
        }

        public string Jump(string jump)
        {
            return jumpbinary.Binary[jump];
        }
    }
}
