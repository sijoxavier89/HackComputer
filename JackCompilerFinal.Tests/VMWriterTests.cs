using System;
using Xunit;

namespace JackCompilerFinal.Tests
{
    public class VMWriterTests
    {
        VMWriter writer;

        [Fact]
        public void CreateAndWriteTest()
        {
            writer = new VMWriter(AppDomain.CurrentDomain.BaseDirectory+@"\test.vm");

            writer.WriteFunction("Main.fibonacci", 0);
            writer.WritePush(Segment.ARG, 0);
            writer.WritePush(Segment.CONST, 2);
            writer.WriteArithmetic(Command.LT);
            writer.WriteIf("IF_TRUE");
            writer.WritePush(Segment.ARG, 0);
            writer.WriteReturn();

            writer.WriteCall("Main.fibonacci", 1);

            writer.Close();
        }
    }
}
