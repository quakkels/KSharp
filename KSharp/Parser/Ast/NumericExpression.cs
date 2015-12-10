namespace KSharp.Parser.Ast
{
    public class NumericExpression : Expression
    {
        public double Value { get; set; }

        public NumericExpression(double val)
        {
            Value = val;
        }
    }
}
