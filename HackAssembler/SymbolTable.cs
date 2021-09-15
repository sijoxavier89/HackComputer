using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    public class SymbolTable
    {
        Dictionary<string, int> symbolTable;
        public SymbolTable()
        {
            // initialize default symbols
           symbolTable = new Dictionary<string, int>();

        }

        public int Getaddress(string key)
        {
            return symbolTable[key];
        }

        public void AddEntry(string key, int value)
        {
            symbolTable.Add(key, value);
        }

        public bool Contains(string symbol)
        {
            if(symbol == "SP" || symbol == "R0")
            {
                return true;
            }
            int value ;
            symbolTable.TryGetValue(symbol, out value);
            return value != 0;
        }

    }
}
