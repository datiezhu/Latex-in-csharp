using System;
using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using Moosetrail.LaTeX.Helpers;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class FormatterParser_Specs
    {
        protected FormatterParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new FormatterParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser_for_Element()
        {
            Assert.IsInstanceOf<LaTexElementParser<Formatter>>(SUT);
        }
        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTeXElementParser>(SUT);
        }

        #region CodeIndicators

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_document(FormatterCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Formatter>)SUT).CodeIndicators, @"\\" + command);
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled(FormatterCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Formatter>)SUT).CodeIndicators, @"\\\\" + command);
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled_without_begin_escaped(FormatterCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Formatter>)SUT).CodeIndicators, "\\\\" + command);
        }

        #endregion CodeIndicators

        #region GetEmptyElement

        [Test]
        public void getEmptyElement_should_throw_not_supported()
        {
            // Then
            Assert.Throws<NotSupportedException>(()=> SUT.GetEmptyElement());
        }

        #endregion GetEmptyElement

        #region SetChiildElement

        [Test]
        public void setChildElement_should_set_elemnts()
        {
            // Given 
            var text1 = new TextBody();
            var section = new Section();
            var chapter = new Formatter(FormatterCommand.chapter);

            // When 
            SUT.SetChildElement(chapter, section, text1);

            // Then
            Assert.AreSame(section, chapter.InnerElements[0]);
            Assert.AreSame(text1, chapter.InnerElements[1]);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var chapter = new Formatter(FormatterCommand.chapter);

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(chapter));
            Assert.AreEqual("No child elements supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_formatter()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody(), new TextBody()));
            Assert.AreEqual("The supplied element wasn't a Formatter, only Formatter is allowed", ex.Message);
        }

        #endregion SetChiildElement

        #region ParseCode

        [Test]
        public void parseCode_should_set_inner_content_of_simple_formatter()
        {
            // Given 
            var code = @"\chapter{Chapter 1}" +
                       @"\section{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}";

            // When 
            var chapter = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.IsInstanceOf<TextBody>(chapter.InnerElements[0]);
            Assert.AreEqual("Chapter 1", ((TextBody)chapter.InnerElements[0]).TheText);
        }

        [Test]
        public void parseCode_should_return_remaining_code_for_simpel_formatter()
        {
            // Given 
            var code = new StringBuilder(@"\chapter{Chapter 1}" +
                       @"\section{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"\section{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}", code.ToString());

        }

        [Test]
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.ParseCode(new StringBuilder("My code")));
            Assert.AreEqual("The code didn't start with an allowed indicator", ex.Message);
        }

        [Test]
        public void praseCode_should_find_simple_formatter_from_escaped_handled_string()
        {
            // Given 
            var code =
                "\\chapter{Introduction}\n\\section{Signals, Systems and Signal Processing}\n\\begin{enumerate}\n\t\\item What is the definition of a signal? - Any physical quantity that varies with time, space or any other independent variable or variables\n\t\\item What does the variable $t$ represent? - Time\n\t\\item What variable represents time? - $t$\n\t\\item What is signal generation usually associated with? - A system that responds to a stimulus or force\n\t\\item What is an alternate definition of system that does not have to do with stimulus or force? - A system can be a physical device that preforms an operation on a signal \n\t\\item What are analog signals? - Functions of a continuous variable such as time\n\\end{enumerate}\n";

            // When 
            var result = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.AreEqual("Introduction", ((TextBody)result.InnerElements[0]).TheText);
        }

        #endregion ParseCode

        #region TestHelpers

        private static IEnumerable<FormatterCommand> allCommands()
        {
            return EnumUtil.GetValues<FormatterCommand>();
        }

        #endregion TestHelpers
    }
}