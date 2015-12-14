namespace KSharp.Parser.Ast
{
    public class BinaryExpression : Expression
    {
        public char Operator { get; set; }
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public BinaryExpression(char @operator, Expression left, Expression right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }
    }
}
