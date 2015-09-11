using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class representing a latex chapter
    /// </summary>
    public class Chapter : LaTeXElement
    {
        /// <summary>
        /// Creates a new isntance of the Chpter class
        /// </summary>
        public Chapter()
        {
            Elements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get/Set the name of the chapter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set the elements of the chapter
        /// </summary>
        public List<LaTeXElement> Elements { get; set; }
    }
}