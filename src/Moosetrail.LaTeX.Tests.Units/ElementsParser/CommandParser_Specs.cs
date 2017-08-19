using System;
using System.Collections.Generic;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using Moosetrail.LaTeX.Helpers;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class CommandParser_Specs
    {
        private CommandParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new CommandParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser_for_Element()
        {
            Assert.IsInstanceOf<LaTexElementParser<Command>>(SUT);
        }
        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTeXElementParser>(SUT);
        }

        #region CodeIndicators

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_document(CommandType commandType)
        {
            CollectionAssert.Contains(((LaTexElementParser<Command>)SUT).CodeIndicators, @"\\begin{" + commandType + "}");
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled(CommandType commandType)
        {
            CollectionAssert.Contains(((LaTexElementParser<Command>)SUT).CodeIndicators, @"\\\\begin{" + commandType + "}");
        }

        [Test]
        [TestCaseSource(nameof(allCommands))]
        public void codeIndicators_should_contain_begin_enumerate_handled_without_begin_escaped(CommandType commandType)
        {
            CollectionAssert.Contains(((LaTexElementParser<Command>)SUT).CodeIndicators, "\\\\begin{" + commandType + "}");
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

        private static IEnumerable<CommandType> allCommands()
        {
            return EnumUtil.GetValues<CommandType>();
        }

        #endregion TestHelpers
    }
}