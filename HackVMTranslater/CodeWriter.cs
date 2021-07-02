using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackVMTranslater
{
    public class CodeWriter
    {
        

        // predefined labels
        private const string SP= "SP";
        private const string LCL= "LCL";
        private const string ARG = "ARG";
        private const string THIS = "THIS";
        private const string THAT = "THAT";

        // segment names
        private const int temp = 5;
      
        private const string static_segment = "static";
        private const string constant_segment = "segment";
        private const string arg_segment = "argument";
        private const string local_segment = "local";
        private const string this_segment = "this";
        private const string that_segment = "that";
        private const string temp_segment = "temp";
        private const string pointer_segment = "pointer";

        String path;
        String asmFile;
        //private int addrIndex;
        private int lblIndex;
        private int retAddrIndex;
        private int staticIndex;

        private string currentFile = string.Empty;
        private string currentFunction = string.Empty;
        public CodeWriter(string file)
        {
            asmFile = Path.GetFileName(file).Split('.')[0];
            path = (file.EndsWith(".vm")) ? asmFile + ".asm": asmFile + "/" + file + ".asm";
            File.WriteAllText(path, string.Empty);

        }

        /// <summary>
        /// informs the codeWriter that 
        /// translation of a new VM file has started
        /// called by Main of VM translater
        /// </summary>
        /// <param name="filename"></param>
        public void SetFileName(string filename)
        {
            Console.WriteLine("Translation started for -" + filename);
            currentFile = filename.Split('.')[0];
            asmFile = currentFile;
            // increment for each static field in the file
            staticIndex = 0;
        }

        /// <summary>
        /// write Bootstrap code that initilaize the
        /// VM
        /// </summary>
        public void WriteInit()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Bootstrap code");
            // set RAM[0] 256
            sb.AppendLine("@256");
            sb.AppendLine("D=A");
            sb.AppendLine("@0");
            sb.AppendLine("M=D");
            WriteFileAsync(sb.ToString(), path).Wait();

            // call Sys.init
            WriteCallAssembly("Sys.init",0);        
        }

        /// <summary>
        /// Writes assembly for label command
        /// </summary>
        /// <param name="lable"></param>
        public void WriteLabel(string label)
        {
            StringBuilder sb = new StringBuilder();

            string lblName = (!string.IsNullOrEmpty(currentFunction)) ? currentFunction + "." + label : label;
            sb.Append('(');
            sb.Append(lblName);
            sb.Append(')');
            WriteFileAsync(sb.ToString(), path).Wait();
        }

        /// <summary>
        /// Writes assembly code for goto
        /// </summary>
        /// <param name="label"></param>
        public void WriteGoto(string label)
        {
            string lblName = (!string.IsNullOrEmpty(currentFunction)) ? currentFunction + "." + label : label;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@"+ lblName);
            sb.AppendLine("0;JMP");
            WriteFileAsync(sb.ToString(), path).Wait();
        }

        /// <summary>
        /// Writes assebly for if-goto
        /// </summary>
        /// <param name="label"></param>
        public void WriteIf(string label)
        {
            string lblName = (!string.IsNullOrEmpty(currentFunction)) ? currentFunction + "." + label : label;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");
            sb.AppendLine("@" + lblName);
            sb.AppendLine("D;JNE");

            WriteFileAsync(sb.ToString(), path).Wait();
        }

        /// <summary>
        /// Writes assembly for function command
        /// </summary>
        /// <param name="functionName"></param>
        public void WriteFunction(string functionName, int numArgs)
        {
            // function started
            
            currentFunction = functionName;

            StringBuilder sb = new StringBuilder();
            string numArgsStr = Convert.ToString(numArgs);
            sb.AppendLine("//"+ functionName +" "+ numArgsStr);

            sb.AppendLine('('+ functionName + ')');
            // initialize local variables
            sb.AppendLine(InitLocal(numArgs));
            // new SP is SP+numArgs
            sb.AppendLine("@"+ numArgsStr);
            sb.AppendLine("D=A");
            sb.AppendLine("@SP");
            sb.AppendLine("M=D+M");

            WriteFileAsync(sb.ToString(), path).Wait();
        }

        /// <summary>
        /// Write assembly code for call command
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="numArgs"></param>
        public void WriteCall(string functionName, int numArgs)
        {
            //**** save caller's stack ***
            WriteCallAssembly(functionName, numArgs);
        }


        private void WriteCallAssembly(string functionName, int numArgs)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("//call " + functionName + " " + numArgs);
            // push retAddrLabel
            string retAddrLabel =  functionName + "$ret." + Convert.ToString(retAddrIndex);
            retAddrIndex++;

            sb.AppendLine("// push retAddrLabel");
            sb.AppendLine("@" + retAddrLabel);
            sb.AppendLine("D=A");
            sb.AppendLine(PushToStackCommand());

            // push LCL
            sb.AppendLine("// push LCL");
            sb.AppendLine("@LCL");
            sb.AppendLine("D=M");
            sb.AppendLine(PushToStackCommand());

            // push ARG
            sb.AppendLine("// push ARG");
            sb.AppendLine("@ARG");
            sb.AppendLine("D=M");
            sb.AppendLine(PushToStackCommand());

            // push THIS
            sb.AppendLine("// push THIS");
            sb.AppendLine("@THIS");
            sb.AppendLine("D=M");
            sb.AppendLine(PushToStackCommand());

            // push THAT
            sb.AppendLine("// push THAT");
            sb.AppendLine("@THAT");
            sb.AppendLine("D=M");
            sb.AppendLine(PushToStackCommand());

            // ARG = SP-5-nArgs
            string nArgStr = Convert.ToString(numArgs);
            sb.AppendLine("// ARG = SP-5-nArgs");
            sb.AppendLine("@SP");
            sb.AppendLine("D=M");
            sb.AppendLine("@5");
            sb.AppendLine("D=D-A");
            sb.AppendLine("@" + nArgStr);
            sb.AppendLine("D=D-A");
            sb.AppendLine("@ARG");
            sb.AppendLine("M=D");

            // LCL = SP
            sb.AppendLine("// LCL = SP");
            sb.AppendLine("@SP");
            sb.AppendLine("D=M");
            sb.AppendLine("@LCL");
            sb.AppendLine("M=D");

            // initialize local variables
            // sb.AppendLine(InitLocal(numArgs));

            // goto functionName
         
            sb.AppendLine("@" + functionName);
            sb.AppendLine("0;JMP");
            // (retAddrLabel)
            sb.AppendLine('(' + retAddrLabel + ')');

            WriteFileAsync(sb.ToString(), path).Wait();
        }
        private string InitLocal(int numArgs)
        {
            if (numArgs <= 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("// initialize locals");
            for (int i = 0; i < numArgs; i++)
            {
                string indexStr = Convert.ToString(i);

                sb.AppendLine("@LCL");                
                sb.AppendLine("D=M");                              // D=M
                sb.AppendLine("@" + indexStr);                     // @index
                sb.AppendLine("D=D+A");                            // D=D+A

                string addr = "R13";
                sb.AppendLine("@" + addr);                           //@addr
                sb.AppendLine("M=D");                              // M=D

                sb.AppendLine("@0");
                sb.AppendLine("D=A");

                sb.AppendLine("@" + addr);                        // @addr
                sb.AppendLine("A=M");                             // A=M
                sb.AppendLine("M=D");                             // M=D
            }

            return sb.ToString();
        }

        /// <summary>
        /// Writes assembly code for return command
        /// </summary>
        public void WriteReturn()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// return");
            // endFrame = LCL
            // *R14 = endFrame         
            sb.AppendLine("@LCL");
            sb.AppendLine("D=M");
            sb.AppendLine("@R14");
            sb.AppendLine("M=D");

            // *R13 =  retAddr = *(endFrame-5)
            sb.AppendLine("// retAddr = *(endFrame-5)");

            sb.AppendLine("@5");
            sb.AppendLine("D=A");
            sb.AppendLine("@LCL");
            sb.AppendLine("D=M-D");
            sb.AppendLine("A=D");
            sb.AppendLine("D=M");
            sb.AppendLine("@R13");
            sb.AppendLine("M=D");

            // *ARG=pop()
            sb.AppendLine("// *ARG=pop()");

            sb.AppendLine(SetStackToD());
            sb.AppendLine("@ARG");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            // SP=ARG+1
            sb.AppendLine("// SP=ARG+1");

            sb.AppendLine("@ARG");
            sb.AppendLine("D=M");
            sb.AppendLine("@1");
            sb.AppendLine("D=D+A");
            sb.AppendLine("@SP");                        
            sb.AppendLine("M=D");

            //THAT = *(endFrame-1)
            sb.AppendLine("//THAT = *(endFrame-1)");

            sb.AppendLine("@R14");
            sb.AppendLine("D=M");
            sb.AppendLine("@1");
            sb.AppendLine("D=D-A");
            sb.AppendLine("A=D");
            sb.AppendLine("D=M");
            sb.AppendLine("@THAT");
            sb.AppendLine("M=D");

            //THIS = *(endFrame-2)
            sb.AppendLine("//THIS = *(endFrame-2)");

            sb.AppendLine("@R14");
            sb.AppendLine("D=M");
            sb.AppendLine("@2");
            sb.AppendLine("D=D-A");
            sb.AppendLine("A=D");
            sb.AppendLine("D=M");
            sb.AppendLine("@THIS");
            sb.AppendLine("M=D");


            //ARG = *(endFrame-3)
            sb.AppendLine("//ARG = *(endFrame-3)");

            sb.AppendLine("@R14");
            sb.AppendLine("D=M");
            sb.AppendLine("@3");
            sb.AppendLine("D=D-A");
            sb.AppendLine("A=D");
            sb.AppendLine("D=M");
            sb.AppendLine("@ARG");
            sb.AppendLine("M=D");

            //ARG = *(endFrame-4)
            sb.AppendLine("//LCL = *(endFrame-4)");

            sb.AppendLine("@R14");
            sb.AppendLine("D=M");
            sb.AppendLine("@4");
            sb.AppendLine("D=D-A");
            sb.AppendLine("A=D");
            sb.AppendLine("D=M");
            sb.AppendLine("@LCL");
            sb.AppendLine("M=D");

            // goto retAddr
            sb.AppendLine("// goto retAddr");

            sb.AppendLine("@R13");
            sb.AppendLine("A=M");
            sb.AppendLine("0;JMP");

            sb.AppendLine(SetStackToD());
            WriteFileAsync(sb.ToString(), path).Wait();

            // currentFunction = string.Empty;

        }
        /// <summary>
        /// Writes to the output file the
        /// assembly code that implements the 
        /// given arithmetic command
        /// </summary>

        public void WriteArithmetic(string command)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("// " + command);
            if (command.Equals("add"))
            {
                sb = sb.AppendLine(Add());
            }else if(command.Equals("sub"))
            {
                sb = sb.AppendLine(Sub());
            }
            else if(command.Equals("and"))
            {
                sb = sb.AppendLine(And());
            }
            else if (command.Equals("or"))
            {
                sb = sb.AppendLine(Or());
            }
            else if (command.Equals("not"))
            {
                sb = sb.AppendLine(Not());
            }
            else if (command.Equals("neg"))
            {
                sb = sb.AppendLine(Neg());
            }
            else if (command.Equals("eq"))
            {
                sb = sb.AppendLine(Eq());
            }
            else if (command.Equals("gt"))
            {
                sb = sb.AppendLine(Gt());
            }
            else if (command.Equals("lt"))
            {
                sb = sb.AppendLine(Lt());
            }

            WriteFileAsync(sb.ToString(), path).Wait();
            
        }


        private string Lt()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M-D");

            string lblTrue = "true" + lblIndex;
            

            sb.AppendLine("@"+ lblTrue);
            sb.AppendLine("D;JLT");
            sb.AppendLine("D=0");

            string push = "push" + lblIndex;
            sb.AppendLine("@"+push);
            sb.AppendLine("0;JMP");
            sb.AppendLine("("+lblTrue+")");
            sb.AppendLine("D=-1");

            sb.AppendLine("(" + push + ")");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            lblIndex++;
            return sb.ToString();

        }

        private string Gt()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M-D");

            string lblTrue = "true" + lblIndex;


            sb.AppendLine("@" + lblTrue);
            sb.AppendLine("D;JGT");
            sb.AppendLine("D=0");

            string push = "push" + lblIndex;
            sb.AppendLine("@" + push);
            sb.AppendLine("0;JMP");
            sb.AppendLine("(" + lblTrue + ")");
            sb.AppendLine("D=-1");

            sb.AppendLine("(" + push + ")");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");

            lblIndex++;

            return sb.ToString();

        }

        private string Eq()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M-D");

            string lblTrue = "true" + lblIndex;


            sb.AppendLine("@" + lblTrue);
            sb.AppendLine("D;JEQ");
            sb.AppendLine("D=0");

            string push = "push" + lblIndex;
            sb.AppendLine("@" + push);
            sb.AppendLine("0;JMP");
            sb.AppendLine("(" + lblTrue + ")");
            sb.AppendLine("D=-1");

            sb.AppendLine("(" + push + ")");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");

            lblIndex++;

            return sb.ToString();

        }

        private string Add()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=D+M");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            return sb.ToString();
        }

        private string Sub()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M-D");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            return sb.ToString();
        }


        private string And()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=D&M");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            return sb.ToString();
        }

        private string Or()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=M");
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("D=D|M");
            sb.AppendLine("@SP");
            sb.AppendLine("A=M");
            sb.AppendLine("M=D");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            return sb.ToString();
        }

        private string Not()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("M=!M");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            return sb.ToString();
        }

        private string Neg()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");
            sb.AppendLine("M=M-1");
            sb.AppendLine("A=M");
            sb.AppendLine("M=-M");

            sb.AppendLine("@SP");
            sb.AppendLine("M=M+1");
            return sb.ToString();
        }
        /// <summary>
        // Writes to the output file the
        /// assembly code that implements the 
        /// given  command
        /// </summary>
        /// <param name="command">given command add , sub etc.</param>
        /// <param name="segment">he segment local, argument, temp etc</param>
        /// <param name="index">index of the given segment</param>
        public void WritePushPop(string command, string segment, int index)
        {
            StringBuilder sb = new StringBuilder();

            if (command.Equals("C_PUSH"))
            {
                sb.AppendLine("// "+ command +" "+segment+" "+index);
                sb.AppendLine(SetSegmentToD(segment, index));
                sb.AppendLine(PushToStackCommand());

            }
            else  // POP command
            {
                sb.AppendLine("// " + command + " " + segment + " " + index);
                sb.AppendLine(PopToSegment(segment, index));
            }

            WriteFileAsync(sb.ToString(), path).Wait();
        }

        private string PushToStackCommand()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");                        // @SP
            sb.AppendLine("A=M");                        // A=M
            sb.AppendLine("M=D");                        // M=D
            sb.AppendLine("@SP");                        // @SP
            sb.AppendLine("M=M+1");                      // M=M+1

            return sb.ToString();
        }

        /// <summary>
        /// Set data from segment to D register
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string SetSegmentToD(string segment, int index)
        {
            StringBuilder sb = new StringBuilder();
            string segmentName = string.Empty;
            string indexStr = Convert.ToString(index);

            if (!segment.Equals("constant"))
            {
                if (segment.Equals(static_segment))
                {
                    segmentName = asmFile+"." + indexStr;
                    sb.AppendLine("@" + segmentName);          //@FileName.index
                    sb.AppendLine("D=M");                        // D=M
                }
                else if (segment.Equals(temp_segment))
                {
                    string addr = Convert.ToString(temp + index);
                    sb.AppendLine("@" + addr);                // @segment            
                    sb.AppendLine("D=M");                        // D=M
                }
                else if (segment.Equals(pointer_segment))
                {
                    if (index == 0)
                    {
                        sb.AppendLine("@" + THIS);                // @THIS
                        sb.AppendLine("D=M");                        // D=M
                    }
                    else if (index == 1)
                    {
                        sb.AppendLine("@" + THAT);                // @THAT
                        sb.AppendLine("D=M");                     // D=M
                    }
                }
                else
                {
                    if(segment.Equals(local_segment))
                    {
                        segmentName = LCL;
                    }
                    else if(segment.Equals(arg_segment))
                    {
                        segmentName = ARG;
                    }
                    else if(segment.Equals(this_segment))
                    {
                        segmentName = THIS;
                    }
                    else if (segment.Equals(that_segment))
                    {
                        segmentName = THAT;
                    }

                    sb.AppendLine("@" + segmentName);             // @segment
                    sb.AppendLine("D=M");                        // D=M
                    sb.AppendLine("@" + indexStr);              // @index
                    sb.AppendLine("A=D+A");                      // A=D+A              
                    sb.AppendLine("D=M");                        // D=M
                }
            }
            else
            {
                sb.AppendLine("@" + indexStr);                // @index
                sb.AppendLine("D=A");                        // D=A
            }
            return sb.ToString();
        }

        /// <summary>
        /// set most recent value frm stack to D register
        /// </summary>
        /// <returns></returns>
        private string SetStackToD()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@SP");                        // @SP
            sb.AppendLine("M=M-1");                        // M=M-1
            sb.AppendLine("A=M");                        // A=M
            sb.AppendLine("D=M");                      // D=M

            return sb.ToString();
        }

        /// <summary>
        /// POP Operation
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string PopToSegment(string segment, int index)
        {

            StringBuilder sb = new StringBuilder();
            string indexStr = Convert.ToString(index);
          
            if (segment.Equals(static_segment))
            {
                sb.AppendLine(SetStackToD());                              // D = *SP
               
                string segmentName = asmFile+"." + indexStr;
                sb.AppendLine("@" + segmentName);          //@Foo.index
                sb.AppendLine("M=D");                              // M=D
            }
            else if (segment.Equals(pointer_segment))
            {
                sb.AppendLine(SetStackToD());                              // D = *SP

                if (index == 0)
                {
                    sb.AppendLine("@" + THIS);                // @THIS
                    sb.AppendLine("M=D");                     // D=M
                }
                else if (index == 1)
                {
                    sb.AppendLine("@" + THAT);                // @THAT
                    sb.AppendLine("M=D");                     // D=M
                }
            }
            else if (segment.Equals(temp_segment))
            {
                sb.AppendLine(SetStackToD());                              // D = *SP

                string addr = Convert.ToString(temp + index);
                sb.AppendLine("@" + addr);                   // @segment            
                sb.AppendLine("M=D");                        // M=D
            }
            else
            {
                string segmentName = string.Empty;
                if (segment.Equals(local_segment))
                {
                    segmentName = LCL;
                }
                else if (segment.Equals(arg_segment))
                {
                    segmentName = ARG;
                }
                else if (segment.Equals(this_segment))
                {
                    segmentName = THIS;
                }
                else if (segment.Equals(that_segment))
                {
                    segmentName = THAT;
                }
                sb.AppendLine("@" + segmentName);                  // @segment
                sb.AppendLine("D=M");                              // D=M
                sb.AppendLine("@" + indexStr);                     // @index
                sb.AppendLine("D=D+A");                            // D=D+A

                string addr = "R13";
                sb.AppendLine("@"+addr);                           //@addr
                sb.AppendLine("M=D");                              // M=D

                sb.AppendLine(SetStackToD());                      // D = *SP

                sb.AppendLine("@" + addr);                        // @addr
                sb.AppendLine("A=M");                             // A=M
                sb.AppendLine("M=D");                             // M=D
              
            }
            return sb.ToString();
        }



        //write
        private async Task WriteFileAsync(string code, string path)
        {
            using (StreamWriter outfile = new StreamWriter(path, true))
            {
                await outfile.WriteLineAsync(code);
            }
        }

        /// <summary>
        /// Close the output file
        /// </summary>
        public void Close()
        {
            WriteFileAsync(Terminate(), path).Wait();
          //  outfile.Close();
        }

        private string Terminate()
        {
            StringBuilder sb =  new StringBuilder();
            sb.AppendLine("(END)");
            sb.AppendLine("@END");
            sb.AppendLine("0;JMP");
            return sb.ToString();
        }
    }
}
