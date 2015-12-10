using System.Collections.Generic;

namespace KSharp.Parser.Ast
{
    public class CallExpression : Expression
    {
        public string Callee { get; set; }
        public List<Expression> Arguments { get; set; }

        public CallExpression(string callee, List<Expression> arguments)
        {
            Callee = callee;
            Arguments = arguments;
        }
    }
}
