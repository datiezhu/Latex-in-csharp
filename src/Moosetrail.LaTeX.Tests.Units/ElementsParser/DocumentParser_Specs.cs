using System;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class DocumentParser_Specs
    {
        private DocumentParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new DocumentParser();
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

        #region ParseCode

        [Test]
        public void parseCode_should_set_author()
        {
            // Given

            // When 
            var doc = SUT.ParseCode(new StringBuilder(BasicDocument)) as Document;

            // Then
            Assert.AreEqual("John Doe", doc.Author);
        }
        [Test]
        public void parseCode_should_leavel_author_empty_with_no_author()
        {
            // Given 
            var code = new StringBuilder(BasicDocument);
            code.Replace(@"\author{John Doe}", "");

            // When 
            var doc = SUT.ParseCode(code) as Document;

            // Then
            Assert.IsEmpty(doc.Author);
        }
        [Test]
        public void parseCode_should_set_title()
        {
            // Given

            // When 
            var doc = SUT.ParseCode(new StringBuilder(BasicDocument)) as Document;

            // Then
            Assert.AreEqual("My title", doc.Title);
        }
        [Test]
        public void parseCode_should_leavel_title_empty_with_no_title()
        {
            // Given 
            var code = new StringBuilder(BasicDocument);
            code.Replace(@"\title{My title}", "");

            // When 
            var doc = SUT.ParseCode(code) as Document;

            // Then
            Assert.IsEmpty(doc.Title);
        }
        [Test]
        public void parseCode_should_set_makeTitle()
        {
            // Given

            // When 
            var doc = SUT.ParseCode(new StringBuilder(BasicDocument)) as Document;

            // Then
            Assert.IsTrue(doc.HasMakeTitle);
        }
        [Test]
        public void parseCode_should_set_hasMakeTitle_to_false_if_no()
        {
            // Given 
            var code = new StringBuilder(BasicDocument);
            code.Replace(@"\maketitle", "");

            // When 
            var doc = SUT.ParseCode(code) as Document;

            // Then
            Assert.IsFalse(doc.HasMakeTitle);
        }

        [Test]
        public void parseCode_should_return_the_remaining_string()
        {
            // Given
            var sb = new StringBuilder(BasicDocument);

            // When 
            SUT.ParseCode(sb);

            // Then
            Assert.AreEqual(@"\chapter{Chapter 1}" +
           @"\section{Section 1}" +
           @"This is some text " +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"This is some other text" +
           @"\end{document}", sb.ToString());
        }

        [Test]
        public void parseCode_should_handle_if_starts_with_title()
        {
            // Given 
            var code = @"\title{My title}" +
           @"\author{John Doe}" +
           @"\maketitle" +
           @"\chapter{Chapter 1}" +
           @"\section{Section 1}" +
           @"This is some text " +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"This is some other text" +
           @"\end{document}";

            // When 
            var doc = SUT.ParseCode(new StringBuilder(code)) as Document;

            // Then
            Assert.IsNotEmpty(doc.Author);
            Assert.IsNotEmpty(doc.Title);
            Assert.IsTrue(doc.HasMakeTitle);
        }
        [Test]
        public void parseCode_should_handle_if_starts_with_author()
        {
            // Given 
            var code =
           @"\author{John Doe}" +
           @"\maketitle" +
           @"\chapter{Chapter 1}" +
           @"\section{Section 1}" +
           @"This is some text " +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"This is some other text" +
           @"\end{document}"; ;

            // When 
            var doc = SUT.ParseCode(new StringBuilder(code)) as Document;

            // Then
            Assert.IsNotEmpty(doc.Author);
            Assert.IsEmpty(doc.Title);
            Assert.IsTrue(doc.HasMakeTitle);
        }
        [Test]
        public void parseCode_should_handle_if_starts_with_makeTitle()
        {
            // Given 
            var code =
           @"\maketitle" +
           @"\chapter{Chapter 1}" +
           @"\section{Section 1}" +
           @"This is some text " +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"This is some other text" +
           @"\end{document}";

            // When 
            var doc = SUT.ParseCode(new StringBuilder(code)) as Document;

            // Then
            Assert.IsEmpty(doc.Author);
            Assert.IsEmpty(doc.Title);
            Assert.IsTrue(doc.HasMakeTitle);
        }

        [Test]
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            // Given

            // Then
            Assert.Throws<ArgumentException>(() => SUT.ParseCode(new StringBuilder("My code")));
        }

        [Test]
        public void parseCode_should_remove_end_clause()
        {
            // Given 
            var code = new StringBuilder(@"\end{document}");

            // When 
            SUT.ParseCode(code);

            // Then
            Assert.IsEmpty(code.ToString());
        }
        [Test]
        public void parseCode_should_return_null_for_end_clause()
        {
            // Given 
            var code = @"\end{document}";

            // When 
            var result = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.IsNull(result);
        }

        #endregion ParseCode

        #region SetChildElement

        [Test]
        public void setChildElement_should_set_elemnts()
        {
            // Given 
            var text1 = new TextBody();
            var section = new Section();
            var document = new Document();

            // When 
            SUT.SetChildElement(document, section, text1);

            // Then
            Assert.AreSame(section, document.Elements[0]);
            Assert.AreSame(text1, document.Elements[1]);
        }

        [Test]
        public void setChildElement_should_throw_if_child_is_documentClass()
        {
            // Given 

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new Document(), new DocumentClass()));
            Assert.AreEqual("A Document can't have a DocumentClass as a child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var doc = new Document();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(doc));
            Assert.AreEqual("No child elements supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_document()
        {
            // Given

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody(), new TextBody()));
            Assert.AreEqual("The supplied element wasn't a Document, only Document is allowed", ex.Message);
        }

        #endregion SetChildElement

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_contain_begin_document()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\begin{document}");
        }
        [Test]
        public void codeIndicators_should_contain_end_document()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\end{document}");
        }
        [Test]
        public void codeIndicators_should_contain_author()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\author");
        }
        [Test]
        public void codeIndicators_should_contain_title()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\title");
        }
        [Test]
        public void codeIndicators_should_contain_maketitle()
        {
            CollectionAssert.Contains(((LaTeXElementParser)SUT).CodeIndicators, @"\\maketitle");
        }

        #endregion CodeIndicators

        #region TestHelpers

        private const string BasicDocument =
           @"\begin{document}" +
           @"\title{My title}" +
           @"\author{John Doe}" +
           @"\maketitle" +
           @"\chapter{Chapter 1}" +
           @"\section{Section 1}" +
           @"This is some text " +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"This is some other text" +
           @"\end{document}";

        #endregion TestHelpers 
    }
}