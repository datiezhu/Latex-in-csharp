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

        #region Code constructor

        [Test]
        public void codeConstructor_should_set_usePacakges()
        {
            // When 
            var result = new DocumentClass(BasicDocument);

            // Then
            CollectionAssert.Contains(result.UsePackages, "[utf8]inputenc");
            CollectionAssert.Contains(result.UsePackages, "babel");
        }
        [Test]
        public void codeConstructor_should_leave_usePackages_empty_if_no_packages()
        {
            // When 
            var result = new DocumentClass(BasicDocumentWithoutPackages);

            // Then
            Assert.IsEmpty(result.UsePackages);
        }

        [Test]
        public void codeConstructor_should_not_do_anything_if_no_dataClass_code()
        {
            // When 
            SUT = new DocumentClass("My text");

            // Then
            Assert.IsNull(SUT.Document);
            Assert.IsEmpty(SUT.UsePackages);
        }

        [Test]
        public void codeConstructor_should_set_document()
        {
            // When 
            SUT = new DocumentClass(BasicDocument);

            // Then
            Assert.IsNotNull(SUT.Document);
        }

        #endregion Code constructor

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