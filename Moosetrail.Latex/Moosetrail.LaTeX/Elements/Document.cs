using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class representing everything inside the \document tag 
    /// </summary>
    public class Document : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Document class without setting any data
        /// </summary>
        public Document()
        {
            Elements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Creates a new instance of the Document class and sets its data from the latex code
        /// </summary>
        /// <param name="latexCode">The code to create the object for</param>
        public Document(string latexCode)
        {
            Elements = new List<LaTeXElement>();
            Author = CodeParser.SimpleContent("author", latexCode);
            Title = CodeParser.SimpleContent("title", latexCode);
            HasMakeTitle = latexCode.Contains(@"\maketitle");
        }

        /// <summary>
        /// Get/Set the author of the document
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Get/Set the title of the document
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get/Set flag to indicate if make title is commanded in the document
        /// </summary>
        public bool HasMakeTitle { get; set; }

        /// <summary>
        /// Get/Set the elements of the document
        /// </summary>
        public List<LaTeXElement> Elements { get; set; }

        public IEnumerable<string> CodeIndicators
        {
            get { throw new System.NotImplementedException(); }
        }

        public void SetChildElement(params LaTeXElement[] elements)
        {
            throw new System.NotImplementedException();
        }

        public string ParseCode(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}
