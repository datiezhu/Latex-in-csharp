using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Interface for all Content Contexts such as chapter, section and so on
    /// </summary>
    public interface ContentContext : LaTeXElement
    {
        /// <summary>
        /// Get/Set the name of the chapter
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Get/Set the elements of the chapter
        /// </summary>
        List<LaTeXElement> Elements { get; set; }
    }
}