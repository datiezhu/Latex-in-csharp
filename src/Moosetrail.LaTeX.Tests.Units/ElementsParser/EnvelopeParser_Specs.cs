using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using Moosetrail.LaTeX.Helpers;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class EnvelopeParser_Specs
    {
        protected EnvelopeParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new EnvelopeParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser_for_Element()
        {
            Assert.IsInstanceOf<LaTexElementParser<Envelope>>(SUT);
        }
        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTeXElementParser>(SUT);
        }

        #region CodeIndicators

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_document(EnvelopeCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Envelope>)SUT).CodeIndicators, @"\\begin{" + command + "}");
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled(EnvelopeCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Envelope>)SUT).CodeIndicators, @"\\\\begin{" + command + "}");
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled_without_begin_escaped(EnvelopeCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Envelope>)SUT).CodeIndicators, "\\\\begin{" + command + "}");
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

        #region SetChildElement

        [Test]
        public void setChildELement_should_set_items()
        {
            // Given 
            var enumerate = new Enumerate();
            var item = new Item();

            // When 
            SUT.SetChildElement(enumerate, item);

            // Then
            Assert.AreSame(item, enumerate.ItemList.ElementAt(0));
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var enumerate = new Enumerate();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(enumerate));
            Assert.AreEqual("No child elements supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_formatter()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody(), new TextBody()));
            Assert.AreEqual("The supplied element wasn't an Envelope, only Envelope is allowed", ex.Message);
        }

        #endregion SetChildElement

        #region ParseCode

        [Test]
        public void parseCode_should_return_empty_envoriment()
        {
            // Given 
            const string code = @"\begin{enumerate} \item Item 1 \item Item 2 \end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}";

            // When 
            var enumerate = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.AreEqual(EnvelopeCommand.enumerate, enumerate.Type);
            Assert.IsEmpty(enumerate.InnerElements);
        }

        [Test]
        public void parseCode_should_return_empty_enumerate_with_double_start()
        {
            // Given 
            const string code = @"\\begin{enumerate} \\item Item 1 \\item Item 2 \\end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}";

            // When 
            var enumerate = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.AreEqual(EnvelopeCommand.enumerate, enumerate.Type);
            Assert.IsEmpty(enumerate.InnerElements);
        }

        [Test]
        public void parseCode_should_return_null_if_end_code()
        {
            // Given 
            const string code = @"\end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}";

            // When 
            var enumerate = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.IsNull(enumerate);
        }

        [Test]
        public void parseCode_should_update_code()
        {
            // Given 

            var code = new StringBuilder(@"\begin{enumerate} \item Item 1 \item Item 2 \end{enumerate}" +
                @"\section{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@" \item Item 1 \item Item 2 \end{enumerate}" +
                            @"\section{Section 1}" +
                            @"This is some text " +
                            @"\chapter{Chapter 2}" +
                            @"\section{Section 2}" +
                            @"This is some other text" +
                            @"\end{document}", code.ToString());

        }
        [Test]
        public void parseCode_should_update_code_double()
        {
            // Given 

            var code = new StringBuilder(@"\\begin{enumerate} \\item Item 1 \\item Item 2 \\end{enumerate}" +
                @"\section{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@" \\item Item 1 \\item Item 2 \\end{enumerate}" +
                            @"\section{Section 1}" +
                            @"This is some text " +
                            @"\chapter{Chapter 2}" +
                            @"\section{Section 2}" +
                            @"This is some other text" +
                            @"\end{document}", code.ToString());

        }

        [Test]
        public void parseCode_should_update_code_for_end_command()
        {
            // Given 
            var code = new StringBuilder(@"\end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}", code.ToString());
        }

        [Test]
        public void parseCode_should_update_code_for_end_command_for_double()
        {
            // Given 
            var code = new StringBuilder(@"\\end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(@"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}", code.ToString());
        }

        [Test]
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.ParseCode(new StringBuilder("my text")));
            Assert.AreEqual("The code didn't start with an allowed indicator", ex.Message);
        }

        #endregion ParseCode

        #region TestHelpers

        private static IEnumerable<EnvelopeCommand> allCommands()
        {
            return EnumUtil.GetValues<EnvelopeCommand>();
        }

        #endregion TestHelpers
    }
}