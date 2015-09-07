using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Represents a documet class in latex (container for all other code)
    /// Contains all the data from \documentclass up unti \document
    /// </summary>
    public class DocumentClass : LaTeXElement
    {
        /// <summary>
        /// Creates a new intance of the DocumentClass without any data set
        /// </summary>
        public DocumentClass()
        {
            UsePackages = new List<string>();
        }

        /// <summary>
        /// Get/Set a list of the packages the document uses
        /// </summary>
        public List<string> UsePackages { get; set; }

        /// <summary>
        /// Get/Set the document of the document class
        /// </summary>
        public Document Document { get; set; }
    }
}