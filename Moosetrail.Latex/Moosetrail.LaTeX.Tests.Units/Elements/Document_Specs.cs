using System.Text;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Document_Specs
    {
        private Document SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Document();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElement()
        {
            Assert.IsInstanceOf<LaTeXElement>(SUT);
        }

        #region Empty Constructor

        [Test]
        public void constructor_should_set_elements_to_empty()
        {
            // When 
            SUT = new Document();

            // Then
            Assert.IsEmpty(SUT.Elements);
        }

        #endregion Empty Constructor

        #region Code constructor

        [Test]
        public void document_should_set_author()
        {
            // When 
            SUT = new Document(BasicDocument);

            // Then
            Assert.AreEqual("John Doe", SUT.Author);
        }
        [Test]
        public void document_should_leavel_author_empty_with_no_author()
        {
            // Given 
            var code = new StringBuilder(BasicDocument);
            code.Replace(@"\author{John Doe}", "");

            // When 
            SUT = new Document(code.ToString());

            // Then
            Assert.IsEmpty(SUT.Author);
        }
        [Test]
        public void document_should_set_title()
        {
            // When 
            SUT = new Document(BasicDocument);

            // Then
            Assert.AreEqual("My title", SUT.Title);
        }
        [Test]
        public void document_should_leavel_title_empty_with_no_title()
        {
            // Given 
            var code = new StringBuilder(BasicDocument);
            code.Replace(@"\title{My title}", "");

            // When 
            SUT = new Document(code.ToString());

            // Then
            Assert.IsEmpty(SUT.Title);
        }
        [Test]
        public void document_should_set_makeTitle()
        {
            // When 
            SUT = new Document(BasicDocument);

            // Then
            Assert.IsTrue(SUT.HasMakeTitle);
        }
        [Test]
        public void document_should_set_hasMakeTitle_to_false_if_no()
        {
            // Given 
            var code = new StringBuilder(BasicDocument);
            code.Replace(@"\maketitle", "");

            // When 
            SUT = new Document(code.ToString());

            // Then
            Assert.IsFalse(SUT.HasMakeTitle);
        }
        [Test]
        public void document_should_have_two_chapters()
        {
            // When 
            SUT = new Document(BasicDocument);

            // Then
            Assert.AreEqual(2, SUT.Elements.Count);
        }

        #endregion Code constructor

        #region TestHelpers

        private const string BasicDocument =
           @"\documentclass[11pt]{report}" +
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