using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JackCompiler.Tests
{
    public class CompilationEngineTests
    {
        [Fact]
        public void CompileClass_Success()
        {
            string filepath = @"C:\Users\sijox\source\repos\JackCompiler.Tests\Square1.jack";
            StreamReader streamReader = new StreamReader(filepath);
            CompilationEngine engine = new CompilationEngine(streamReader, "test");

            // Act
            engine.CompileClass();
        }

        [Fact]
        public void CompileClass_classVarDec_Success()
        {
            string filepath = @"C:\Users\sijox\source\repos\JackCompiler.Tests\VarTest.jack";
            StreamReader streamReader = new StreamReader(filepath);
            CompilationEngine engine = new CompilationEngine(streamReader, "test");

            // Act
            engine.CompileClass();
        }

        [Fact]
        public void CompileClass_ArrayMain()
        {
            string filepath = @"C:\Users\sijox\source\repos\JackCompiler.Tests\Main.jack";
            StreamReader streamReader = new StreamReader(filepath);
            CompilationEngine engine = new CompilationEngine(streamReader, "test");

            // Act
            engine.CompileClass();
        }
    }
}
