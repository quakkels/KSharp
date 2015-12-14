using KSharp.Lexer;
using KSharp.Tests.Fakes;
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
            Assert.AreEqual(TokenType.Identifier, result.Type);
            Assert.AreEqual("asdf", (string)result.Value);
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
            Assert.AreEqual(TokenType.Def, result.Type);
            Assert.AreEqual(TokenType.Def, (TokenType)result.Value);
        }

        [TestMethod]
        public void CanGetNumberToken()
        {
            // arrange
            var r = new FakeSourceReader("asdf 1232 asdf");
            var t = new Tokenizer(r);

            // act
            t.GetToken();
            var result = t.GetToken();

            // assert
            Assert.AreEqual(TokenType.Number, result.Type);
            Assert.AreEqual("1232", (string)result.Value);
        }

        [TestMethod]
        public void CanGetTwoTokens()
        {
            // arrange
            var reader = new FakeSourceReader("asdf def");
            var t = new Tokenizer(reader);

            // act
            var result1 = t.GetToken();
            var result2 = t.GetToken();

            // assert
            Assert.AreEqual(TokenType.Identifier, result1.Type);
            Assert.AreEqual(TokenType.Def, result2.Type);
        }

        [TestMethod]
        public void CanGetEndOfFile()
        {
            // arrange
            var reader = new FakeSourceReader("asdf def");
            var t = new Tokenizer(reader);

            // act
            t.GetToken();
            t.GetToken();
            var result = t.GetToken();

            // assert
            Assert.AreEqual(TokenType.Eof, result.Type);
            Assert.AreEqual(TokenType.Eof, (TokenType)result.Value);
        }

        [TestMethod]
        public void CanIgnoreNewlinesAndComments()
        {
            // arrange
            var reader = new FakeSourceReader(
                "asdf     "
                + Environment.NewLine
                + Environment.NewLine
                + "#asdfsadf"
                + Environment.NewLine
                + "def"
                + Environment.NewLine);
            var t = new Tokenizer(reader);

            // act
            var result1 = t.GetToken();
            var result2 = t.GetToken();
            var result3 = t.GetToken();

            // assert
            Assert.AreEqual(TokenType.Identifier, result1.Type);
            Assert.AreEqual(TokenType.Def, result2.Type);
            Assert.AreEqual(TokenType.Eof, result3.Type);
        }

        [TestMethod]
        public void CanHandleNonAlphaNumericToken()
        {
            // arrange
            var r = new FakeSourceReader("=");
            var t = new Tokenizer(r);

            // act
            var result = t.GetToken();

            // assert
            Assert.AreEqual(TokenType.Character, result.Type);
            Assert.AreEqual('=', Convert.ToChar(result.Value));
        }

        [TestMethod]
        public void CanTokenizeWithWhitespacePresent()
        {
            var r = new FakeSourceReader("12 asdf ( ) - =");
            var t = new Tokenizer(r);

            var number = t.GetToken();
            var id = t.GetToken();
            var openParen = t.GetToken();
            var closeParen = t.GetToken();
            var minus = t.GetToken();
            var equals = t.GetToken();
            var eof = t.GetToken();

            Assert.AreEqual(12, Convert.ToInt32(number.Value));
            Assert.AreEqual("asdf", id.Value);
            Assert.AreEqual('(', Convert.ToChar(openParen.Value));
            Assert.AreEqual(')', Convert.ToChar(closeParen.Value));
            Assert.AreEqual('-', Convert.ToChar(minus.Value));
            Assert.AreEqual('=', Convert.ToChar(equals.Value));
            Assert.AreEqual(TokenType.Eof, eof.Type);
        }

        [TestMethod]
        public void CanTokenizeWithoutWhitespacePresent()
        {
            var r = new FakeSourceReader("12asdf()-=");
            var t = new Tokenizer(r);

            var number = t.GetToken();
            var id = t.GetToken();
            var openParen = t.GetToken();
            var closeParen = t.GetToken();
            var minus = t.GetToken();
            var equals = t.GetToken();
            var eof = t.GetToken();

            Assert.AreEqual(12, Convert.ToInt32(number.Value));
            Assert.AreEqual("asdf", id.Value);
            Assert.AreEqual('(', Convert.ToChar(openParen.Value));
            Assert.AreEqual(')', Convert.ToChar(closeParen.Value));
            Assert.AreEqual('-', Convert.ToChar(minus.Value));
            Assert.AreEqual('=', Convert.ToChar(equals.Value));
            Assert.AreEqual(TokenType.Eof, eof.Type);
        }
    }
}
