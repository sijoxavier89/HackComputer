using System;
using System.Collections.Generic;

namespace JackCompilerFinal
{
    /// <summary>
    /// Create symbol table for Class ans subroutine for
    /// Compiler
    /// </summary>
    public class SymbolTable
    {

        private int classStaticCount;
        private int classFieldCount;

        private int subroutineArgCount;
        private int subroutineLocalCount;


        private Dictionary<string, STRow> classST;
        private Dictionary<string, STRow> subroutineST;
      
        public SymbolTable()
        {
            classStaticCount = -1;
            classFieldCount = -1;
           

            classST = new Dictionary<string, STRow>();
        }
        /// <summary>
        /// Starts a new subroutine scope
        /// </summary>
        public void StartSubroutine()
        {
            subroutineArgCount = -1;
            subroutineLocalCount = -1;

            subroutineST = new Dictionary<string, STRow>();
        }

        /// <summary>
        /// Defines a new identifier of the given name,
        /// type, and kind, and assigns it a running index.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="kind"></param>
        public void Define(string name, string type, Kind kind)
        {

            if (kind == Kind.STATIC) // class scope
            {
                classST.Add(name, new STRow() { Type = type, Kind = kind, Index = classStaticCount + 1 });
                classStaticCount ++;
            }
            else if(kind == Kind.FIELD)
            {
                classST.Add(name, new STRow() { Type = type, Kind = kind, Index = classFieldCount + 1 });
                classFieldCount ++;
            }
            else  if(kind == Kind.ARG)    // subroutine scope
            {
                subroutineST.Add(name, new STRow() { Type = type, Kind = kind, Index = subroutineArgCount + 1 });
                subroutineArgCount ++;
            }
            else if(kind == Kind.VAR)
            {
                subroutineST.Add(name, new STRow() { Type = type, Kind = kind, Index = subroutineLocalCount + 1 });
                subroutineLocalCount ++;
            }
            

        }

        /// <summary>
        /// Returns the number of variables 
        /// of the given kind already defined in the current
        /// scope
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public int VarCount(Kind kind)
        {
            int count = 0;
            if (kind == Kind.STATIC) // class scope
            {
                count = classStaticCount + 1;
            }
            else if (kind == Kind.FIELD)
            {
                count = classFieldCount + 1;
            }
            else if (kind == Kind.ARG)    // subroutine scope
            {
                count = subroutineArgCount + 1;
            }
            else if (kind == Kind.VAR)
            {
                count = subroutineLocalCount + 1;
            }

            return count;
        }

        /// <summary>
        /// Returns the kind of the named identifier in the current scope
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Kind KindOf(string name)
        {

            Kind kind = Kind.NONE;

            subroutineST.TryGetValue(name, out var entry);

            if(entry != null)
            {
                kind = entry.Kind;
            }
            else
            {
                classST.TryGetValue(name, out var classEntry);
                kind = (classEntry != null)?classEntry.Kind: Kind.NONE;
            }

            return kind;
        }

        /// <summary>
        /// Returns the type of the named identifier in the current
        /// scope
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string TypeOf(string name)
        {
            var type = string.Empty;

            subroutineST.TryGetValue(name, out var entry);

            if (entry != null)
            {
                type = entry.Type;
            }
            else
            {
                classST.TryGetValue(name, out var classEntry);
                type = classEntry.Type;
            }

            return type;
        }

        /// <summary>
        /// Returns the index assigned to the named identifier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int IndexOf(string name)
        {
            var index =-1;

            subroutineST.TryGetValue(name, out var entry);

            if (entry != null)
            {
                index = entry.Index;
            }
            else
            {
                classST.TryGetValue(name, out var classEntry);
                index = (classEntry != null)? classEntry.Index : -1;
            }

            return index;
        }
    }

    class STRow
    {
        public string Type { get; set; }
        public Kind Kind { get; set; }

        public int Index { get;set;}
    }
    public enum Kind
    {
        STATIC,
        FIELD,
        ARG,
        VAR,
        NONE,
        CONSTANT
    }
}
