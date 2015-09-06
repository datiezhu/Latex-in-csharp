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
    }
}
