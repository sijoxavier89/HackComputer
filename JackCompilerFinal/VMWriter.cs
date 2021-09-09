using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JackCompilerFinal
{
    public class VMWriter
    {
        private static Dictionary<Segment, string> segmentName = new Dictionary<Segment, string>()
        {
            {Segment.ARG, "argument" },
            {Segment.LOCAL, "local" },
            {Segment.POINTER, "pointer" },
            {Segment.THIS, "this" },
            {Segment.THAT, "that" },
            {Segment.TEMP, "temp" },
            {Segment.STATIC, "static" },
            {Segment.CONST, "constant" }
        };

        private static Dictionary<Command, string> operation = new Dictionary<Command, string>()
        {
            {Command.ADD, "add" },
            {Command.SUB, "sub" },
            {Command.NEG, "neg" },
            {Command.EQ, "eq" },
            {Command.GT, "gt" },
            {Command.LT, "lt" },
            {Command.AND, "and" },
            {Command.OR, "or" },
            {Command.NOT, "not" },
        };

      
        StreamWriter writer;
        public VMWriter(string outputFile)
        {
            writer = new StreamWriter(outputFile);


        }

        /// <summary>
        /// Writes a VM push command
        /// </summary>
        /// <param name="segment"></param>
        public void WritePush(Segment segment, int index)
        {
           CreateCommand("push", segmentName[segment], index);
           
        }

        
        /// <summary>
        /// Write Vm pop command
        /// </summary>
        /// <param name="segment"></param>
        public void WritePop(Segment segment, int index)
        {
             CreateCommand("pop", segmentName[segment], index);
        
        }

        private void CreateCommand(string cmd, string segment, int index)
        {
            StringBuilder sb = new StringBuilder(cmd);
            sb.Append(' ');
            sb.Append(segment);
            sb.Append(' ');
            sb.Append(index);
            sb.AppendLine();
            WriteFile(sb.ToString());
            
        }
        /// <summary>
        /// Write VM arithmetic-logic command
        /// </summary>
        /// <param name="segment"></param>
        public void WriteArithmetic(Command cmd)
        {
            StringBuilder sb = new StringBuilder(operation[cmd]);
            sb.AppendLine();
            WriteFile(sb.ToString());
        }

        /// <summary>
        /// Writes a VM label command
        /// 
        /// </summary>
        /// <param name="label"></param>
        public void WriteLabel(string label)
        {
            StringBuilder sb = new StringBuilder("label");
            sb.Append(' ');
            sb.Append(label);
            sb.AppendLine();
            WriteFile(sb.ToString());
        }

        /// <summary>
        /// Write VM goto command
        /// </summary>
        /// <param name="label"></param>
        public void WriteGoto(string label)
        {
            WriteBranchCommand("goto", label);
        }

        /// <summary>
        /// Write VM if-goto
        /// </summary>
        /// <param name="label"></param>
        public void WriteIf(string label)
        {
            WriteBranchCommand("if-goto", label);

        }

        private void WriteBranchCommand(string cmd, string label)
        {
            StringBuilder sb = new StringBuilder(cmd);
            sb.Append(' ');
            sb.Append(label);
            sb.AppendLine();
            WriteFile(sb.ToString());
        }
        /// <summary>
        /// Writes VM call command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nArgs"></param>
        public void WriteCall(string name, int nArgs)
        {
            CreateCommand("call", name, nArgs);
 
        }

        /// <summary>
        /// Writes a VM function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nLocals"></param>
        public void WriteFunction(string name, int nLocals)
        {
          CreateCommand("function", name, nLocals);

        }

        /// <summary>
        /// Writes VM Return
        /// </summary>
        public void WriteReturn()
        {
            StringBuilder sb = new StringBuilder("return");
            sb.AppendLine();
            WriteFile(sb.ToString());
        }


        //write
        private  void WriteFile(string code)
        {
           
                writer.WriteLine(code);           
        }

        /// <summary>
        /// Close the output file
        /// </summary>
        public void Close()
        {
            writer.Dispose();
        }
    }

    public enum Segment
    {
        ARG,
        LOCAL,
        STATIC,
        THIS,
        THAT,
        POINTER,
        TEMP,
        CONST
    }

    public enum Command
    {
        ADD,
        SUB,
        NEG,
        EQ,
        GT,
        LT,
        AND,
        OR,
        NOT
    }
}
