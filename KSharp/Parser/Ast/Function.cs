namespace KSharp.Parser.Ast
{
    public class Function
    {
        public Prototype Proto { get; set; }
        public Expression Body { get; set; }

        public Function(Prototype proto, Expression body)
        {
            Proto = proto;
            Body = body;
        }
    }
}
