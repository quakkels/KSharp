using KSharp.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KSharp.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void CanTokenizeAnIdentifier()
        {
            // arrange
            var reader = new FakeSourceReader("asdf");
            var tokenizer = new Tokenizer(reader);

            // act
            var result = tokenizer.GetToken();

            // assert
            Assert.AreEqual(TokenType.Identifier, result);
        }

        [TestMethod]
        public void CanGetKeywordToken()
        {
            // arrange
            var reader = new FakeSourceReader("def");
            var tknzr = new Tokenizer(reader);

            // act
            var result = tknzr.GetToken();

            // assert
            Assert.AreEqual(TokenType.Def, result);
        }

        [TestMethod]
        public void CanGetTwoTokens()
        {
            // arrange
            var reader = new FakeSourceReader("asdf def");
            var t = new Tokenizer(reader);

            // act
            var result1 =t.GetToken();
            var result2 = t.GetToken();

            // assert
            Assert.AreEqual(TokenType.Identifier, result1);
            Assert.AreEqual(TokenType.Def, result2);
        }
    }

    public class FakeSourceReader : ISourceReader
    {
        private char[] _source;
        private int _currentIndex;
        private int _sourceLength;

        public FakeSourceReader(string source)
        {
            _source = source.ToCharArray();
            _sourceLength = source.Length;
        }

        public int GetChar()
        {
            if (_currentIndex >= _sourceLength)
            {
                return -1;
            }

            var thisChar = _source[_currentIndex];
            _currentIndex++;

            return thisChar;
        }
    }
}