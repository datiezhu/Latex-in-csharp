using System.Collections.Generic;
using System.Text;

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
        /// Converts the object into latex code
        /// </summary>
        /// <returns>A string with latex code that represents this object in latex format</returns>
        public string ToCode()
        {
            _code = new StringBuilder(BeginEnumerate);
            foreach (var element in ItemList)
            {
                _code.AppendFormat(@"\item {0} ", ((Item)element).Elements[0]);
            }
            _code.Append(EndEnumerate);

            return _code.ToString();
        }

    }
}