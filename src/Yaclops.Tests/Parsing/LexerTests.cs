using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class LexerTests
    {
        [Test]
        public void CanUnpushOnce()
        {
            var lexer = new Lexer("foo bar");

            lexer.Pop().Text.ShouldBe("foo");
            lexer.Unpush();
            lexer.Pop().Text.ShouldBe("foo");
            lexer.Pop().Text.ShouldBe("bar");
        }



        [Test]
        public void CanUnpushTwice()
        {
            var lexer = new Lexer("foo bar");

            lexer.Pop().Text.ShouldBe("foo");
            lexer.Unpush();
            lexer.Pop().Text.ShouldBe("foo");
            lexer.Unpush();
            lexer.Pop().Text.ShouldBe("foo");
            lexer.Pop().Text.ShouldBe("bar");
        }



        [Test]
        public void UnpushWithoutPopThrows()
        {
            var lexer = new Lexer("foo bar");

            Should.Throw<CommandLineParserException>(() => lexer.Unpush());
        }



        [Test, TestCaseSource(typeof(TestCaseFactory))]
        public void CanLexSingleToken(string content, string kindList, string textList)
        {
            var lexer = new Lexer(content);

            // The kinds parameter is specified as pipe-delimited string because TokenKind is internal and
            // because an array of strings doesn't look as nice when an assertion fails
            var kinds = kindList.Split(new[] {TestCaseFactory.Separator}, StringSplitOptions.None);
            var texts = textList.Split(new[] {TestCaseFactory.Separator}, StringSplitOptions.None);

            for (int i = 0; i < kinds.Length; i++)
            {
                TokenKind kind = (TokenKind)Enum.Parse(typeof(TokenKind), kinds[i]);

                var token = lexer.Pop();
                token.Kind.ShouldBe(kind);

                if (token.Kind != TokenKind.EndOfInput)
                {
                    string text = texts[i];
                    token.Text.ShouldBe(text);
                }
            }
        }



        public class TestCaseFactory : IEnumerable<TestCaseData>
        {
            public const string Separator = ", ";

            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return Content("-short").ShortName("short");
                yield return Content("--long").LongName("long");
                yield return Content("value").Value("value");
                yield return Content("-short value").ShortName("short").Value("value");
                yield return Content("--long value").LongName("long").Value("value");
                yield return Content("--long=value").LongName("long").Value("value");
                yield return Content("--file \"spacey value\" -short").LongName("file").Value("spacey value").ShortName("short");
            }


            private CaseBuilder Content(string content)
            {
                return new CaseBuilder(content);
            }


            private class CaseBuilder
            {
                private readonly string _content;
                private readonly List<TokenKind> _kinds = new List<TokenKind>();
                private readonly List<string> _texts = new List<string>();


                public CaseBuilder(string content)
                {
                    _content = content;
                }


                public CaseBuilder ShortName(string name)
                {
                    _kinds.Add(TokenKind.ShortName);
                    _texts.Add(name);
                    return this;
                }


                public CaseBuilder LongName(string name)
                {
                    _kinds.Add(TokenKind.LongName);
                    _texts.Add(name);
                    return this;
                }


                public CaseBuilder Value(string val)
                {
                    _kinds.Add(TokenKind.Value);
                    _texts.Add(val);
                    return this;
                }


                public static implicit operator TestCaseData(CaseBuilder builder)
                {
                    builder._kinds.Add(TokenKind.EndOfInput);

                    var kinds = string.Join(Separator, builder._kinds.Select(x => x.ToString()));
                    var texts = string.Join(Separator, builder._texts);

                    return new TestCaseData(builder._content, kinds, texts);
                }
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
