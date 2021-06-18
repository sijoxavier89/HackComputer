using System.IO;
using Xunit;

namespace HackAssembler.Tests
{
    public class SymbolTranslaterTests
    {
        SymbolTranslater symbolTranslater;
        public SymbolTranslaterTests()
        {
                
        }

        [Fact]
        public void Should_Translate()
        {
            string path = @"C:\Users\sijox\source\repos\HackAssembler.Tests\InputFiles\Max.asm";
            var st = new SymbolTable();
            var translater = new SymbolTranslater(path, st);

            Assert.Equal(0,st.Getaddress("SP"));
            Assert.Equal(10, st.Getaddress("OUTPUT_FIRST"));
            Assert.Equal(12, st.Getaddress("OUTPUT_D"));
            Assert.Equal(14, st.Getaddress("INFINITE_LOOP"));
        }

        [Fact]
        public void Should_Translate_Pong()
        {
            string path = @"C:\Users\sijox\source\repos\HackAssembler.Tests\InputFiles\Pong.asm";
            var st = new SymbolTable();
            var translater = new SymbolTranslater(path, st);

            Assert.Equal(0, st.Getaddress("SP"));
           Assert.True(st.Contains("ponggame.0"));
            int value = st.Getaddress("ponggame.0");
        }
    }
}
