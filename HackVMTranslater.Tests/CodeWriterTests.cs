using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HackVMTranslater.Tests
{
    public class CodeWriterTests
    {
        CodeWriter codeWriter;
        public CodeWriterTests()
        {
            codeWriter = new CodeWriter("output");
        }

        [Fact]
        public void WritePushPop_push()
        {
            codeWriter.WritePushPop("C_PUSH", "constant", 10);
            codeWriter.WritePushPop("C_PUSH", "local", 0);
        }
    }
}
