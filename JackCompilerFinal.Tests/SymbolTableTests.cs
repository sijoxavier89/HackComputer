using System;
using Xunit;

namespace JackCompilerFinal.Tests
{
    public class SymbolTableTests
    {
        SymbolTable st;
        public SymbolTableTests()
        {
            st = new SymbolTable();
        }

        [Fact]
        public void SymbolTable_Initialize()
        {
            // Arrange

            // class
            st.Define("x", "int", Kind.FIELD);
            st.Define("y", "int", Kind.FIELD);
            st.Define("pointCount", "int", Kind.STATIC);

            // subroutine
            st.StartSubroutine();
            st.Define("this", "Point", Kind.ARG);
            st.Define("other", "Point", Kind.ARG);
            st.Define("dx", "int", Kind.VAR);
            st.Define("dy", "int", Kind.VAR);

            // Assert

            Assert.Equal("int", st.TypeOf("x"));
            Assert.Equal("Point", st.TypeOf("this"));
            Assert.Equal(1, st.IndexOf("y"));
            Assert.Equal(2, st.VarCount(Kind.FIELD));
            Assert.Equal("int", st.TypeOf("dx"));
            Assert.Equal(2, st.VarCount(Kind.VAR));

            // subroutine
            st.StartSubroutine();
            Assert.Equal(-1, st.IndexOf("dy"));
            Assert.Equal(0, st.VarCount(Kind.VAR));

            Assert.Equal(2, st.VarCount(Kind.FIELD));
        }
    }
}
