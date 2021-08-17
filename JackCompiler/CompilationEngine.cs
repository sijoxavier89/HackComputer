using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    /// <summary>
    /// Generates the compiler's output
    /// </summary>
    public class CompilationEngine
    {
        private const string KEYWORD = "KEYWORD";
        private const string SYMBOL = "SYMBOL";
        private const string INT = "INT_CONSTANT";
        private const string INTCONST = "integerConstant";
        private const string STRING = "STRING_CONSTANT";
        private const string STRINGCONST = "stringConstant";
        private const string IDENTIFIER = "IDENTIFIER";


        StreamReader inputFile;
        string outputFile;
        JackXmlWriter writer;
        private JackTokenizer tokenizer;

        public CompilationEngine(StreamReader inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            this.writer = new JackXmlWriter(outputFile);
            tokenizer = new JackTokenizer(inputFile);
        }


        public void CompileClass()
        {
            tokenizer.Advance();
            writer.AddStartElement("class"); // <class>
           
            if (string.Equals(tokenizer.TokenType(), KEYWORD)) // {
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            tokenizer.Advance();
            if (string.Equals(tokenizer.TokenType(), IDENTIFIER)) 
            {
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier().ToString());
            }

            tokenizer.Advance();
            if (string.Equals(tokenizer.TokenType(), SYMBOL))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Identifier().ToString());
            }

            tokenizer.Advance();
            // todo: compile ClassVarDec
            if (string.Equals(tokenizer.TokenType(), KEYWORD) && 
                (string.Equals(tokenizer.KeyWord(), "static",StringComparison.OrdinalIgnoreCase) || string.Equals(tokenizer.KeyWord(), "field", StringComparison.OrdinalIgnoreCase)))
            {
                CompileClassVarDec();
            }

            // SubroutineDec
            string token = tokenizer.KeyWord().ToLower();
            if (string.Equals(token, "constructor") || string.Equals(token, "function") || string.Equals(token, "method"))
            {
                CompileSubroutineDec();
            }

           
            if (string.Equals(tokenizer.TokenType(), SYMBOL)) // }
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());
            }
            writer.CloseElement();     // </class>

            // close the file
            writer.Close();
        }

        public void CompileClassVarDec()
        {
            string token = tokenizer.KeyWord().ToLower();
            while(string.Equals(token, "static") || string.Equals(token, "field"))
            {
                writer.AddStartElement("classVarDec");
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

                tokenizer.Advance();
                if (string.Equals(tokenizer.TokenType(), KEYWORD))  // type
                {
                    writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                }
                else if(string.Equals(tokenizer.TokenType(), IDENTIFIER))
                {
                    writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                }

                tokenizer.Advance();
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());

                // check the next token 
                tokenizer.Advance();
                while(string.Equals(tokenizer.TokenType(), SYMBOL) && tokenizer.Symbol() == ',')
                {
                    writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

                    tokenizer.Advance();                   
                    writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                      
                    tokenizer.Advance();
                }

                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString()); // ;
                // close the main element
                writer.CloseElement();

                tokenizer.Advance();
                token = string.Equals(tokenizer.TokenType(), KEYWORD) ? tokenizer.KeyWord().ToLower() : null;

            }
             
        }

        public void CompileSubroutineDec()
        {
            string token = tokenizer.KeyWord().ToLower();
            if (!(string.Equals(token, "constructor") || string.Equals(token, "function") || string.Equals(token, "method")))
            {
                return;
            }

                writer.AddStartElement("subroutineDec");
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                // type | void
               
                   tokenizer.Advance();
                  
                   if (tokenizer.TokenType().Equals(IDENTIFIER, StringComparison.OrdinalIgnoreCase))
                   {
                     writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                   }
                  else if(tokenizer.TokenType().Equals(KEYWORD, StringComparison.OrdinalIgnoreCase))
                  {
                    writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                  }
                
              // method  name
             tokenizer.Advance();
             string tokenType = tokenizer.TokenType().ToLower();

            if (tokenType.Equals(IDENTIFIER, StringComparison.OrdinalIgnoreCase))
            {
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
            }

          
            // (
            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            }

            // Params
            tokenizer.Advance();
            writer.AddStartElement("parameterList");
            if (tokenizer.TokenType().Equals(KEYWORD) || (tokenizer.TokenType().Equals(IDENTIFIER)))
            {
               
                CompileParameterList();

            }
            else
            {
                writer.WriteRaw(Environment.NewLine);
            }
            writer.CloseElement();

            // )
            //tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            }

            // SubroutineBody
            tokenizer.Advance();
            CompileSubroutineBody();

            writer.CloseElement();

            // recursive call for next subrotine
            tokenizer.Advance();
            CompileSubroutineDec();
        }

        
        public void CompileParameterList()
        {
           
            if (string.Equals(tokenizer.TokenType(), KEYWORD))  // type
            {
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
               
            }
            else if (string.Equals(tokenizer.TokenType(), IDENTIFIER))
            {
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
               
            }

            tokenizer.Advance();
            writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
           
            // check the next token 
            tokenizer.Advance();
            if (string.Equals(tokenizer.TokenType(), SYMBOL) && tokenizer.Symbol() == ',')
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                tokenizer.Advance();

                CompileParameterList();
            }

            return;
        }

        public void CompileSubroutineBody()
        {
           
            writer.AddStartElement("subroutineBody");
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            string token = tokenizer.KeyWord().ToLower();
            if(token.Equals("var"))
            {
               
                CompileVarDec();
               
            }
           
            writer.AddStartElement("statements");
            if (IsStatement())
            {
                CompileStatements();
            }
            else
            {
                writer.WriteRaw(Environment.NewLine);
            }
            writer.CloseElement();

            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            writer.CloseElement();
        }

        public void CompileVarDec()
        {
            string token = tokenizer.KeyWord().ToLower();
            if(!token.Equals("var"))
            {
                return;
            }
            writer.AddStartElement("varDec");
            // var
            writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // type
            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(IDENTIFIER, StringComparison.OrdinalIgnoreCase))
            {
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
            }
            else if (tokenizer.TokenType().Equals(KEYWORD, StringComparison.OrdinalIgnoreCase))
            {
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
            }

            // varName
            tokenizer.Advance();
            writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());

            // can be either "," or ":"
            tokenizer.Advance();
            while (string.Equals(tokenizer.TokenType(), SYMBOL) && tokenizer.Symbol().Equals(','))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

                tokenizer.Advance();
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());

                tokenizer.Advance();
            }

            // ";"
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());
           

            writer.CloseElement();

            tokenizer.Advance();
            CompileVarDec();

        }

        public void CompileStatements()
        {
           
            if (!IsStatement())
            {
                return;
            }
            string token = tokenizer.KeyWord().ToLower();
            if (token.Equals("let"))
            {
                CompileLet();
                tokenizer.Advance();
            }
            else if(token.Equals("while"))
            {
                CompileWhile();
               // tokenizer.Advance();
            }
            else if(token.Equals("if"))
            {
                CompileIf();
                //tokenizer.Advance();
            }
            else if(token.Equals("do"))
            {
                CompileDo();
                tokenizer.Advance();
            }
            else if (token.Equals("return"))
            {
                CompileReturn();
                tokenizer.Advance();
            }

           
            CompileStatements();
        }

        /// <summary>
        /// check whether the current token represent start of a statement
        /// </summary>
        /// <returns></returns>
        private bool IsStatement()
        {
            string token = tokenizer.KeyWord().ToLower();
            return (string.Equals(token, "let") || string.Equals(token, "while") || string.Equals(token, "if") || string.Equals(token, "do") || string.Equals(token, "return"));
        }

        public void CompileLet()
        {
            writer.AddStartElement("letStatement");
            writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());


            tokenizer.Advance();
            var token = tokenizer.Identifier();
            tokenizer.Advance();
            if (IsArrayEntry())
            {
                writer.AddElement(IDENTIFIER.ToLower(), token);
                ArrayEntryExpression();
                tokenizer.Advance();
            }
            else
            {
                writer.AddElement(IDENTIFIER.ToLower(), token);
                
            }

            // =
           
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

            // expression
            tokenizer.Advance();
           
            WriteExpression(); // new

            // ;
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());
         

            writer.CloseElement();
        }

        public void CompileIf()
        {
            writer.AddStartElement("ifStatement");
            writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // (
            tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();

            WriteExpression();

            
            // )
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // {
            tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            writer.AddStartElement("statements");

            if (IsStatement())
            {
                CompileStatements();
            }
            else
            {
                writer.WriteRaw(Environment.NewLine);
            }
            writer.CloseElement();

            // }
           // tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // else 
            tokenizer.Advance();
            if(tokenizer.TokenType().Equals(KEYWORD) && tokenizer.KeyWord().Equals("else", StringComparison.OrdinalIgnoreCase))
            {
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

                // {
                tokenizer.Advance();
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                writer.AddStartElement("statements");
                if (IsStatement())
                {
                    CompileStatements();
                }
                else
                {
                    writer.WriteRaw(Environment.NewLine);
                }
                writer.CloseElement();
                // }
                //tokenizer.Advance();
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
            }

            writer.CloseElement();
           
        }

        public void CompileWhile()
        {
            writer.AddStartElement("whileStatement");
            writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // (
            tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            WriteExpression();

            // )
           // tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // {
            tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            writer.AddStartElement("statements");
            CompileStatements();
            writer.CloseElement();

            // }
            // tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            writer.CloseElement();

            tokenizer.Advance();
        }

        public void CompileDo()
        {
            writer.AddStartElement("doStatement");
            // do
            writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
            
            SubroutineCall();

            // ;
            tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

            writer.CloseElement();
        }

        private void SubroutineCall()
        {
            tokenizer.Advance();
            writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());

            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals("."))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                tokenizer.Advance();
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                tokenizer.Advance();
            }

            // (
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            writer.AddStartElement("expressionList");
            if (!tokenizer.Symbol().Equals(')'))
            {              

                WriteExpression();
                while(tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals(","))
                {
                    writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                    tokenizer.Advance();
                    WriteExpression();
                }

            }
            else
            {
                writer.WriteRaw(Environment.NewLine);
            }
            writer.CloseElement();

            //)
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
           
        }

        private void WriteExpression()
        {
            writer.AddStartElement("expression");
          
            CompileTerm();
            // recursive call
           
            while (IsOperationSymbol())
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                CompileTerm();
            }

            writer.CloseElement();
            //  writer.CloseElement();

           // tokenizer.Advance();

            return;

        }

        private void CompileTerm()
        {
            writer.AddStartElement("term");
            // when current token is an identifier (varName)
            // it can be variable name , an array, subroutine call
            if (tokenizer.TokenType().Equals(IDENTIFIER))
            {
                var token = tokenizer.Identifier();
                tokenizer.Advance();
                if (IssubroutineCall())
                {
                    writer.AddElement(IDENTIFIER.ToLower(), token);
                    SubroutineCallExpression();

                    tokenizer.Advance();
                  //  writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());
                }
                else if (IsArrayEntry())
                {
                    writer.AddElement(IDENTIFIER.ToLower(), token);
                    ArrayEntryExpression();
                    tokenizer.Advance();
                }
                else
                {
                    writer.AddElement(IDENTIFIER.ToLower(), token);
                }
            }
            else if(IsUnaryOp())
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                CompileTerm();
            }
            else if(tokenizer.TokenType().Equals(INT))
            {
                writer.AddElement(INTCONST, Convert.ToString(tokenizer.IntVal()));
                tokenizer.Advance();
            }
            else if(tokenizer.TokenType().Equals(STRING))
            {
                writer.AddElement(STRINGCONST, tokenizer.StringVal());
                tokenizer.Advance();
            }
            else if (tokenizer.TokenType().Equals(KEYWORD)) // true|false|this|null
            {
                writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                tokenizer.Advance();
            }
            else if(tokenizer.TokenType().Equals(SYMBOL) && (tokenizer.Symbol().Equals('('))) // term expression eg; (a & (b|c))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                WriteExpression();

               // tokenizer.Advance();

                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
            }

            writer.CloseElement();

            
            // tokenizer.Advance();
            return;
        }

        private void SubroutineCallExpression()
        {
            if (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals("."))
            {
                writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                tokenizer.Advance();
                writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                tokenizer.Advance();
            }

            // (
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            writer.AddStartElement("expressionList");
            if (!tokenizer.Symbol().Equals(')')) // if next token is not closing bracket
            {

                // WriteExpression();
                WriteExpression();
                while (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals(","))
                {
                    writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                    tokenizer.Advance();
                    WriteExpression();
                }

            }
            else
            {
                writer.WriteRaw(Environment.NewLine);
            }
            writer.CloseElement();

            //)
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
        }

        private void ArrayEntryExpression()
        {
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            WriteExpression();

           // tokenizer.Advance();
            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
        }
        private bool IsArrayEntry()
        {
            if (tokenizer.TokenType().Equals(SYMBOL))
            {
                return tokenizer.Symbol().Equals('[');
            }

            return false;
        }

        
        private bool IssubroutineCall()
        {
            
           if(tokenizer.TokenType().Equals(SYMBOL))
            {
                return tokenizer.Symbol().Equals('.');
            }

           return false;
        }

        private bool IsOperationSymbol()
        {
            return tokenizer.TokenType().Equals(SYMBOL) &&  (tokenizer.Symbol().Equals('-') || tokenizer.Symbol().Equals('+') || tokenizer.Symbol().Equals('/') || tokenizer.Symbol().Equals('&') ||
                 tokenizer.Symbol().Equals('|') || tokenizer.Symbol().Equals('<') || tokenizer.Symbol().Equals('>') || tokenizer.Symbol().Equals('=') || tokenizer.Symbol().Equals('*'));
        }
       
       
        private bool IsUnaryOp()
        {
            return tokenizer.Symbol().Equals('-') || tokenizer.Symbol().Equals('~');
        }
        public void CompileReturn()
        {
            writer.AddStartElement("returnStatement");
            writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            tokenizer.Advance();
            if(!tokenizer.TokenType().Equals(SYMBOL))
            {
                WriteExpression();
            }

            writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            writer.CloseElement();
        }
    }
}
