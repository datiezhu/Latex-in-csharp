using System.Collections.Generic;
using Moosetrail.LaTeX.CodeGenerator;

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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var codeGenerator = new ItemCodeGenerator();
            return codeGenerator.Generate(this);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

    }
}