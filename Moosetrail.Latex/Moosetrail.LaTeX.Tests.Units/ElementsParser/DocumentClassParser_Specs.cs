using System;
using System.Text;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.ElementsParser;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class DocumentClassParser_Specs
    {
        private DocumentClassParser SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new DocumentClassParser();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        #region CodeIndicators Static

        [Test]
        public void codeIndicatorsStatic_should_return_documentClass()
        {
            CollectionAssert.Contains(DocumentClassParser.CodeIndicators, @"\\documentclass");
        }

        [Test]
        public void codeIndicatorsStatic_should_return_usePackages_witout_parameters()
        {
            CollectionAssert.Contains(DocumentClassParser.CodeIndicators, @"\\usepackage\{([^}]+)\}");
        }

        [Test]
        public void codeIndicatorsStatic_should_return_usePackages_with_parameters()
        {
            CollectionAssert.Contains(DocumentClassParser.CodeIndicators, @"\\usepackage\[([^}]+)\}");
        }

        #endregion CodeIndicators Static

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_return_documentClass()
        {
            // Given 
            LaTeXElementParser sut = new DocumentClassParser();

            // When
            var result = sut.CodeIndicators;

            // Then
            CollectionAssert.Contains(result, @"\\documentclass");
        }

        [Test]
        public void codeIndicators_should_return_usePackages_witout_parameter()
        {
            // Given 
            LaTeXElementParser sut = new DocumentClassParser();

            // When
            var result = sut.CodeIndicators;

            // Then
            CollectionAssert.Contains(result, @"\\usepackage\{([^}]+)\}");
        }

        [Test]
        public void codeIndicators_should_return_usePackages_with_parameter()
        {
            // Given 
            LaTeXElementParser sut = new DocumentClassParser();

            // When
            var result = sut.CodeIndicators;

            // Then
            CollectionAssert.Contains(result, @"\\usepackage\[([^}]+)\}");
        }

        #endregion CodeIndicators

        #region ParseCode

        [Test]
        public void parseCode_should_set_usePacakges()
        {
            // Given

            // When 
            var docClass = SUT.ParseCode(new StringBuilder(BasicDocument)) as DocumentClass;

            // Then
            CollectionAssert.Contains(docClass.UsePackages, "[utf8]inputenc");
            CollectionAssert.Contains(docClass.UsePackages, "babel");
        }
        [Test]
        public void parseCode_should_leave_usePackages_empty_if_no_packages()
        {
            // Given

            // When 
            var docClass = SUT.ParseCode(new StringBuilder(BasicDocumentWithoutPackages)) as DocumentClass;

            // Then
            Assert.IsEmpty(docClass.UsePackages);
        }

        [Test]
        public void parseCode_should_handle_start_with_packages()
        {
            // Given 
            const string str = @"\usepackage[utf8][test]{inputenc}" +
                               @"\usepackage{babel}" +
                               @"\begin{document}";

            // When 
            var docClass = SUT.ParseCode(new StringBuilder(str)) as DocumentClass;

            // Then
            CollectionAssert.Contains(docClass.UsePackages, "[utf8][test]inputenc");
            CollectionAssert.Contains(docClass.UsePackages, "babel");
        }

        [Test]
        public void parseCode_should_return_the_remaining_string()
        {
            // Given 
            const string str = @"\documentclass[11pt]{report}" +
                               @"\usepackage[utf8]{inputenc}" +
                               @"\usepackage{babel}" +
                               @"\begin{document}";
            var sb = new StringBuilder(str);

            // When 
            SUT.ParseCode(sb);

            // Then
            Assert.AreEqual(@"\begin{document}", sb.ToString());
        }
        [Test]
        public void parseCode_should_return_the_remaining_string_with_linebreak_in_class()
        {
            // Given 
            const string str = @"\documentclass[11pt]{report}\n" +
                               @"\usepackage[utf8]{inputenc}" +
                               @"\usepackage{babel}" +
                               @"\begin{document}";
            var sb = new StringBuilder(str);

            // When 
            SUT.ParseCode(sb);

            // Then
            Assert.AreEqual(@"\n\begin{document}", sb.ToString());
        }

        [Test]
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            // Given
           

            // Then
            Assert.Throws<ArgumentException>(() => SUT.ParseCode(new StringBuilder("My text")));
        }

        #endregion ParseCode

        #region SetChildElement

        [Test]
        public void setChildElement_should_set_document()
        {
            // Given 
            var docClass = new DocumentClass();
            var document = new Document();

            // When 
            SUT.SetChildElement(docClass, document);

            // Then
            Assert.AreSame(document, docClass.Document);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_children_set()
        {
            // Given
            var docClass = new DocumentClass();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(docClass));
            Assert.AreEqual("No child document supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_two_children_are_supplied()
        {
            // Given
            var docClass = new DocumentClass();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(docClass, new Document(), new Document()));
            Assert.AreEqual("More than one child element supplied, not accepted", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_child_isnt_a_document()
        {
            // Given
            var docClass = new DocumentClass();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(docClass, new TextBody()));
            Assert.AreEqual("The supplied child wasn't a Document, only Document is allowed", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_documentClass()
        {
            // Given
            var docClass = new DocumentClass();

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody(), new Document()));
            Assert.AreEqual("The supplied element wasn't a Document, only Document is allowed", ex.Message);
        }

        #endregion SetChildElement

        #region TestHelpers

        private const string BasicDocument =
           @"\documentclass[11pt]{report}" +
           @"\usepackage[utf8]{inputenc}" +
           @"\usepackage{babel}" +
           @"\begin{document}" +
           @"\title{My title}" +
           @"\author{John Doe}" +
           @"\maketitle" +
           @"\chapter{Chapter 1}\n" +
           @"\section{Section 1}" +
           @"This is some text " +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"This is some other text" +
           @"\begin{enumerate}" +
           @"\item First element " +
           @"\item Second element" +
           @"\end{enumerate}" +
           "My second text" +
           @"\end{document}";

        private const string BasicDocumentWithoutPackages =
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