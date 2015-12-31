using KSharp.Lexer;
using KSharp.Parser.Ast;
using System;
using System.Collections.Generic;

namespace KSharp.Parser
{
    public class Parsr
    {
        private ITokenizer _tokenizer;
        private Dictionary<char, int> _binopPrecendence;

        public Token CurrentToken { get; set; }

        public Parsr(ITokenizer tokenizr, Dictionary<char, int> binaryOperators)
        {
            _tokenizer = tokenizr;
            _binopPrecendence = binaryOperators;
        }

        public Parsr(Dictionary<char, int> binaryOperators) : this(new Tokenizer(), binaryOperators) { }

        public Token GetNextToken()
        {
            CurrentToken = _tokenizer.GetToken();
            return CurrentToken;
        }

        public int GetCurrentTokenPrecedence()
        {
            if (CurrentToken.Type == TokenType.Character)
            {
                if (_binopPrecendence.ContainsKey(Convert.ToChar(CurrentToken.Value)))
                {
                    return _binopPrecendence[Convert.ToChar(CurrentToken.Value)];
                }
            }

            return -1;
        }

        public Expression ParseIdentifier()
        {
            var name = Convert.ToString(CurrentToken.Value);

            GetNextToken();

            if (!CurrentToken.IsChar('('))
            {
                return new IdentifierExpression(name);
            }

            GetNextToken(); // munch the open paren

            var args = new List<Expression>();
            if (!CurrentToken.IsChar(')'))
            {
                while (true)
                {
                    args.Add(ParseExpression());

                    if (CurrentToken.IsChar(')'))
                    {
                        break;
                    }
                    else if (!CurrentToken.IsChar(','))
                    {
                        throw new Exception("Expected \" or ) in argument list.");   
                    }

                    GetNextToken();
                }
            }

            GetNextToken(); // munch close paren

            return new CallExpression(name, args);
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

            var contents = ParseExpression();

            if (!CurrentToken.IsChar(')'))
            {
                throw new Exception("Expected \")\"");
            }

            return contents;
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

            if (CurrentToken.IsChar('('))
            {
                return ParseParen();
            }

            throw new Exception("Unkown token when expecting an expression.");
        }

        public Expression ParseExpression()
        {
            var left = ParsePrimary();

            return ParseBinaryRightSide(left, 0);
        }

        public Expression ParseBinaryRightSide(Expression left, int leftPrecedence)
        {
            while (true)
            {
                var precedence = GetCurrentTokenPrecedence();
                if (precedence < leftPrecedence)
                {
                    return left;
                }

                var binop = Convert.ToChar(CurrentToken.Value);
                GetNextToken();

                var right = ParsePrimary();
                var nextPrecedence = GetCurrentTokenPrecedence();

                if (precedence < nextPrecedence)
                {
                    right = ParseBinaryRightSide(right, precedence + 1);
                }

                left = new BinaryExpression(binop, left, right);
            }
        }

        public Prototype ParsePrototype()
        {
            if (CurrentToken.Type != TokenType.Identifier)
            {
                throw new Exception("Expected function name in prototype.");
            }

            var functionName = (string)CurrentToken.Value;
            GetNextToken();

            if (!CurrentToken.IsChar('('))
            {
                throw new Exception("Expected ( in prototype.");
            }

            var argumentNames = new List<string>();
            while (GetNextToken().Type == TokenType.Identifier)
            {
                argumentNames.Add((string)CurrentToken.Value);

            }

            if (!CurrentToken.IsChar(')'))
            {
                throw new Exception("Expected ) in prototype.");
            }

            // success. eat the ')'
            GetNextToken();

            return new Prototype(functionName, argumentNames);
        }

        public Function ParseDefinition()
        {
            GetNextToken(); // eat the def
            var proto = ParsePrototype();
            var body = ParseExpression();

            var function = new Function(proto, body);
            return function;
        }
    }
}
