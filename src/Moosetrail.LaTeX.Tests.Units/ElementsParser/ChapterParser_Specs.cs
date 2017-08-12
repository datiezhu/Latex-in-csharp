using System;
using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.Exceptions;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class ChapterParser_Specs : FormatterParser_Specs
    {
        #region SetChildElement

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
        [TestCaseSource(nameof(_higherHierarchyElements))]
        public void setChildElement_should_throw_if_child_is_higher_hirarcy(LaTeXElement element)
        {
            // Given 

            // Then
            var ex = Assert.Throws<LaTeXException>(() => SUT.SetChildElement(new Formatter(FormatterCommand.chapter) ,element));
            Assert.AreEqual("A Chapter can't have a DocumentClass, Document or Chapter as a child", ex.Message);
        }

        #endregion SetChildElement

        #region Parse Code

        [Test]
        public void parseCode_should_set_name()
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
        public void parseCode_should_return_remaining_code()
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
        public void praseCode_should_find_chapter_from_escaped_handled_string()
        {
            // Given 
            var code =
                "\\chapter{Introduction}\n\\section{Signals, Systems and Signal Processing}\n\\begin{enumerate}\n\t\\item What is the definition of a signal? - Any physical quantity that varies with time, space or any other independent variable or variables\n\t\\item What does the variable $t$ represent? - Time\n\t\\item What variable represents time? - $t$\n\t\\item What is signal generation usually associated with? - A system that responds to a stimulus or force\n\t\\item What is an alternate definition of system that does not have to do with stimulus or force? - A system can be a physical device that preforms an operation on a signal \n\t\\item What are analog signals? - Functions of a continuous variable such as time\n\\end{enumerate}\n";

            // When 
            var result = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.AreEqual("Introduction", ((TextBody)result.InnerElements[0]).TheText);
        }

        #endregion Parse Code

        #region TestHelpers

        private static readonly List<LaTeXElement> _higherHierarchyElements = new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Formatter(FormatterCommand.chapter)
        };

        #endregion TestHelpers
    }
}