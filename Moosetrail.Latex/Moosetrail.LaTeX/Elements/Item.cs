using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class representing items in a list in latex
    /// </summary>
    public class Item : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Item class
        /// </summary>
        public Item()
        {
            Elements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get/Set the elements of the item
        /// </summary>
        public List<LaTeXElement> Elements { get; set; }
    }
}