using System;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class TextBodyParser_Specs
    {
        private TextBodyParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new TextBodyParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTexElementParser<TextBody>>(SUT);
        }

        #region GetEmptyElement

        [Test]
        public void getEmptyElement_should_return_TextObject()
        {
            // When 
            var result = SUT.GetEmptyElement();

            // Then
            Assert.IsNotNull(result);
        }

        #endregion GetEmptyElement

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_include_emph()
        {
            CollectionAssert.Contains(TextBodyParser.CodeIndicators, @"\\emph{");
        }
        [Test]
        public void codeIndicators_should_include_inline_math()
        {
            CollectionAssert.Contains(TextBodyParser.CodeIndicators, @"\\\(");
        }
        [Test]
        public void codeIndicators_should_include_newLine_math()
        {
            CollectionAssert.Contains(TextBodyParser.CodeIndicators, @"\\\[");
        }

        #endregion CodeIndicators

        #region SetChildElement

        [Test]
        public void setChildeElement_should_throw_notSupported()
        {
            Assert.Throws<NotSupportedException>(() => SUT.SetChildElement(new TextBody(), new TextBody()));
        }

        #endregion SetChildElement

        #region CodeStartsWithText

        [Test]
        public void codeStartsWithText_should_return_true_when_it_is_text()
        {
            // Given
            var code =
                new StringBuilder(@"My item" +
                                  @"\item My other item" +
                                  @"\chapter{Chapter 2}" +
                                  @"\section{Section 2}" +
                                  @"This is some other text" +
                                  @"\end{document}");

            // When 
            var result = SUT.CodeStartsWithText(code);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void codeStartsWithText_should_return_false_when_it_isnt_text()
        {
            // Given
            var code =
                new StringBuilder(@"\item My item" +
                                  @"\item My other item" +
                                  @"\chapter{Chapter 2}" +
                                  @"\section{Section 2}" +
                                  @"This is some other text" +
                                  @"\end{document}");

            // When 
            var result = SUT.CodeStartsWithText(code);

            // Then
            Assert.IsFalse(result);
        }

        #endregion CodeStartsWithText

        #region ParseCode

        [Test]
        public void parseCode_should_return_textobject_with_text()
        {
            // Given 
            var code =
             new StringBuilder(@"My item" +
                     @"\item My other item" +
              @"\chapter{Chapter 2}" +
              @"\section{Section 2}" +
              @"This is some other text" +
              @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual("My item", result.ToString());
        }

        [Test]
        public void parseCode_should_stop_when_command_and_swallow_newLInes()
        {
            // Given 
            var code =
                new StringBuilder(
                    @"My item 
\item My second Item");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual("My item", result.ToString());
        }

        [Test]
        public void parseCode_should_return_command_from_real_file()
        {
            // Given 
            var code = new StringBuilder(@"Hur definieras en signal? - Det är en fysisk kvantitet som varierar med tid, utrymme eller annan oberoende variabel (variabler) 
	\item Hur kan ett system vara definierat? - Kan vara definierat som en fysisk apparat som utför en operation på en signal. Alternativt ett system som svarar på stimulans eller kraft genom att skapa en signal
\end{enumerate}
}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual("Hur definieras en signal? - Det är en fysisk kvantitet som varierar med tid, utrymme eller annan oberoende variabel (variabler)", result.ToString());
        }

        [Test]
        public void parseCode_should_remove_text_from_code()
        {
            // Given 
            var code =
             new StringBuilder(@"My item" +
                     @"\item My other item" +
              @"\chapter{Chapter 2}" +
              @"\section{Section 2}" +
              @"This is some other text" +
              @"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"\item My other item" +
                            @"\chapter{Chapter 2}" +
                            @"\section{Section 2}" +
                            @"This is some other text" +
                            @"\end{document}", code.ToString());
        }

        [Test]
        public void parseCode_should_handle_math_commands_by_including_them()
        {
            // Given 
            var code =
            new StringBuilder(@"My item $-\infty$ och $\infty$" +
                    @"\item My other item" +
             @"\chapter{Chapter 2}" +
             @"\section{Section 2}" +
             @"This is some other text" +
             @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"My item $-\infty$ och $\infty$", result.ToString());
        }

        [Test]
        public void parseCode_should_handle_dollar_signs_by_including_them()
        {
            // Given 
            var code =
            new StringBuilder(@"My item $-\infty$ och \$$\infty$" +
                    @"\item My other item" +
             @"\chapter{Chapter 2}" +
             @"\section{Section 2}" +
             @"This is some other text" +
             @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"My item $-\infty$ och \$$\infty$", result.ToString());
        }

        [Test]
        public void parseCode_should_handle_math_in_owrn_line_commands_by_including_them()
        {
            // Given 
            var code =
            new StringBuilder(@"My item \[-\infty\] och $\infty$" +
                    @"\item My other item" +
             @"\chapter{Chapter 2}" +
             @"\section{Section 2}" +
             @"This is some other text" +
             @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"My item \[-\infty\] och $\infty$", result.ToString());
        }
        [Test]
        public void parseCode_should_handle_math_line_commands_with_math_commands_by_including_them()
        {
            // Given 
            var code =
            new StringBuilder(@"My item \(H_1, \dots, H_n\)" +
                    @"\item My other item" +
             @"\chapter{Chapter 2}" +
             @"\section{Section 2}" +
             @"This is some other text" +
             @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"My item \(H_1, \dots, H_n\)", result.ToString());
        }

        [Test]
        public void parseCode_should_handle_inline_math_commands_by_including_them()
        {
            // Given 
            var code =
            new StringBuilder(@"My item \(-\infty\) och \(\infty\)" +
                    @"\item My other item" +
             @"\chapter{Chapter 2}" +
             @"\section{Section 2}" +
             @"This is some other text" +
             @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"My item \(-\infty\) och \(\infty\)", result.ToString());
        }

        [Test]
        public void parseCode_should_handle_emph_clause_in_text_by_including_them()
        {
            // Given 
            var code =
                new StringBuilder(@"My item \emph{important} that then continues" +
                                  @"\item My other item" +
                                  @"\chapter{Chapter 2}" +
                                  @"\section{Section 2}" +
                                  @"This is some other text" +
                                  @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"My item \emph{important} that then continues", result.ToString());
        }

        [Test]
        public void parseCode_should_handle_emph_clause_starting_a_text_by_including_them()
        {
            // Given 
            var code =
                new StringBuilder(@"\emph{important} that then continues" +
                                  @"\item My other item" +
                                  @"\chapter{Chapter 2}" +
                                  @"\section{Section 2}" +
                                  @"This is some other text" +
                                  @"\end{document}");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"\emph{important} that then continues", result.ToString());
        }

        [Test]
        public void parseCode_should_handle_math_with_inner_command_blocks()
        {
            // Given 
            var code = new StringBuilder(@"Definiera den likformigt fördelat för en kontinuerligt s.v. $X$ - Om den s.v. $X$ har täthetsfunktionen \[ f_X(x) =\begin{cases}\frac{1}{(b-a)} & \quad \text{om } a < x < b\\ 0  & \quad  \text{annars}\\ \end{cases} \] sägs $X$ vara likformigt fördelad");

            // When 
            var result = SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"Definiera den likformigt fördelat för en kontinuerligt s.v. $X$ - Om den s.v. $X$ har täthetsfunktionen \[ f_X(x) =\begin{cases}\frac{1}{(b-a)} & \quad \text{om } a < x < b\\ 0  & \quad  \text{annars}\\ \end{cases} \] sägs $X$ vara likformigt fördelad",
                result.ToString());
        }

        [Test]
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.ParseCode(new StringBuilder(@"\item My code")));
            Assert.AreEqual("The code didn't start with text but an code object", ex.Message);
        }

        #endregion ParseCode
    }
}