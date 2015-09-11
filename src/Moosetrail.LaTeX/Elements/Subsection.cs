using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class representing a Subsection in LaTeX code
    /// </summary>
    public class Subsection : ContentContext
    {
        /// <summary>
        /// Creates a new instance of the Subsection class
        /// </summary>
        public Subsection()
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