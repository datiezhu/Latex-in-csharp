using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class representing a Subsubsection in LaTeX code
    /// </summary>
    public class Subsubsection : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Subsubsection class
        /// </summary>
        public Subsubsection()
        {
            Elements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get/Set the name of the section
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set the elements of the section
        /// </summary>
        public List<LaTeXElement> Elements { get; set; }
    }
}