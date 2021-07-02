using System;
using System.IO;
using Xunit;

namespace HackVMTranslater.Tests
{
    public class ParserTests
    {
       Parser parser;
        private const string C_ARITHMETIC = "C_ARITHMETIC";
        private const string C_PUSH = "C_PUSH";
        private const string C_POP = "C_POP";
        private const string C_LABEL = "C_LABEL";
        private const string C_GOTO = "C_GOTO";
        private const string C_FUNCTION = "C_FUNCTION";
        private const string C_IF = "C_IF";
        private const string C_RETURN = "C_RETURN";
        private const string C_CALL = "C_CALL";
        public ParserTests()
        {
            parser = new Parser(GetFile());
        }

        [Fact]
        public void ShouldLoadFiles()
        {
            Assert.True(parser.HasMoreCommands());
        }

        private StreamReader GetFile()
        {

            StreamReader streamReader = new StreamReader(@"C:\Users\sijox\source\repos\HackVMTranslater.Tests\InputFiles\BasicTest.vm");
            return streamReader;
        }

       

        [Fact]
        public void Should_ReadAllCommands()
        {
            // Arrange
            parser = new Parser(GetFile());
            int expectedCount = 25;

            int actualCount = 0;

            // Act and Assert
            while (parser.HasMoreCommands())
            {
                parser.Advance();
                string arg1;
                int arg2 = -1;
                string cType = parser.CommandType();
                if (!cType.Equals(C_RETURN))
                {
                    arg1 = parser.Arg1();
                    Assert.NotNull(arg1);


                    if (cType.Equals(C_PUSH) || cType.Equals(C_POP) || cType.Equals(C_FUNCTION) || cType.Equals(C_CALL))
                    {
                        arg2 = parser.Arg2();
                        Assert.NotEqual(-1, arg2);
                    }
                }
                else
                {
                    Assert.Equal(C_RETURN,cType);
                }
                actualCount++;
            }

            Assert.Equal(expectedCount, actualCount);

        }
    }
}
