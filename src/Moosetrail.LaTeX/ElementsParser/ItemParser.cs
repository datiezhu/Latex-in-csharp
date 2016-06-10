using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementsParser
{
    /// <summary>
    /// Parser for items in lists
    /// </summary>
    public class ItemParser : LaTeXElementParser, LaTexElementParser<Item>
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTexElementParser<Item>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators = new List<string>
        {
            @"\\item",
            @"\\\\item"
        };

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public Item GetEmptyElement()
        {
            return new Item();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(Item element, params LaTeXElement[] children)
        {
            if (children.Any(DocumentOrganisation.ElementIsMainContentDevider))
                throw new ArgumentException("An Item can't have a DocumentClass, Document Chapter or other element that is concidered to organize information");
            else if (!children.Any())
                throw new ArgumentException("No child elements supplied to set as child");
            else if (children.Any(x => x is Item))
                throw new ArgumentException("Can't set Item as child for Item");

            var item = element;
            item.Elements.AddRange(children);
        }

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// The newly parsed object. The string builder will also have been updted, the code parsed is removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        public Item ParseCode(StringBuilder code)
        {
            if (!Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
                throw new ArgumentException("The code didn't start with an allowed indicator");

            var item = new Item();
            var match = CodeIndicators.SingleOrDefault(x => code.ToString().StartsWith(x));
            code.Remove(0, match?.Length ?? 5);

            return item;
        }

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        LaTeXElement LaTeXElementParser.GetEmptyElement()
        {
            return GetEmptyElement();
        }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        void LaTeXElementParser.SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            SetChildElement(element as Item, children);
        }

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// The newly parsed object. The string builder will also have been updted, the code parsed is removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        LaTeXElement LaTeXElementParser.ParseCode(StringBuilder code)
        {
            return ParseCode(code);
        }
    }
}