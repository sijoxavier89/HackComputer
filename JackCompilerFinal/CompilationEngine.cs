using System;
using System.Collections.Generic;
using System.IO;

namespace JackCompilerFinal
{
    /// <summary>
    /// Gets the input from a JackTokenozer and writes its output using VMWriter
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

        private string classname;
        private string currentSubroutine;
        private int labelIfCount;
        private int labelWhileCount;


        private static Dictionary<string, Kind> kindLookup = new Dictionary<string, Kind>()
        {
            {"static",Kind.STATIC },
            {"field",Kind.FIELD },

        };

        private static Dictionary<Kind, Segment> kindToSegment = new Dictionary<Kind, Segment>()
        {
            {Kind.FIELD, Segment.THIS },
            {Kind.STATIC, Segment.STATIC },
            {Kind.ARG, Segment.ARG },
            {Kind.VAR, Segment.LOCAL },
            {Kind.CONSTANT, Segment.CONST }
        };


        private static Dictionary<char, Command> operation = new Dictionary<char, Command>()
        {
            { '+', Command.ADD },
            { '-', Command.SUB },
            { '=', Command.EQ},
            { '>', Command.GT },
            { '<', Command.LT },
            { '&', Command.AND },
            { '|', Command.OR },
            { '!', Command.NOT },
            { '~', Command.NEG },
        };


        StreamReader inputFile;
        string outputFile;
        //JackXmlWriter writer;
        VMWriter vmWriter;
        private JackTokenizer tokenizer;
        private SymbolTable symbolTable;
        public CompilationEngine(StreamReader inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            // this.writer = new JackXmlWriter(outputFile);
            tokenizer = new JackTokenizer(inputFile);
            symbolTable = new SymbolTable();
            vmWriter = new VMWriter(outputFile);
        }


        public void CompileClass()
        {
            tokenizer.Advance();
            // writer.AddStartElement("class"); // <class>

            if (string.Equals(tokenizer.TokenType(), KEYWORD)) // {
                                                               //writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

                tokenizer.Advance();
            if (string.Equals(tokenizer.TokenType(), IDENTIFIER))
            {
                // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier().ToString());
                classname = tokenizer.Identifier().ToString();
            }

            tokenizer.Advance();
            if (string.Equals(tokenizer.TokenType(), SYMBOL))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Identifier().ToString());
            }

            tokenizer.Advance();
            // todo: compile ClassVarDec
            if (string.Equals(tokenizer.TokenType(), KEYWORD) &&
                (string.Equals(tokenizer.KeyWord(), "static", StringComparison.OrdinalIgnoreCase) || string.Equals(tokenizer.KeyWord(), "field", StringComparison.OrdinalIgnoreCase)))
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
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());
            }
            //  writer.CloseElement();     // </class>

            // close the file
            vmWriter.Close();
        }

        public void CompileClassVarDec()
        {
            string token = tokenizer.KeyWord().ToLower();
            string kind;
            string type = string.Empty;
            string name;
            while (string.Equals(token, "static") || string.Equals(token, "field"))
            {
                // writer.AddStartElement("classVarDec");
                //  writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                kind = token;

                tokenizer.Advance();
                if (string.Equals(tokenizer.TokenType(), KEYWORD))  // type
                {
                    // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                    type = tokenizer.KeyWord().ToLower();
                }
                else if (string.Equals(tokenizer.TokenType(), IDENTIFIER))
                {
                    // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                    type = tokenizer.Identifier();
                }

                tokenizer.Advance();
                //writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                name = tokenizer.Identifier();

                // Add to symbol table
                AddToSymbolTable(name, type, kindLookup[kind]);

                // check the next token 
                tokenizer.Advance();
                while (string.Equals(tokenizer.TokenType(), SYMBOL) && tokenizer.Symbol() == ',')
                {
                    //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

                    tokenizer.Advance();
                    //writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                    name = tokenizer.Identifier();
                    // Add to symbol table
                    AddToSymbolTable(name, type, kindLookup[kind]);
                    tokenizer.Advance();
                }

                //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString()); // ;
                // close the main element
                // writer.CloseElement();

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

            // writer.AddStartElement("subroutineDec");
            // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // type | void

            tokenizer.Advance();

            if (tokenizer.TokenType().Equals(IDENTIFIER, StringComparison.OrdinalIgnoreCase))
            {
                // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
            }
            else if (tokenizer.TokenType().Equals(KEYWORD, StringComparison.OrdinalIgnoreCase))
            {
                //writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
            }

            // method  name
            tokenizer.Advance();
            string tokenType = tokenizer.TokenType().ToLower();

            if (tokenType.Equals(IDENTIFIER, StringComparison.OrdinalIgnoreCase))
            {
                //writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                // set method name
                currentSubroutine = tokenizer.Identifier();
            }


            // (
            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            }

            // define new subroutine scope
            symbolTable.StartSubroutine();
            AddToSymbolTable("this", classname, Kind.ARG);

            // Params
            tokenizer.Advance();
            //  writer.AddStartElement("parameterList");
            if (tokenizer.TokenType().Equals(KEYWORD) || (tokenizer.TokenType().Equals(IDENTIFIER)))
            {

                CompileParameterList();

            }
            else
            {
                //writer.WriteRaw(Environment.NewLine);
            }
            //  writer.CloseElement();

            // )
            //tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            }

            // SubroutineBody
            tokenizer.Advance();
            CompileSubroutineBody();

            // writer.CloseElement();

            // recursive call for next subrotine
            tokenizer.Advance();
            CompileSubroutineDec();
        }


        /// <summary>
        /// Compile paramters in the function definition
        /// </summary>
        public void CompileParameterList()
        {
            string type = string.Empty;
            string name = string.Empty;
            if (string.Equals(tokenizer.TokenType(), KEYWORD))  // type
            {
                // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                type = tokenizer.KeyWord().ToLower();

            }
            else if (string.Equals(tokenizer.TokenType(), IDENTIFIER))
            {
                //  writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                type = tokenizer.Identifier();
            }

            tokenizer.Advance();
            // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
            name = tokenizer.Identifier();
            // Add to symbol table
            AddToSymbolTable(name, type, Kind.ARG);

            // check the next token 
            tokenizer.Advance();
            if (string.Equals(tokenizer.TokenType(), SYMBOL) && tokenizer.Symbol() == ',')
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                tokenizer.Advance();

                CompileParameterList();
            }

            return;
        }

        public void CompileSubroutineBody()
        {

            //  writer.AddStartElement("subroutineBody");
            //   writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            string token = tokenizer.KeyWord().ToLower();
            if (token.Equals("var"))
            {

                CompileVarDec();

            }

            // write function : function name nArgs
            // function name -> className.functioname
            var nArgs = symbolTable.VarCount(Kind.VAR);
            var functionName = classname + "." + currentSubroutine;
            vmWriter.WriteFunction(functionName, nArgs);

            //  writer.AddStartElement("statements");
            if (IsStatement())
            {
                CompileStatements();
            }
            else
            {
                //  writer.WriteRaw(Environment.NewLine);
            }
            // writer.CloseElement();

            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            // writer.CloseElement();
        }

        public void CompileVarDec()
        {
            string token = tokenizer.KeyWord().ToLower();
            if (!token.Equals("var"))
            {
                return;
            }

            string type = string.Empty;
            string name = string.Empty;
            //writer.AddStartElement("varDec");
            // var
            //writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // type
            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(IDENTIFIER, StringComparison.OrdinalIgnoreCase))
            {
                // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                type = tokenizer.Identifier();
            }
            else if (tokenizer.TokenType().Equals(KEYWORD, StringComparison.OrdinalIgnoreCase))
            {
                // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                type = tokenizer.KeyWord().ToLower();
            }

            // varName
            tokenizer.Advance();
            // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
            name = tokenizer.Identifier();

            // Add to symbol table
            AddToSymbolTable(name, type, Kind.VAR);
            // can be either "," or ":"
            tokenizer.Advance();
            while (string.Equals(tokenizer.TokenType(), SYMBOL) && tokenizer.Symbol().Equals(','))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

                tokenizer.Advance();
                //writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());

                name = tokenizer.Identifier();
                // Add to symbol table
                AddToSymbolTable(name, type, Kind.VAR);

                tokenizer.Advance();
            }

            // ";"
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());


            // writer.CloseElement();

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
            else if (token.Equals("while"))
            {
                CompileWhile();
                // tokenizer.Advance();
            }
            else if (token.Equals("if"))
            {
                CompileIf();
                //tokenizer.Advance();
            }
            else if (token.Equals("do"))
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
            // writer.AddStartElement("letStatement");
            // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            var lhs = string.Empty;
            tokenizer.Advance();
            var token = tokenizer.Identifier();
            tokenizer.Advance();
            if (IsArrayEntry())
            {
                // writer.AddElement(IDENTIFIER.ToLower(), token);
                ArrayEntryExpression();
                tokenizer.Advance();
            }
            else
            {
                // writer.AddElement(IDENTIFIER.ToLower(), token);
                lhs = token;
            }

            // =

            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

            // expression
            tokenizer.Advance();

            WriteExpression(); // new

            // ;
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

            // pop the value to the lhs
            var kind = symbolTable.KindOf(lhs);
            var index = symbolTable.IndexOf(lhs);

            vmWriter.WritePop(kindToSegment[kind], index);

            // writer.CloseElement();
        }

        public void CompileIf()
        {
            // writer.AddStartElement("ifStatement");
            // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // (
            tokenizer.Advance();
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();

            WriteExpression();


            // )
            //  writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // {
            tokenizer.Advance();
            //  writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            //writer.AddStartElement("statements");

            // write label for if statements
            var lblTrue = "IF_TRUE" + Convert.ToString(labelIfCount);
            var lblFalse = "IF_FALSE" + Convert.ToString(labelIfCount);
            var lblEnd = "IF_END" + Convert.ToString(labelIfCount);

            vmWriter.WriteIf(lblTrue);
            vmWriter.WriteGoto(lblFalse);
            vmWriter.WriteLabel(lblTrue);
            labelIfCount++;

            if (IsStatement())
            {
                CompileStatements();
            }
            else
            {
                // writer.WriteRaw(Environment.NewLine);
            }
            //writer.CloseElement();

            // }
            // tokenizer.Advance();
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // else 
            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(KEYWORD) && tokenizer.KeyWord().Equals("else", StringComparison.OrdinalIgnoreCase))
            {
                // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());
                // write else block label
                vmWriter.WriteGoto(lblEnd);    // goto IF_END0
                vmWriter.WriteLabel(lblFalse); // label IF_FALSE0
                // {
                tokenizer.Advance();
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                //  writer.AddStartElement("statements");
                if (IsStatement())
                {
                    CompileStatements();
                }
                else
                {
                    // writer.WriteRaw(Environment.NewLine);
                }
                // writer.CloseElement();
                // }
                //tokenizer.Advance();
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                // end Else block ; label IF_END0
                vmWriter.WriteLabel(lblEnd);

                tokenizer.Advance();
            }
            else // write label for if only statement
            {
                vmWriter.WriteLabel(lblFalse); // label IF_FALSE0
            }

            // writer.CloseElement();

        }

        public void CompileWhile()
        {
            // writer.AddStartElement("whileStatement");
            // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            // (
            tokenizer.Advance();
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();

            // label the while expression
            var lblWhileExpression = "WHILE_EXP" + Convert.ToString(labelWhileCount);
            var lblWhileEnd = "WHILE_END" + Convert.ToString(labelWhileCount);
            labelWhileCount++;

            vmWriter.WriteLabel(lblWhileExpression);
            WriteExpression();

            // )
            // tokenizer.Advance();
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // {
            tokenizer.Advance();
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // write while block label 
            vmWriter.WriteArithmetic(Command.NOT);
            vmWriter.WriteIf(lblWhileEnd);

            tokenizer.Advance();
            // writer.AddStartElement("statements");
            CompileStatements();
            // writer.CloseElement();

            // }
            // tokenizer.Advance();
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // Write block end commands
            vmWriter.WriteGoto(lblWhileExpression); // goto WHILE_EXP0
            vmWriter.WriteLabel(lblWhileEnd);       // label WHILE_END0
                                                    // writer.CloseElement();

            tokenizer.Advance();
        }

        public void CompileDo()
        {
            // writer.AddStartElement("doStatement");
            // do
            // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

           // tokenizer.Advance();                                             // moved from method
                                                                             // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier()); // moved from method
            SubroutineCall();

            // ;
            tokenizer.Advance();
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());

            // writer.CloseElement();
        }
        /// <summary>
        /// for do Subroutine();
        /// </summary>
        private void SubroutineCall()
        {

            tokenizer.Advance();
            var subName = tokenizer.Identifier();
            int nArgs = 0; // number of arguments pushed to stack before calling the method
            tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals("."))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                tokenizer.Advance();
                // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());

                // set method name for call
                // check the meethod is instance method or function 
                var type = symbolTable.TypeOf(subName);
                var method = tokenizer.Identifier();

                if (!String.IsNullOrEmpty(type))
                {
                    subName = type + "." + method;
                    nArgs++;
                }
                else
                {
                    subName = subName + "." + method;
                }

                tokenizer.Advance();
            }
            else
            {
                subName = classname + subName;
            }

            // (
            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            // writer.AddStartElement("expressionList");
            if (!tokenizer.Symbol().Equals(')'))
            {

                WriteExpression();
                nArgs++; // increment arg count for each expression in the expression list
                while (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals(","))
                {
                    // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                    tokenizer.Advance();
                    WriteExpression();

                    nArgs++;
                }

            }
            else
            {
                // writer.WriteRaw(Environment.NewLine);
            }
            //writer.CloseElement();

            //)
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            // call method

            vmWriter.WriteCall(subName, nArgs);

        }

        private void WriteExpression()
        {
            // writer.AddStartElement("expression");

            CompileTerm();
            // recursive call

            while (IsOperationSymbol())
            {
                //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                var op = tokenizer.Symbol();
                tokenizer.Advance();
                CompileTerm();

                // add operation vm command
                vmWriter.WriteArithmetic(operation[op]);
            }

            //  writer.CloseElement();
            //  writer.CloseElement();

            // tokenizer.Advance();

            return;

        }

        private void CompileTerm()
        {
            // writer.AddStartElement("term");
            // when current token is an identifier (varName)
            // it can be variable name , an array, subroutine call
            if (tokenizer.TokenType().Equals(IDENTIFIER))
            {
                var token = tokenizer.Identifier();
                tokenizer.Advance();
                if (IssubroutineCall())
                {
                    // writer.AddElement(IDENTIFIER.ToLower(), token);
                    SubroutineCallExpression();
                   // SubroutineCall();

                    tokenizer.Advance();
                    //  writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString());
                }
                else if (IsArrayEntry())
                {
                    // writer.AddElement(IDENTIFIER.ToLower(), token);
                    ArrayEntryExpression();
                    tokenizer.Advance();
                }
                else
                {
                    // writer.AddElement(IDENTIFIER.ToLower(), token);
                    // push the value to the lhs
                    var kind = symbolTable.KindOf(token);
                    var index = symbolTable.IndexOf(token);
                    vmWriter.WritePush(kindToSegment[kind], index);
                }
            }
            else if (IsUnaryOp())
            {
                //  writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                CompileTerm();

                // write arithmetic and logic command
                vmWriter.WriteArithmetic(operation[tokenizer.Symbol()]);
            }
            else if (tokenizer.TokenType().Equals(INT))
            {
                // writer.AddElement(INTCONST, Convert.ToString(tokenizer.IntVal()));

                // write integer constant
                vmWriter.WritePush(kindToSegment[Kind.CONSTANT], tokenizer.IntVal());
                tokenizer.Advance();
            }
            else if (tokenizer.TokenType().Equals(STRING)) //x="cc...c" are handled using a series of calls to String.appendChar(c);
            {
                // writer.AddElement(STRINGCONST, tokenizer.StringVal());

                // Write string assignment
                WriteString(tokenizer.StringVal());
                tokenizer.Advance();
            }
            else if (tokenizer.TokenType().Equals(KEYWORD)) // true|false|this|null
            {
                // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

                // write vm
                WriteKeywordConstant(tokenizer.KeyWord().ToLower());

                tokenizer.Advance();
            }
            else if (tokenizer.TokenType().Equals(SYMBOL) && (tokenizer.Symbol().Equals('('))) // term expression eg; (a & (b|c))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
                WriteExpression();

                // tokenizer.Advance();

                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                tokenizer.Advance();
            }

            //writer.CloseElement();


            // tokenizer.Advance();
            return;
        }


        private void WriteString(string stringConstant)
        {
            // push the size of the string
            vmWriter.WritePush(Segment.CONST, stringConstant.Length);
            vmWriter.WriteCall("String.new", 1);
            foreach (char c in stringConstant)
            {
                // push the asci code  of the char
                vmWriter.WritePush(Segment.CONST, c);
                vmWriter.WriteCall("String.appendChar", 2); // 2 -> 1st argument for string object, 2nd for char c
            }

        }

        /// <summary>
        /// Writes command for true, false, this, null
        /// </summary>
        /// <param name="keyword"></param>
        private void WriteKeywordConstant(string keyword)
        {
            if (keyword.Equals("this"))
            {
                vmWriter.WritePush(Segment.POINTER, 0);
            }
            else if (keyword.Equals("false") || keyword.Equals("null"))
            {
                vmWriter.WritePush(Segment.CONST, 0);
            }
            else if (keyword.Equals("true"))
            {
                vmWriter.WritePush(Segment.CONST, 1);
                vmWriter.WriteArithmetic(Command.NEG);
            }
        }
        private void SubroutineCallExpression()
        {
            var subName = tokenizer.Identifier();
            int nArgs = 0; // number of arguments pushed to stack before calling the method
            // tokenizer.Advance();
            if (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals("."))
            {
                // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
                tokenizer.Advance();
                // writer.AddElement(IDENTIFIER.ToLower(), tokenizer.Identifier());
                tokenizer.Advance();
            }

            // (
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            // writer.AddStartElement("expressionList");
            if (!tokenizer.Symbol().Equals(')')) // if next token is not closing bracket
            {

                // WriteExpression();
                WriteExpression();
                while (tokenizer.TokenType().Equals(SYMBOL) && tokenizer.Symbol().ToString().Equals(","))
                {
                    //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

                    tokenizer.Advance();
                    WriteExpression();
                }

            }
            else
            {
                //writer.WriteRaw(Environment.NewLine);
            }
            //writer.CloseElement();

            //)
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
        }

        private void ArrayEntryExpression()
        {
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());

            tokenizer.Advance();
            WriteExpression();

            // tokenizer.Advance();
            //writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
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

            if (tokenizer.TokenType().Equals(SYMBOL))
            {
                return tokenizer.Symbol().Equals('.');
            }

            return false;
        }

        private bool IsOperationSymbol()
        {
            return tokenizer.TokenType().Equals(SYMBOL) && (tokenizer.Symbol().Equals('-') || tokenizer.Symbol().Equals('+') || tokenizer.Symbol().Equals('/') || tokenizer.Symbol().Equals('&') ||
                 tokenizer.Symbol().Equals('|') || tokenizer.Symbol().Equals('<') || tokenizer.Symbol().Equals('>') || tokenizer.Symbol().Equals('=') || tokenizer.Symbol().Equals('*'));
        }


        private bool IsUnaryOp()
        {
            return tokenizer.Symbol().Equals('-') || tokenizer.Symbol().Equals('~');
        }
        public void CompileReturn()
        {
            // writer.AddStartElement("returnStatement");
            // writer.AddElement(KEYWORD.ToLower(), tokenizer.KeyWord().ToLower());

            tokenizer.Advance();
            if (!tokenizer.TokenType().Equals(SYMBOL))
            {
                WriteExpression();
            }

            // writer.AddElement(SYMBOL.ToLower(), tokenizer.Symbol().ToString().ToLower());
            // writer.CloseElement();
        }

        private void AddToSymbolTable(string name, string type, Kind kind)
        {
            symbolTable.Define(name, type, kind);

        }
    }
}
