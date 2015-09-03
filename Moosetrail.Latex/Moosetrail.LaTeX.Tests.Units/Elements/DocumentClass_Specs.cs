using System;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class DocumentClass_Specs
    {
        private DocumentClass SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new DocumentClass();
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
        
        #region EmptyContrcutor

        [Test]
        public void constructor_should_create_empty_usePackages()
        {
            // When 
            SUT = new DocumentClass();

            // Then
            Assert.IsEmpty(SUT.UsePackages);
        }

        #endregion EmptyContrcutor

        #region CodeIndicators Static

        [Test]
        public void codeIndicatorsStatic_should_return_documentClass()
        {
            CollectionAssert.Contains(DocumentClass.CodeIndicators, @"\\documentclass");
        }

        [Test]
        public void codeIndicatorsStatic_should_return_usePackages_witout_parameters()
        {
            CollectionAssert.Contains(DocumentClass.CodeIndicators, @"\\usepackage\{([^}]+)\}");
        }

        [Test]
        public void codeIndicatorsStatic_should_return_usePackages_with_parameters()
        {
            CollectionAssert.Contains(DocumentClass.CodeIndicators, @"\\usepackage\[([^}]+)\}");
        }

        #endregion CodeIndicators Static

        #region CodeIndicators

        [Test]
        public void codeIndicators_should_return_documentClass()
        {
            // Given 
            LaTeXElement sut = new DocumentClass();
            
            // When
            var result = sut.CodeIndicators;

            // Then
            CollectionAssert.Contains(result, @"\\documentclass");
        }

        [Test]
        public void codeIndicators_should_return_usePackages_witout_parameter()
        {
            // Given 
            LaTeXElement sut = new DocumentClass();

            // When
            var result = sut.CodeIndicators;

            // Then
            CollectionAssert.Contains(result, @"\\usepackage\{([^}]+)\}");
        }

        [Test]
        public void codeIndicators_should_return_usePackages_with_parameter()
        {
            // Given 
            LaTeXElement sut = new DocumentClass();

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
            // When 
            SUT.ParseCode(BasicDocument);

            // Then
            CollectionAssert.Contains(SUT.UsePackages, "[utf8]inputenc");
            CollectionAssert.Contains(SUT.UsePackages, "babel");
        }
        [Test]
        public void parseCode_should_leave_usePackages_empty_if_no_packages()
        {
            // When 
            SUT.ParseCode(BasicDocumentWithoutPackages);

            // Then
            Assert.IsEmpty(SUT.UsePackages);
        }

        [Test]
        public void parseCode_should_handle_start_with_packages()
        {
            // Given 
            const string str = @"\usepackage[utf8][test]{inputenc}" +
                               @"\usepackage{babel}" +
                               @"\begin{document}";

            // When 
            SUT.ParseCode(str);

            // Then
            CollectionAssert.Contains(SUT.UsePackages, "[utf8][test]inputenc");
            CollectionAssert.Contains(SUT.UsePackages, "babel");
        }

        [Test]
        public void parseCode_should_return_the_remaining_string()
        {
            // Given 
            const string str = @"\usepackage[utf8]{inputenc}" +
                               @"\usepackage{babel}" +
                               @"\begin{document}";

            // When 
            var result = SUT.ParseCode(str);

            // Then
            Assert.AreEqual(@"\begin{document}", result);
        }

        [Test]
        public void parseCode_should_not_do_anything_if_no_dataClass_code()
        {
            // When 
            try
            {
                SUT.ParseCode("My text");
            }
            catch (ArgumentException)
            {}

            // Then
            Assert.IsNull(SUT.Document);
            Assert.IsEmpty(SUT.UsePackages);
        }

        [Test]
        public void parseCode_should_throw_if_string_doesnt_start_with_codeIndicator()
        {
            Assert.Throws<ArgumentException>(() => SUT.ParseCode("My code"));
        }

        #endregion ParseCode

        #region SetChildElement

        [Test]
        public void setChildElement_should_set_document()
        {
            // Given 
            var document = new Document();

            // When 
            SUT.SetChildElement(document);

            // Then
            Assert.AreSame(document, SUT.Document);
        }

        [Test]
        public void setChildElement_should_throw_if_there_is_no_element_set()
        {
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement());
            Assert.AreEqual("No document supplied to set as child", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_two_documents_are_supplied()
        {
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new Document(), new Document()));
            Assert.AreEqual("More than one document supplied, not accepted", ex.Message);
        }

        [Test]
        public void setChildElement_should_throw_if_suplied_element_isnt_a_document()
        {
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new TextBody()));
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