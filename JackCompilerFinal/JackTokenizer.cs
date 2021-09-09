using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace JackCompilerFinal
{
    public class JackTokenizer
    {
        StreamReader fileInput;
        List<string> commands;
        string nextLine;
        Queue<string> queue;
        private string currentToken;
        public JackTokenizer(StreamReader file)
        {
            fileInput = file;
            commands = new List<string>();
            queue = new Queue<string>();
            while ((nextLine = fileInput.ReadLine()) != null)
            {
                if (IsCommand(nextLine.Trim()))
                {
                    // ignore inline space and comments
                    var cmd = nextLine.Trim();
                    cmd = cmd.Split("//")[0];
                    commands.Add(cmd.Trim());
                }
            }

            // process commands
            EvaluateCommands();
        }

        /// <summary>
        /// Filter blankspaces and comments
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsCommand(string line)
        {
            if (line != "" && !line.StartsWith("//") && !line.StartsWith("/*") && !line.StartsWith("*"))
            {
                return true;
            }
            return false; ;
        }

        /// <summary>
        /// Evaluate each commands, add each valid segments into 
        /// queue as tokens
        /// </summary>
        private void EvaluateCommands()
        {
            foreach (var command in commands)
            {
                // split by space then
                // again split by symbols
                // add entries and symbols into the queue
                string[] splitByBlank;
                if (!command.Contains("\""))
                {
                    splitByBlank = command.Split(' ');
                }
                else
                {

                    splitByBlank = SplitStringConstant(command);

                }


                foreach (var word in splitByBlank)
                {

                    string[] parts = Regex.Split(word, @"([{}()[\].,;+\-*/&|<>=~])");
                    foreach (var part in parts)
                    {
                        if (!string.IsNullOrEmpty(part))
                            queue.Enqueue(part);
                    }


                }

            }
        }

        private static string[] SplitStringConstant(string command)
        {
            var regex = new Regex("\"(.*?)\"");
            var test = regex.Split(command);
            var match = regex.Match(command).Value;
            var matchValue = match.Trim(new char[] { '\"' });
            var list = new List<string>();

            foreach (string s in test)
            {
                if (!s.Equals(matchValue))
                {
                    var split = s.Split(' ');
                    foreach (var sp in split)
                    {
                        list.Add(sp);
                    }
                }
                else
                {
                    list.Add(match);
                }
            }

            return list.ToArray();
        }

        public bool HasMoreTokens()
        {
            return queue.Count > 0;

        }

        public void Advance()
        {
            currentToken = queue.Dequeue();
        }

        public string TokenType()
        {

            return TokenHelper.TokenType(currentToken);
        }

        public string KeyWord()
        {
            return currentToken.ToUpper();
        }

        /// <summary>
        /// Returns the character which is the 
        /// current token
        /// </summary>
        /// <returns></returns>
        public char Symbol()
        {
            return currentToken[0];
        }

        /// <summary>
        /// returns the identifier which is the 
        /// current token
        /// </summary>
        /// <returns></returns>
        public string Identifier()
        {
            return currentToken;
        }

        /// <summary>
        /// returns the intiger value of the 
        /// current token. should be called only 
        /// if tokenType is INT_CONST
        /// </summary>
        /// <returns></returns>
        public int IntVal()
        {
            return Convert.ToInt16(currentToken);
        }

        /// <summary>
        /// Returns the string value of the current token, without 
        /// enclosing double quotes
        /// </summary>
        /// <returns></returns>
        public string StringVal()
        {
            return currentToken.Trim(new char[] { '\"' });
        }
    }
}
