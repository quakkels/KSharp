namespace KSharp.Parser.Ast
{
    public class VariableExpression : Expression
    {
        public string Name { get; set; }

        public VariableExpression(string name)
        {
            Name = name;
        }
    }
}
