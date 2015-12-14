using System;

namespace KSharp.Lexer
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public object Value { get; private set; }

        public Token(TokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public Token(TokenType type)
        {
            Type = type;
            Value = type;
        }

        public bool IsChar(char character)
        {
            if (TokenType.Character == Type
                && Convert.ToChar(Value) == character)
            {
                return true;
            }

            return false;
        }
    }
}
