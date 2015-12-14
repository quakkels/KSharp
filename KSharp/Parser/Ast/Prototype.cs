using System.Collections.Generic;

namespace KSharp.Parser.Ast
{
    public class Prototype
    {
        public string Name { get; set; }
        public List<string> Arguments { get; set; }

        public Prototype(string name, List<string> arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
