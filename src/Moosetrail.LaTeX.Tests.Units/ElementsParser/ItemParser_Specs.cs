using System;
using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class ItemParser_Specs
    {
        private ItemParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new ItemParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTexElementParser<Item>>(SUT);
        }

        #region GetEmptyElement

        [Test]
        public void getEmptyElement_should_return_item()
        {
            // When 
            var result = SUT.GetEmptyElement();

            // Then
            Assert.IsNotNull(result);
        }

        #endregion GetEmptyElement

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_contain_begin_document()
        {
            CollectionAssert.Contains(((LaTexElementParser<Item>)SUT).CodeIndicators, @"\\item");
        }

        [Test]
        public void codeIndicators_should_contain_begin_item_handled()
        {
            CollectionAssert.Contains(((LaTexElementParser<Item>)SUT).CodeIndicators, @"\\\\item");
        }

        #endregion CodeIndicators

        #region SetChildElement

        [Test]
        public void setChildElement_should_set_elemnts()
        {
            // Given 
            var text1 = new TextBody();
            var text2 = new TextBody();
            var item = new Item();

            // When 
            SUT.SetChildElement(item, text1, text2);

            // Then
            Assert.AreSame(text1, item.Elements[0]);
            Assert.AreSame(text2, item.Elements[1]);
        }

        [Test]
        public void setChildElement_should_throw_if_child_is_item()
        {
            // Given
            var item = new Item();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(item, new Item()));
            Assert.AreEqual("Can't set Item as child for Item", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var item = new Item();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(item));
            Assert.AreEqual("No child elements supplied to set as child", ex.Message);
        }

        [Test]
        [TestCaseSource(nameof(_higherHierarchyElements))]
        public void setChildElement_should_throw_if_hirarcy_is_higher(LaTeXElement element)
        {
            // When 
            var item = new Item();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(item, element));
            Assert.AreEqual("An Item can't have a DocumentClass, Document Chapter or other element that is concidered to organize information", ex.Message);
        }

        #endregion SetChildElement

        #region Parse Code

        [Test]
        public void parseCode_should_return_remaining_code()
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
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(
                " My item" + 
                  @"\item My other item" +
                @"\chapter{Chapter 2}" +
                @"\section{Section 2}" +
                @"This is some other text" +
                @"\end{document}", code.ToString());
        }

        [Test]
        public void parseCode_should_return_remaining_code_when_double_code()
        {
            // Given 
            var code =
               new StringBuilder(@"\\item My item" +
                       @"\item My other item" +
                @"\chapter{Chapter 2}" +
                @"\section{Section 2}" +
                @"This is some other text" +
                @"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.AreEqual(
                " My item" +
                  @"\item My other item" +
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
            new Section(),
            new Subsection(),
            new Subsubsection()
        };

        #endregion TestHelpers
    }
}