using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementsParser
{
    /// <summary>
    /// Parser for text in an Latex element
    /// </summary>
    public class TextBodyParser : LaTeXElementParser
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public IEnumerable<string> CodeIndicators => new List<string>();

        /// <summary>
        /// Get indicator to see if the code starts with text or not
        /// </summary>
        /// <param name="code">The code to analyse</param>
        /// <returns></returns>
        public bool CodeStartsWithText(StringBuilder code)
        {
            return !doesntStartWithText(code);
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            throw new NotSupportedException();
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
        public LaTeXElement ParseCode(StringBuilder code)
        {
            if (doesntStartWithText(code))
                throw new ArgumentException("The code didn't start with text but an code object");

            var textMach = getTextMatch(code);
            code.Replace(textMach.Groups[1].Value, "");
            var str = new StringBuilder(textMach.Groups[1].Value);

            getMoreOfTheString(code, str);

            var text = new TextBody
            {
                TheText = str.ToString()
            };

            return text;
        }

        private static bool doesntStartWithText(StringBuilder code)
        {
            return Regex.IsMatch(code.ToString(), @"^\\");
        }

        private static Match getTextMatch(StringBuilder code)
        {
            var textMach = Regex.Match(code.ToString(), @"^(.*?|\s)\\");
            if (!textMach.Success)
                textMach = Regex.Match(code.ToString(), @"^(.*\s)");
            return textMach;
        }

        private static void getMoreOfTheString(StringBuilder code, StringBuilder str)
        {
            var hasModifedStr = getFullMath(code, str);

            if (stopedAtEscapedChar(code) && !hasModifedStr)
            {
                str.AppendFormat(@"\{0}", getNextTextPart(code));
                hasModifedStr = true;
            }
            

            if(hasModifedStr)
                getMoreOfTheString(code, str);
        }

        private static bool getFullMath(StringBuilder code, StringBuilder str)
        {
            var hasModifedStr = false;

            while (hasUnfinishedMath(str))
            {
                var nextPart = getNextTextPart(code);
                str.AppendFormat(@"\{0}", nextPart);
                hasModifedStr = true;
            }

            return hasModifedStr;
        }


        private static bool hasUnfinishedMath(StringBuilder str)
        {
            return (str.ToString().Count(x => x == '$') - Regex.Matches(str.ToString(), @"(\\\$)").Count) % 2 != 0;
        }

        private static string getNextTextPart(StringBuilder code)
        {
            code.Remove(0, 1);
            var text = getTextMatch(code);

            if (!String.IsNullOrWhiteSpace(text.Groups[1].Value)) { }
            code.Replace(text.Groups[1].Value, "");

            return text.Groups[1].Value;
        }

        private static bool stopedAtEscapedChar(StringBuilder code)
        {
            return Regex.IsMatch(code.ToString(), @"^\\\$");
        }
    }
}