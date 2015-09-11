using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class representing a Section in LaTeX code
    /// </summary>
    public class Section : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Section class
        /// </summary>
        public Section()
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