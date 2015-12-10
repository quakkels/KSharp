using KSharp.Lexer;
using KSharp.Parser.Ast;
using System;

namespace KSharp.Parser
{
    public class Parsr
    {
        private ITokenizer _tokenizer;

        public Token CurrentToken { get; set; }

        public Parsr(ITokenizer tokenizr)
        {
            _tokenizer = tokenizr;
        }

        public Parsr() : this(new Tokenizer()) { }

        public Token GetNextToken()
        {
            CurrentToken = _tokenizer.GetToken();
            return CurrentToken;
        }

        public Expression ParseIdentifier()
        {
            var name = Convert.ToString(CurrentToken.Value);

            GetNextToken();

            if (CurrentToken.IsChar('('))
            {

            }
            return null;
        }

        public NumericExpression ParseNumber()
        {
            var result = new NumericExpression(Convert.ToDouble(CurrentToken.Value));
            GetNextToken();
            return result;
        }

        public Expression ParseParen()
        {
            GetNextToken(); // munch the first '('

            // todo: get contents of the paren

            if (CurrentToken.Type != TokenType.Character
                && Convert.ToChar(CurrentToken.Value) != ')')
            {
                throw new Exception("Expected \")\"");
            }

            return null;
        }

        public Expression ParsePrimary()
        {
            if (CurrentToken.Type == TokenType.Identifier)
            {
                return ParseIdentifier();
            }

            if (CurrentToken.Type == TokenType.Number)
            {
                return ParseNumber();
            }

            if (CurrentToken.Type == TokenType.Character
                && CurrentToken.Value.Equals(')'))
            {
                return ParseParen();
            }

            throw new Exception("Unkown token when expecting an expression.");
        }
    }
}
