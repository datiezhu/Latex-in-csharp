﻿using System;
using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class ChapterParser_Specs
    {
        private ChapterParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new ChapterParser();
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
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\chapter");
        }

        #endregion CodeIndicators

        #region SetChildElement

        [Test]
        public void setChildElement_should_set_elemnts()
        {
            // Given 
            var text1 = new TextBody();
            var section = new Section();
            var chpter = new Chapter();

            // When 
            SUT.SetChildElement(chpter, section, text1);

            // Then
            Assert.AreSame(section, chpter.Elements[0]);
            Assert.AreSame(text1, chpter.Elements[1]);
        }

        [Test]
        [TestCaseSource(nameof(_higherHierarchyElements))]
        public void setChildElement_should_throw_if_child_is_higher_hirarcy(LaTeXElement element)
        {
            // Given 

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new Chapter(),element));
            Assert.AreEqual("A Chapter can't have a DocumentClass, Document or Chapter as a child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var chapter = new Chapter();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(chapter));
            Assert.AreEqual("No child elements supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_chapter()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody(), new TextBody()));
            Assert.AreEqual("The supplied element wasn't a Chapter, only Chapter is allowed", ex.Message);
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
            var chapter = SUT.ParseCode(new StringBuilder(code)) as Chapter;

            // Then
            Assert.AreEqual("Chapter 1", chapter.Name);
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

        #endregion Parse Code

        #region TestHelpers

        readonly List<LaTeXElement> _higherHierarchyElements = new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Chapter()
        };

        #endregion TestHelpers
    }
}