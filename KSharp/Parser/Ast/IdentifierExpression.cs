namespace KSharp.Parser.Ast
{
    public class IdentifierExpression : Expression
    {
        public string Name { get; set; }

        public IdentifierExpression(string name)
        {
            Name = name;
        }
    }
}
