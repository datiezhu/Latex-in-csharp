using System;
using System.Linq;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class EnumerateParser_Specs
    {
        private EnumerateParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new EnumerateParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElementParser()
        {
            Assert.IsInstanceOf<LaTexElementParser<Enumerate>>(SUT);
        }

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_contain_begin_document()
        {
            CollectionAssert.Contains(((LaTexElementParser<Enumerate>)SUT).CodeIndicators, @"\\begin{enumerate}");
        }

        [Test]
        public void codeIndicators_should_contain_begin_enumerate_handled()
        {
            CollectionAssert.Contains(((LaTexElementParser<Enumerate>)SUT).CodeIndicators, @"\\\\begin{enumerate}");
        }

        #endregion CodeIndicators

        #region GetEmptyElement

        [Test]
        public void getEmptyElement_should_return_an_enumerate()
        {
            // When 
            var result = SUT.GetEmptyElement();

            // Then
            Assert.IsNotNull(result);
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
        public void setChildElement_should_throw_if_child_isnt_item()
        {
            // Given 

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new Enumerate(), new TextBody()));
            Assert.AreEqual("Enumerate can only hold Item objects", ex.Message);
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

        #endregion SetChildElement

        #region ParseCode

        [Test]
        public void parseCode_should_return_empty_enumerate()
        {
            // Given 
            const string code = @"\begin{enumerate} \item Item 1 \item Item 2 \end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}";

            // When 
            var enumerate = SUT.ParseCode(new StringBuilder(code)) as Enumerate;

            // Then
            Assert.IsEmpty(enumerate.ItemList);
        }

        [Test]
        public void parseCode_should_return_null_if_end_code()
        {
            // Given 
            const string code = @"\end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}";

            // When 
            var enumerate = SUT.ParseCode(new StringBuilder(code)) as Enumerate;

            // Then
            Assert.IsNull(enumerate);
        }

        [Test]
        public void parseCode_should_update_code()
        {
            // Given 

            var code =new StringBuilder(@"\begin{enumerate} \item Item 1 \item Item 2 \end{enumerate}" +
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
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.ParseCode(new StringBuilder("my text")));
            Assert.AreEqual("The code didn't start with an allowed indicator", ex.Message);
        }

        #endregion ParseCode
    }
}