using System;
using System.IO;
using System.Linq;
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
            Assert.AreEqual(7, enuerate.ItemList.Count());
        }

        [Test]
        public void prase_should_document_with_list_sublit_and_emph()
        {
            // Given 
            var uri = new Uri(new Uri(Environment.CurrentDirectory), "../../TestData/ListWithSublistAndEmph.tex");
            var code = File.ReadAllText(uri.AbsolutePath);

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            var section = result.ElementAt(0) as Section;
            var enumerate = section.Elements.ElementAt(0) as Enumerate;
            var subList = enumerate.ItemList.ElementAt(0).Elements.ElementAt(1) as Enumerate;
            var firstItemInSubList = subList.ItemList.ElementAt(0) as Item;

            Assert.AreEqual(3, enumerate.ItemList.Count);
            Assert.AreEqual(4, subList.ItemList.Count);
            Assert.AreEqual(@"\emph{Viktning}. Vik $h(k)$ runt $k=0$ för att få $h(-k)$", firstItemInSubList.Elements.ElementAt(0).ToString());
        }

        #endregion Pasre Full Text

        #region Parse Strucutred document

        [Test]
        public void praseStructuredDocument_should_find_chapters()
        {
            // Given 
            var uri = new Uri(new Uri(Environment.CurrentDirectory), "../../TestData/ListWithSubsections.tex");
            var code = File.ReadAllText(uri.AbsolutePath);

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            var docClass = result.ElementAt(0) as DocumentClass;
            Assert.AreEqual(2, docClass.Document.Elements.Count);
            Assert.IsTrue(docClass.Document.Elements.All(x => x is Chapter));
        }
        [Test]
        public void praseStructuredDocument_should_find_sections_in_chapter()
        {
            // Given 
            var uri = new Uri(new Uri(Environment.CurrentDirectory), "../../TestData/ListWithSubsections.tex");
            var code = File.ReadAllText(uri.AbsolutePath);

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            var docClass = result.ElementAt(0) as DocumentClass;
            foreach (var chapter in docClass.Document.Elements.Select(x => x as ContentContext))
            {
                Assert.AreEqual(2, chapter.Elements.Count);
                Assert.IsTrue(chapter.Elements.All(x => x is Section));
            }
        }
        [Test]
        public void praseStructuredDocument_should_find_subsections_in_section()
        {
            // Given 
            var uri = new Uri(new Uri(Environment.CurrentDirectory), "../../TestData/ListWithSubsections.tex");
            var code = File.ReadAllText(uri.AbsolutePath);

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            var docClass = result.ElementAt(0) as DocumentClass;
            foreach (var chapter in docClass.Document.Elements.Select(x => x as ContentContext))
            {
                foreach (var section in chapter.Elements.Select(x => x as ContentContext))
                {
                    Assert.AreEqual(2, section.Elements.Count);
                    Assert.IsTrue(section.Elements.All(x => x is Subsection));
                }
            }
        }

        #endregion Parse Strucutred document

        #region Parse Enumerate

        [Test]
        public void parse_should_should_handle_enumerate_with_math()
        {
            // Given 
            var code = @"\begin{enumerate}	\item är parvis oförenliga \(H_1, \dots, H_n\)	\item \(H_1 \cup \dots \cup H_n = \Omega\) \end{enumerate}";

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            Assert.IsNotNull(result);
        }

        [Test]
        public void parse_should_should_handle_sub_enumerate_text_after()
        {
            // Given 
            var code = @"\begin{enumerate}	\item är parvis oförenliga \(H_1, \dots, H_n\) \begin{enumerate}\item My subitem \end{enumerate} My trailing text \end{enumerate}";

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            Assert.AreEqual(1, result.Count(), "To many parent elements");
            Assert.AreEqual(1, ((Enumerate)result.ElementAt(0)).ItemList.Count, "To many items");
        }

        #endregion Parse Enumerate

        #region ParseText

        [Test]
        public void praseTextWithMthCommands_should_parse_correcty()
        {
            // Given 
            var uri = new Uri(new Uri(Environment.CurrentDirectory), "../../TestData/MathWithInlineCommands.tex");
            var code = File.ReadAllText(uri.AbsolutePath);

            // When 
            var result = LatexParser.ParseCode(code);

            // Then
            var body = result.ElementAt(0) as TextBody;
            Assert.AreEqual(
                @"Definiera den likformigt fördelat för en kontinuerligt s.v. $X$ - Om den s.v. $X$ har täthetsfunktionen \[ f_X(x) =\begin{cases}\frac{1}{(b-a)} & \quad \text{om } a < x < b\\ 0  & \quad  \text{annars}\\ \end{cases} \] sägs $X$ vara likformigt fördelad",
                body.ToString());
        }

        #endregion ParseText

        [Test]
        public void parseContent_should_parse_handled_code()
        {
            // Given 
            var code =
                @"\\chapter{Introduction}\n\\section{Signals, Systems and Signal Processing}\n\\begin{enumerate}\n\t\\item What is the definition of a signal? - Any physical quantity that varies with time, space or any other independent variable or variables\n\t\\item What does the variable $t$ represent? - Time\n\t\\item What variable represents time? - $t$\n\t\\item What is signal generation usually associated with? - A system that responds to a stimulus or force\n\t\\item What is an alternate definition of system that does not have to do with stimulus or force? - A system can be a physical device that preforms an operation on a signal \n\t\\item What are analog signals? - Functions of a continuous variable such as time\n\\end{enumerate}\n";

            // Then 
            var result = LatexParser.ParseCode(code);

            // When
            var chapter = result.ElementAt(0) as Chapter; 
            Assert.NotNull(chapter);
        }

        [Test]
        public void parseContent_should_parse_item_with_slash_math_as_expected()
        {
            // Given 
            var code =
                @"\\item What is the official definition of an analog signal? - A signal that is defined for every value of time and take on values in the continuous interval $(a, b)$ where $a$ can be $-\\infty$ and $b$ can be $\\infty$";

            // Then 
            var result = LatexParser.ParseCode(code);

            // When
            var item = result.ElementAt(0) as Item;
            Assert.NotNull(item);
            Assert.AreEqual("\\item What is the official definition of an analog signal? - A signal that is defined for every value of time and take on values in the continuous interval $(a, b)$ where $a$ can be $-\\infty$ and $b$ can be $\\infty$\n ", item.ToString());
        }

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