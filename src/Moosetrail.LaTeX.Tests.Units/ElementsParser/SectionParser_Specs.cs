using System;
using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class SectionParser_Specs
    {
        private SectionParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new SectionParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTeXElementParser>(SUT);
        }

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_contain_begin_document()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\section");
        }

        [Test]
        public void codeIndicators_should_contain_begin_section_handled()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\\\section");
        }

        #endregion CodeIndicators

        #region SetChildElement

        [Test]
        public void setChildElement_should_set_elemnts()
        {
            // Given 
            var text1 = new TextBody();
            var text2 = new TextBody();
            var section = new Section();

            // When 
            SUT.SetChildElement(section, text1, text2);

            // Then
            Assert.AreSame(text1, section.Elements[0]);
            Assert.AreSame(text2, section.Elements[1]);
        }

        [Test]
        [TestCaseSource(nameof(_higherHierarchyElements))]
        public void setChildElement_should_throw_if_child_is_higher_hirarcy(LaTeXElement element)
        {
            // Given 

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new Section(),element));
            Assert.AreEqual("A Section can't have a DocumentClass, Document or Chapter as a child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var section = new Section();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(section));
            Assert.AreEqual("No child elements supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_chapter()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody(), new TextBody()));
            Assert.AreEqual("The supplied element wasn't a Section, only Section is allowed", ex.Message);
        }

        #endregion SetChildElement

        #region Parse Code

        [Test]
        public void parseCode_should_set_name()
        {
            // Given 
            var code = @"\section{Section 1}" +
                       @"This is some text " +
                       @"\chapter{Chapter 2}" +
                       @"\section{Section 2}" +
                       @"This is some other text" +
                       @"\end{document}";

            // When 
            var section = SUT.ParseCode(new StringBuilder(code)) as Section;

            // Then
            Assert.AreEqual("Section 1", section.Name);
        }

        [Test]
        public void parseCode_should_return_remaining_code()
        {
            // Given 
            var code =
               new StringBuilder(@"\section{Section 1}" +
                @"This is some text " +
                @"\chapter{Chapter 2}" +
                @"\section{Section 2}" +
                @"This is some other text" +
                @"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(
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

        #endregion Parse Code

        #region TestHelpers

        private static readonly List<LaTeXElement> _higherHierarchyElements = new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Chapter(),
            new Section()
        };

        #endregion TestHelpers
    }
}