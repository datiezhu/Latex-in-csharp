using System;
using System.Collections.Generic;
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

        #endregion ParseCode

        #region TestHelpers

        private static IEnumerable<FormatterCommand> allCommands()
        {
            return EnumUtil.GetValues<FormatterCommand>();
        }

        #endregion TestHelpers
    }
}