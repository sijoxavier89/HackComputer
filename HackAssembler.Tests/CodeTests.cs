using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HackAssembler.Tests
{
    public class CodeTests
    {
        Code code;
        public CodeTests()
        {
            code = new Code();
        }

        [Fact]
        public void CheckDest()
        {
            string symbol = "null";

            Assert.Equal("000",code.Dest(symbol));

            symbol = "AMD";

            Assert.Equal("111", code.Dest(symbol));
        }

        [Fact]
        public void CheckComp()
        {
            string symbol = "M+1";

            Assert.Equal("110111", code.Comp(symbol));

            symbol = "D|M";

            Assert.Equal("010101", code.Comp(symbol));
        }

        [Fact]
        public void CheckJump()
        {
            string symbol = "JMP";

            Assert.Equal("111", code.Jump(symbol));

            symbol = "JLE";

            Assert.Equal("110", code.Jump(symbol));
        }
    }
}
