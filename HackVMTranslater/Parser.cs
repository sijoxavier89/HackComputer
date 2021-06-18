using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackVMTranslater
{
    class Parser
    {
        StreamReader fileInput;
        string nextLine;
        string command;
        int counter;
        List<string> commands;
        string commandType;

        private const string C_ARITHMETIC = "C_ARITHMETIC";
        private const string C_PUSH = "C_PUSH";
        private const string C_POP = "C_POP";
        private const string C_LABEL = "C_LABEL";
        private const string C_GOTO = "C_GOTO";
        private const string C_FUNCTION = "C_FUNCTION";
        private const string C_IF = "C_IF";
        private const string C_RETURN = "C_RETURN";
        private const string C_CALL = "C_CALL";

        public Parser(StreamReader file)
        {
            fileInput = file;
            commands = new List<string>();

            while ((nextLine = fileInput.ReadLine()) != null)
            {
                if (IsCommand(nextLine.Trim()))
                {
                    // ignore inline space and commands
                    var cmd = nextLine.Trim();
                    cmd = cmd.Split("//")[0];
                    commands.Add(cmd.Trim());
                }
            }
        }


        public bool HasMoreCommands()
        {
            if (commands.Count > counter)
            {
                return commands[counter] != null;
            }
            return false;

        }

        /// <summary>
        /// Check the line is actual command or comments
        /// </summary>
        /// <param name="line"></param>
        /// <returns>
        /// true - if it is a command
        /// </returns>
        private bool IsCommand(string line)
        {
            if (line != "" && line.Substring(0, 2) != "//")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reads the next command from the input and makes it
        /// the current command
        /// </summary>
        public void Advance()
        {
            command = commands[counter];
            counter++;
        }


        /// <summary>
        /// Returns a constant representing the 
        /// type of the current command
        /// makes it the 
        /// </summary>
        /// <returns></returns>
        public string CommandType()
        {
            
            if (command.StartsWith("push"))
            {
                commandType = C_PUSH;
            }
            else if (command.StartsWith("pop"))
            {
                commandType = C_POP;
            }
            else if (command.StartsWith("label"))
            {
                commandType = C_LABEL;
            }
            else if (command.StartsWith("goto"))
            {
                commandType = C_GOTO;
            }
            else if (command.StartsWith("if-goto"))
            {
                commandType = C_IF;
            }
            else if (command.StartsWith("function"))
            {
                commandType = C_FUNCTION;
            }
            else if (command.StartsWith("call"))
            {
                commandType = C_CALL;
            }
            else if (command.StartsWith("return"))
            {
                commandType = C_RETURN;
            }
            else if(IsArithmeticCommand(command))
            {
                commandType = C_ARITHMETIC;
            }
            return commandType;
        }

        private static bool IsArithmeticCommand(string command)
        {
            return command.Equals("add") || command.Equals("sub") || command.Equals("gt") || command.Equals("lt") || command.Equals("and") 
                || command.Equals("or") || command.Equals("or") || command.Equals("not") || command.Equals("eq");
        }
        /// <summary>
        /// Returns the first argument of the current command
        /// incase of C_ARITHMETIC command itself returned
        /// </summary>
        /// <returns></returns>
        public string Arg1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the second argument of the command
        /// </summary>
        /// <returns>
        /// </returns>
        public string Arg2()
        {
            throw new NotImplementedException();
        }

    }
}
