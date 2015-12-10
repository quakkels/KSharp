using System;

namespace KSharp.Lexer
{
    public class Tokenizer : ITokenizer
    {
        private readonly ISourceReader _sourceReader;
        private string _identifier = string.Empty;

        public Tokenizer() 
            : this(new SourceReader())
        {
        }
    
        public Tokenizer(ISourceReader sourceReader)
        {
            _sourceReader = sourceReader;
        }

        public Token GetToken()
        {
            int lastChar = ' ';

            while (char.IsWhiteSpace((char)lastChar))
            {
                lastChar = _sourceReader.GetChar();
            }

            if (char.IsLetter((char)lastChar))
            {
                _identifier = Convert.ToString((char)lastChar);
                lastChar = _sourceReader.GetChar();
                while (char.IsLetterOrDigit((char)lastChar))
                {
                    _identifier += (char)lastChar;
                    lastChar = _sourceReader.GetChar();
                    if (lastChar == -1)
                    {
                        break;
                    }
                }

                if (_identifier == "def")
                {
                    return new Token(TokenType.Def);
                }

                if (_identifier == "extern")
                {
                    return new Token(TokenType.Extern);
                }

                return new Token(TokenType.Identifier, _identifier);
            }

            if(char.IsDigit((char)lastChar) || (char)lastChar == '.')
            {
                var number = string.Empty;

                do
                {
                    number += (char)lastChar;
                    lastChar = _sourceReader.GetChar();
                }
                while (char.IsDigit((char)lastChar) || (char)lastChar == '.');

                return new Token(TokenType.Number, number);
            }

            while (lastChar == '#')
            {
                do
                {
                    lastChar = _sourceReader.GetChar();
                } while (lastChar != (int)TokenType.Eof && lastChar != '\n' && lastChar != '\r');

                if (lastChar != (int)TokenType.Eof)
                {
                    return GetToken();
                }
            }

            if (lastChar == (int)TokenType.Eof)
            {
                return new Token(TokenType.Eof);
            }

            var thisChar = lastChar;
            lastChar = _sourceReader.GetChar();
            return new Token(TokenType.Character, thisChar);
        }
    }
}
