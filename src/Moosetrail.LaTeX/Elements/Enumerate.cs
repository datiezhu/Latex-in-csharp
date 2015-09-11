using System.Collections.Generic;
using System.Text;
using Moosetrail.LaTeX.CodeGenerator;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Class represent an enumerate list for LaTeX
    /// </summary>
    public class Enumerate : LaTeXElement
    {
        private const string BeginEnumerate = @"\begin{enumerate}";
        private const string EndEnumerate = @"\end{enumerate}";

        private StringBuilder _code;

        /// <summary>
        /// Creates a new instance of the Enumerate class
        /// </summary>
        public Enumerate()
        {
            ItemList = new List<Item>();
        }

        /// <summary>
        /// Get/Set the list of itmes in the list
        /// </summary>
        public List<Item> ItemList { get; }

        /// <summary>
        /// Adds a new text item to the enumerate
        /// </summary>
        /// <param name="item">The text to add as the item</param>
        public void AddItem(string item)
        {
            if (!TextFormatStrings.EmptyString(item))
            {
                var i = new Item();
                i.Elements.Add(new TextBody(TextFormatStrings.RemoveStartingFormating(item)));
                ItemList.Add(i);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var codeGenerator = new EnumerateCodeGenerator();
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