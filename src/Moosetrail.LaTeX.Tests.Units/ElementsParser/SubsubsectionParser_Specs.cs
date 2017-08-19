using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.Exceptions;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class SubsusectionParser_Specs : FormatterParser_Specs
    {
      
        #region SetChildElement

        [Test]
        [TestCaseSource(nameof(_higherHierarchyElements))]
        public void setChildElement_should_throw_if_child_is_higher_hirarcy(LaTeXElement element)
        {
            // Given 

            // Then
            var ex = Assert.Throws<LaTeXException>(() => SUT.SetChildElement(new Formatter(FormatterCommand.subsubsection),element));
            Assert.AreEqual("A Subsubsection can't have a DocumentClass, Document, Chapter, Section, Subsection or Subsubsection as a child", ex.Message);
        }

        #endregion SetChildElement

        #region Parse Code

        [Test]
        public void parseCode_should_return_formatter_for_subsubsection()
        {
            // Given
            var code = @"\subsubsection{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}";

            // When 
            var chapter = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.AreEqual(FormatterCommand.subsubsection, chapter.Type);
        }

        #endregion Parse Code

        #region TestHelpers

        private static readonly List<LaTeXElement> _higherHierarchyElements = new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Formatter(FormatterCommand.chapter),
            new Formatter(FormatterCommand.section),
            new Formatter(FormatterCommand.subsection),
            new Formatter(FormatterCommand.subsubsection)
        };

        #endregion TestHelpers
    }
}