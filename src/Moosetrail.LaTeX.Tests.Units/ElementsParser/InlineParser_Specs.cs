using System;
using System.Collections.Generic;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using Moosetrail.LaTeX.Helpers;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class InlineParser_Specs
    {
        private InlineParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new InlineParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser_for_Element()
        {
            Assert.IsInstanceOf<LaTexElementParser<Inline>>(SUT);
        }
        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTeXElementParser>(SUT);
        }

        #region CodeIndicators

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_document(InlineCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Inline>)SUT).CodeIndicators, @"\\begin{" + command + "}");
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled(InlineCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Inline>)SUT).CodeIndicators, @"\\\\begin{" + command + "}");
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled_without_begin_escaped(InlineCommand command)
        {
            CollectionAssert.Contains(((LaTexElementParser<Inline>)SUT).CodeIndicators, "\\\\begin{" + command + "}");
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

        

        #endregion SetChiildElement

        #region ParseCode

        #endregion ParseCode

        #region TestHelpers

        private static IEnumerable<InlineCommand> allCommands()
        {
            return EnumUtil.GetValues<InlineCommand>();
        }

        #endregion TestHelpers
    }
}