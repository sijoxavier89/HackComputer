using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    public class TokenHelper
    {
        private const string KEYWORD = "KEYWORD";
        private const string SYMBOL = "SYMBOL";
        private const string INT = "INT_CONSTANT";
        private const string STRING = "STRING_CONSTANT";
        private const string IDENTIFIER = "IDENTIFIER";

        private static HashSet<char> Symbols = new HashSet<char>()
        {
          '{','}','(',')','[',']','.',',',';','+','-','*','/','&','|','<','>','=','~'

        };

        private static HashSet<string> KeyWords = new HashSet<string>()
        {
            "class", "constructor", "function","method", "field", "static", "var", "int", "char", "boolean",
            "void", "true", "false", "null", "this", "let", "do", "if", "else", "while", "return"
        };

       
        /// <summary>
        /// Determine the token type and
        /// return token type
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string TokenType(string token)
        {
            if (IsKeyword(token))
                return KEYWORD;
            if (IsStringConstant(token))
                return STRING;
            if (IsIntiger(token))
                return INT;
            if (IsSymbol(token))
                return SYMBOL;

            return IDENTIFIER;
        }

        private static bool IsStringConstant(string token)
        {
            return token.StartsWith('\"');
        }

        private static bool IsIntiger(string token)
        {
            return Int16.TryParse(token, out short result);
        }

        private static bool IsKeyword(string token)
        {
            return KeyWords.Contains(token);
        }

        private static bool IsSymbol(string token)
        {
            var tokenChar = token[0];
            return Symbols.Contains(tokenChar);
        }

    }
}
