using System;
using System.IO;
using System.Linq;
using System.Text;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units
{
    [TestFixture]
    public class LatexParser_Specs
    {
        #region Parse DocumentClass

        [Test]
        public void parseDocumentclass_should_set_packages()
        {
            // Given
            const string code = @"\documentclass[11pt]{report}" +
                                @"\usepackage[utf8]{inputenc}" +
                                @"\usepackage{babel}" +
                                @"\begin{document}" +
                                @"\title{My title}" +
                                @"\author{John Doe}" +
                                @"\maketitle" +
                                @"\chapter{Chapter 1}" +
                                @"\section{Section 1}" +
                                @"\chapter{Chapter 2}" +
                                @"\section{Section 2}" +
                                @"\begin{enumerate}" +
                                @"\item First element " +
                                @"\item Second element" +
                                @"\end{enumerate}" +
                                @"\end{document}";

            // When 
            var result = LatexParser.ParseCode(code);
            var doc = result.ElementAt(0) as DocumentClass;

            // Then
            Assert.NotNull(doc);
            Assert.IsNotEmpty(doc.UsePackages);
        }

        [Test]
        public void parseDocumentClass_should_only_return_one_element()
        {
            // Given
            const string code = @"\documentclass[11pt]{report}" +
                                @"\usepackage[utf8]{inputenc}" +
                                @"\usepackage{babel}" +
                                @"\begin{document}" +
                                @"\title{My title}" +
                                @"\author{John Doe}" +
                                @"\maketitle" +
                                @"\chapter{Chapter 1}" +
                                @"\section{Section 1}" +
                                @"\chapter{Chapter 2}" +
                                @"\section{Section 2}" +
                                @"\begin{enumerate}" +
                                @"\item First element " +
                                @"\item Second element" +
                                @"\end{enumerate}" +
                                @"\end{document}";

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void parseDocumentClass_should_set_document()
        {
            // Given
            const string code = @"\documentclass[11pt]{report}" +
                                @"\usepackage[utf8]{inputenc}" +
                                @"\usepackage{babel}" +
                                @"\begin{document}" +
                                @"\title{My title}" +
                                @"\author{John Doe}" +
                                @"\maketitle" +
                                @"\chapter{Chapter 1}" +
                                @"\section{Section 1}" +
                        
                                @"\chapter{Chapter 2}" +
                                @"\section{Section 2}" +
                           
                                @"\begin{enumerate}" +
                                @"\item First element " +
                                @"\item Second element" +
                                @"\end{enumerate}" +
                  
                                @"\end{document}";

            // When 
            var result = LatexParser.ParseCode(code);
            var doc = result.ElementAt(0) as DocumentClass;

            // Then
            Assert.NotNull(doc.Document);
        }

        #endregion Parse DocumentClass

        #region Parse Document

        [Test]
        public void document_should_set_docInfo()
        {
            // Given
            const string code = @"\begin{document}" +
                                @"\title{My title}" +
                                @"\author{John Doe}" +
                                @"\maketitle" +
                                @"\chapter{Chapter 1}" +
                                @"\section{Section 1}" +
                                @"\chapter{Chapter 2}" +
                                @"\section{Section 2}" +
                                @"\begin{enumerate}" +
                                @"\item First element " +
                                @"\item Second element" +
                                @"\end{enumerate}" +
                                @"\end{document}";

            // When
            var result = LatexParser.ParseCode(code);
            var doc = result.ElementAt(0) as Document;

            // Then
            Assert.AreEqual("My title", doc.Title);
            Assert.AreEqual("John Doe", doc.Author);
            Assert.IsTrue(doc.HasMakeTitle);
        }

        [Test]
        public void document_should_set_all_content_as_children()
        {
            // Given
            const string code = @"\begin{document}" +
                                @"\title{My title}" +
                                @"\author{John Doe}" +
                                @"\maketitle" +
                                @"\chapter{Chapter 1}" +
                                @"\section{Section 1}" +
                                @"\chapter{Chapter 2}" +
                                @"\section{Section 2}" +
                                @"\begin{enumerate}" +
                                @"\item First element " +
                                @"\item Second element" +
                                @"\end{enumerate}" +
                                @"\end{document}";

            // When
            var result = LatexParser.ParseCode(code);
            var doc = result.ElementAt(0) as Document;

            // Then
            Assert.AreEqual(2, doc.Elements.Count);
        }

        #endregion Parse Document

        #region Pasre Full Text

        [Test]
        public void fullDocument_should_parse_full_tree()
        {
            // Given 

            // When 
            var result = LatexParser.ParseCode(BasicRepeatDocument);

            // Then
            var docClass = result.ElementAt(0) as DocumentClass;
            var chapter1 = docClass.Document.Elements.ElementAt(0) as Chapter;
            var chapter2 = docClass.Document.Elements.ElementAt(1) as Chapter;
            var section = chapter2.Elements.ElementAt(0) as Section;
            var enuerate = section.Elements.ElementAt(0) as Enumerate;
            Assert.NotNull(docClass);
            Assert.NotNull(docClass.Document);
            Assert.AreEqual(2, docClass.Document.Elements.Count);
            Assert.AreEqual(1, chapter1.Elements.Count);
            Assert.AreEqual(1, chapter2.Elements.Count);
            Assert.AreEqual(2, enuerate.ItemList.Count());
        }

        [Test]
        public void prase_should_document_with_only_lists()
        {
            // Given 
            var uri = new Uri(new Uri(Environment.CurrentDirectory), "../../TestData/ListDocument.tex");
            var code = File.ReadAllText(uri.AbsolutePath);

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            var docClass = result.ElementAt(0) as DocumentClass;
            var chapter1 = docClass.Document.Elements.ElementAt(0) as Chapter;
            var chapter2 = docClass.Document.Elements.ElementAt(1) as Chapter;
            var section = chapter2.Elements.ElementAt(0) as Section;
            var enuerate = section.Elements.ElementAt(0) as Enumerate;
            Assert.NotNull(docClass);
            Assert.NotNull(docClass.Document);
            Assert.AreEqual(2, docClass.Document.Elements.Count);
            Assert.AreEqual(3, chapter1.Elements.Count);
            Assert.AreEqual(3, chapter2.Elements.Count);
            Assert.AreEqual(2, enuerate.ItemList.Count());
        }

        #endregion Pasre Full Text

        #region TestHelpers

        private const string BasicRepeatDocument =
           @"\documentclass[11pt]{report}" +
           @"\usepackage[utf8]{inputenc}" +
           @"\usepackage{babel}" +
           @"\begin{document}" +
           @"\title{My title}" +
           @"\author{John Doe}" +
           @"\maketitle" +
           @"\chapter{Chapter 1}" +
           @"\section{Section 1}" +
           @"\chapter{Chapter 2}" +
           @"\section{Section 2}" +
           @"\begin{enumerate}" +
           @"\item First element " +
           @"\item Second element" +
           @"\end{enumerate}" +
           @"\end{document}";

        #endregion TestHelpers

    }
}