using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JackCompiler.Tests
{
    public class JackTokenizerTests
    {
        JackTokenizer tokenizer;
            
        [Fact]
        public void SeparateTokens()
        {
            string filepath = @"C:\Users\sijox\source\repos\JackCompiler.Tests\Square.jack"; 
             StreamReader streamReader = new StreamReader(filepath);
            tokenizer = new JackTokenizer(streamReader);
        }

        [Fact]
        public void TokenizeArray()
        {
            string filepath = @"C:\Users\sijox\source\repos\JackCompiler.Tests\Main.jack";
            StreamReader streamReader = new StreamReader(filepath);
            tokenizer = new JackTokenizer(streamReader);

            while(tokenizer.HasMoreTokens())
            {
                tokenizer.Advance();
                tokenizer.TokenType();
            }
        }
    }
}
