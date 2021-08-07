using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JackCompiler.Tests
{
    /// <summary>
    /// Providemethod to determine the type of given token
    /// </summary>
    public class TokenHelperTests
    {

        private const string KEYWORD = "KEYWORD";
        private const string SYMBOL = "SYMBOL";
        private const string INT = "INT_CONSTANT";
        private const string STRING = "STRING_CONST";
        private const string IDENTIFIER = "IDENTIFIER";

        [Theory]
        [InlineData(("class"))]
        [InlineData(("constructor"))]
        [InlineData(("function"))]
        [InlineData(("method"))]
        [InlineData(("field"))]
        [InlineData(("static"))]
        [InlineData(("int"))]
        [InlineData(("char"))]
        [InlineData(("boolean"))]
        [InlineData(("void"))]
        [InlineData(("true"))]
        [InlineData(("false"))]
        [InlineData(("null"))]
        [InlineData(("this"))]
        [InlineData(("let"))]
        [InlineData(("do"))]
        [InlineData(("if"))]
        [InlineData(("else"))]
        [InlineData(("while"))]
        [InlineData(("return"))]
        public void TokenType_Returns_Keyword(string token)
        {
            var tokentype = TokenHelper.TokenType(token);
            string expected = KEYWORD;
            Assert.Equal(expected, tokentype);
        }

        [Theory]
        [InlineData(("{"))]
        [InlineData(("}"))]
        [InlineData(("("))]
        [InlineData((")"))]
        [InlineData(("["))]
        [InlineData(("]"))]
        [InlineData(("."))]
        [InlineData((","))]
        [InlineData((";"))]
        [InlineData(("+"))]
        [InlineData(("-"))]
        [InlineData(("*"))]
        [InlineData(("/"))]
        [InlineData(("&"))]
        [InlineData(("|"))]
        [InlineData(("<"))]
        [InlineData((">"))]
        [InlineData(("="))]
        [InlineData(("~"))]
        public void TokenType_Returns_Symbol(string token)
        {
            var tokentype = TokenHelper.TokenType(token);
            string expected = SYMBOL;
            Assert.Equal(expected, tokentype);
        }


        [Theory]
        [InlineData(("test"))]
        [InlineData(("a"))]
        [InlineData(("a123"))]
        [InlineData(("_input"))]
        public void TokenType_Returns_Identifier(string token)
        {
            var tokentype = TokenHelper.TokenType(token);
            string expected = IDENTIFIER;
            Assert.Equal(expected, tokentype);
        }

        [Theory]
        [InlineData((@"""test"""))]
        public void TokenType_Returns_StringConstant(string token)
        {
            var tokentype = TokenHelper.TokenType(token);
            string expected = STRING;
            Assert.Equal(expected, tokentype);
        }

        [Theory]
        [InlineData(("0"))]
        [InlineData(("32767"))]
        public void TokenType_Returns_IntigerConstant(string token)
        {
            var tokentype = TokenHelper.TokenType(token);
            string expected = INT;
            Assert.Equal(expected, tokentype);
        }

    }
}
