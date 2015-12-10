using KSharp.Lexer;
using KSharp.Parser;
using KSharp.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KSharp.Tests
{
    [TestClass]
    public class ParserTests
    {
        private ISourceReader _r;
        private ITokenizer _t;
        private Parsr _p;

        [TestInitialize]
        public void Initialize()
        {
            _r = new FakeSourceReader(string.Empty);
            _t = new Tokenizer(_r);
            _p = new Parsr(_t);
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

        private void SetSource(string source)
        {
            ((FakeSourceReader)_r).Source = source;
        }
    }
}
