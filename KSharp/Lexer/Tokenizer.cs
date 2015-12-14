using System;

namespace KSharp.Lexer
{
    public class Tokenizer : ITokenizer
    {
        private readonly ISourceReader _sourceReader;
        private string _identifier = string.Empty;
        private int _lastChar;

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
            while (char.IsWhiteSpace((char)_lastChar) || _lastChar == 0)
            {
                _lastChar = _sourceReader.GetChar();
            }

            if (char.IsLetter((char)_lastChar))
            {
                _identifier = Convert.ToString((char)_lastChar);
                _lastChar = _sourceReader.GetChar();
                while (char.IsLetterOrDigit((char)_lastChar))
                {
                    _identifier += (char)_lastChar;
                    _lastChar = _sourceReader.GetChar();
                    if (_lastChar == -1)
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

            if(char.IsDigit((char)_lastChar) || (char)_lastChar == '.')
            {
                var number = string.Empty;

                do
                {
                    number += (char)_lastChar;
                    _lastChar = _sourceReader.GetChar();
                }
                while (char.IsDigit((char)_lastChar) || (char)_lastChar == '.');

                return new Token(TokenType.Number, number);
            }

            while (_lastChar == '#')
            {
                do
                {
                    _lastChar = _sourceReader.GetChar();
                } while (_lastChar != (int)TokenType.Eof && _lastChar != '\n' && _lastChar != '\r');

                if (_lastChar != (int)TokenType.Eof)
                {
                    return GetToken();
                }
            }

            if (_lastChar == (int)TokenType.Eof)
            {
                return new Token(TokenType.Eof);
            }

            var thisChar = _lastChar;
            _lastChar = _sourceReader.GetChar();
            return new Token(TokenType.Character, thisChar);
        }
    }
}
