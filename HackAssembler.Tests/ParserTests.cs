using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace HackAssembler.Tests
{
    public class ParserTests
    {
        Parser parser;
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
          
            StreamReader streamReader = new StreamReader(@"C:\Users\sijox\source\repos\HackAssembler.Tests\InputFiles\Max.asm");
            return streamReader;
        }

        [Fact]
        public void Should_AdavanceToNextCommand()
        {
          if(parser.HasMoreCommands())
            {
                parser.Advance();
                string cType = parser.CommandType();
                Assert.Equal("A", cType);
            }
            // second line
            if (parser.HasMoreCommands())
            {
                parser.Advance();
                string cType = parser.CommandType();
                Assert.Equal("C", cType);
            }
        }

        [Fact]
        public void Should_ReadAllCommands()
        {
            int expectedCount = 6;

            int actualCount=0;
            while(parser.HasMoreCommands())
            {
                parser.Advance();
                string result;
                string cType = parser.CommandType();
                if(cType.Equals("A"))
                {
                    result = parser.Symbol();
                    Assert.NotNull(result);
                }

                if(cType.Equals("C"))
                {
                    result = parser.Dest();
                    Assert.NotNull(result);

                    result = parser.Comp();
                    Assert.NotNull(result);

                    //result = parser.Jump();
                    //Assert.NotNull(result);
                }
                actualCount++;
            }

            Assert.Equal(expectedCount, actualCount);
           
        }
    }
}
