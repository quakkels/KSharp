using KSharp.Lexer;
using KSharp.Parser;
using KSharp.Parser.Ast;
using KSharp.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KSharp.Tests
{
    [TestClass]
    public class ParserTests
    {
        private ISourceReader _r;
        private ITokenizer _t;
        private Dictionary<char, int> _bo;
        private Parsr _p;

        [TestInitialize]
        public void Initialize()
        {
            _r = new FakeSourceReader(string.Empty);
            _t = new Tokenizer(_r);
            _bo = new Dictionary<char, int> {
                { '<', 10 },
                { '+', 20 },
                { '-', 30 },
                { '*', 40 }
            };
            _p = new Parsr(_t, _bo);
        }

        [TestMethod]
        public void CanGetNextToken()
        {
            SetSource("123 asdf");

            var result = _p.GetNextToken();
            Assert.AreEqual(TokenType.Number, _p.CurrentToken.Type);
            Assert.AreEqual(TokenType.Number, result.Type);

            var result2 = _p.GetNextToken();
            Assert.AreEqual(TokenType.Identifier, _p.CurrentToken.Type);
            Assert.AreEqual(TokenType.Identifier, result2.Type);
        }

        [TestMethod]
        public void CanParseNumberExpression()
        {
            SetSource("123.3");

            _p.GetNextToken();
            var result = _p.ParseNumber();

            Assert.IsNotNull(result);
            Assert.AreEqual(123.3, result.Value);
            Assert.AreEqual(TokenType.Eof, _p.CurrentToken.Type);
        }

        [TestMethod]
        public void NonOperatorReturnsLowPrecedence()
        {
            SetSource("$");

            _p.GetNextToken();
            var result = _p.GetCurrentTokenPrecedence();

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void ParseIdentitierExpressionReturnsVariable()
        {
            SetSource("asdf");
            _p.GetNextToken();

            var result = _p.ParseIdentifier();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IdentifierExpression));
        }

        [TestMethod]
        public void ParseIdentitierExpressionReturnsCallExpression()
        {
            SetSource("asdf()");
            _p.GetNextToken();

            var result = _p.ParseIdentifier() as CallExpression;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CallExpression));
            Assert.AreEqual("asdf", result.Callee);
            Assert.AreEqual(0, result.Arguments.Count);
        }

        [TestMethod]
        public void ParseIdentifierExpressionWithArguments()
        {
            SetSource("asdf(one, two)");
            _p.GetNextToken();

            var result = _p.ParseIdentifier() as CallExpression;

            Assert.IsNotNull(result);
            Assert.AreEqual("asdf", result.Callee);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual("one", ((IdentifierExpression)(result.Arguments[0])).Name);
            Assert.AreEqual("two", ((IdentifierExpression)(result.Arguments[1])).Name);

        }

        [TestMethod]
        public void ParseNumber()
        {
            SetSource("213.1");
            _p.GetNextToken();

            NumericExpression result = _p.ParseNumber();

            Assert.AreEqual(213.1, result.Value);
        }

        [TestMethod]
        public void ParseParen()
        {
            SetSource("(123)");
            _p.GetNextToken();

            var result = _p.ParseParen();

            Assert.IsNotNull(result);
            Assert.AreEqual(123, ((NumericExpression)result).Value);
        }

        [TestMethod]
        public void ParsePrimary_Identifier()
        {
            SetSource("abc123");
            _p.GetNextToken();

            var result = _p.ParsePrimary();

            Assert.IsNotNull(result);
            Assert.AreEqual("abc123", ((IdentifierExpression)result).Name);
        }

        [TestMethod]
        public void ParsePrimary_Number()
        {
            SetSource("123");
            _p.GetNextToken();

            var result = _p.ParsePrimary();

            Assert.IsNotNull(result);
            Assert.AreEqual(123, ((NumericExpression)result).Value);
        }

        [TestMethod]
        public void ParsePrimary_Paren()
        {
            SetSource("(a)");
            _p.GetNextToken();

            var result = _p.ParsePrimary();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", ((IdentifierExpression)result).Name);
        }

        [TestMethod]
        public void ParsePrototype()
        {
            // arrange
            SetSource("FName(arg1 arg2)");
            _p.GetNextToken();

            // act
            Prototype result = _p.ParsePrototype();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("FName", result.Name);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual("arg1", result.Arguments[0]);
            Assert.AreEqual("arg2", result.Arguments[1]);
        }

        [TestMethod]
        public void ParseExpression()
        {
            SetSource("123 + 321");
            _p.GetNextToken();

            var result = _p.ParseExpression();

            Assert.IsNotNull(result);
            var left = ((BinaryExpression)result).Left as NumericExpression;
            var op = ((BinaryExpression)result).Operator;
            var right = ((BinaryExpression)result).Right as NumericExpression;
            Assert.AreEqual(123, left.Value);
            Assert.AreEqual('+', op);
            Assert.AreEqual(321, right.Value);
        }

        [TestMethod]
        public void ParseDefinition_WithAnExpression()
        {
            SetSource("def FName (arg1 arg2) arg1 + arg2");
            _p.GetNextToken();

            var result = _p.ParseDefinition();

            Assert.IsNotNull(result);
            Assert.AreEqual("FName", result.Proto.Name);
            Assert.AreEqual(2, result.Proto.Arguments.Count);
            Assert.AreEqual("arg2", result.Proto.Arguments[1]);

            var binExp = result.Body as BinaryExpression;
            Assert.IsNotNull(binExp);
            Assert.AreEqual("arg1", ((IdentifierExpression)binExp.Left).Name);
            Assert.AreEqual('+', binExp.Operator);
            Assert.AreEqual("arg2", ((IdentifierExpression)binExp.Right).Name);
        }

        [TestMethod]
        public void ParseDefinition_WithCallExpression()
        {
            SetSource("def FName (arg1 arg2) FName2()");
            _p.GetNextToken();

            var result = _p.ParseDefinition();

            Assert.IsNotNull(result);
            Assert.AreEqual("FName", result.Proto.Name);
            Assert.AreEqual(2, result.Proto.Arguments.Count);
            Assert.AreEqual("arg2", result.Proto.Arguments[1]);

            var bodyExp = result.Body as CallExpression;
            Assert.IsNotNull(bodyExp);
            Assert.AreEqual("FName2", bodyExp.Callee);
            Assert.AreEqual(0, bodyExp.Arguments.Count);
        }

        [TestMethod]
        public void ParseExtern()
        {
            SetSource("extern FName()");
            _p.GetNextToken();

            var result = _p.ParseExtern();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Arguments.Count);
            Assert.AreEqual("FName", result.Name);
        }

        [TestMethod]
        public void ParseTopLevelExpression()
        {
            SetSource("1 - 1");
            _p.GetNextToken();

            var result = _p.ParseTopLevelExpression();

            Assert.IsNotNull(result);
            var left = ((BinaryExpression)(result.Body)).Left;
            var right = ((BinaryExpression)(result.Body)).Right;
            Assert.AreEqual(1, ((NumericExpression)left).Value);
            Assert.AreEqual('-', ((BinaryExpression)result.Body).Operator);
            Assert.AreEqual(1, ((NumericExpression)right).Value);
        }
        private void SetSource(string source)
        {
            ((FakeSourceReader)_r).Source = source;
        }
    }
}
