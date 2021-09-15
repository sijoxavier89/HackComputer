using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    /// <summary>
    /// Parse the input files into fields
    /// ignore whitespaces and comments
    /// </summary>
    public class Parser
    {
        StreamReader fileInput;
        string nextLine;
        string command;
        int counter;
        List<string> commands;
        string commandType;
       
        public Parser(StreamReader file)
        {
            fileInput = file;
            commands = new List<string>();

            while((nextLine = fileInput.ReadLine()) != null)
            {
                if(IsCommand(nextLine.Trim()))
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

        private bool IsCommand(string line)
        {
           if(line != "" && line.Substring(0,2) != "//")
            {
                return true;
            }
           return false;
        }
        public void Advance()
        {
            command = commands[counter];
            counter++;
        }

        public string CommandType()
        {
            //string commandType;
          if(command.StartsWith('@'))
            {
                commandType = "A";
            }else if(command.StartsWith('(') && command.EndsWith(')'))
            {
                commandType = "L";
            }
            else
            {
                commandType = "C";
            }
            return commandType;
        }

        public string Symbol()
        {
            if (commandType.Equals("A"))
            {
                return command.Substring(1, command.Length - 1);
            }
            else if (commandType.Equals("L"))
            {
                return command.Substring(1, command.Length - 2);
            }
            return null;
        }

        public string Dest()
        {
            if (command.Contains("="))
            {
                if (commandType.Equals("C"))
                    return command.Split("=")[0];
            }

            return "null";
            
        }

        public string Comp()
        {
            if (command.Contains("="))
            {
                if (commandType.Equals("C"))
                    return command.Split("=")[1];
            }
            else
            {
                return command.Split(";")[0];
            }

            return null;
        }

        public string Jump()
        {
            if (commandType.Equals("C"))
            {
                var cmd = command.Split(";");
                if(cmd.Length ==1 )
                {
                    return "null";
                }
                return cmd[1];
            }

            return "null";
        }
    }
}
