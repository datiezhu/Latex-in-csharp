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
        private static string escapedDollar = @"(\\\$)";
        private static string newLineMathStart = @"\\\[";
        private static string newLineMathEnd = @"\\\]";

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators => new List<string>
        {
            @"\\emph{"
        };

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public LaTeXElement GetEmptyElement()
        {
            return new TextBody();
        }

        /// <summary>
        /// Get indicator to see if the code starts with text or not
        /// </summary>
        /// <param name="code">The code to analyse</param>
        /// <returns></returns>
        public bool CodeStartsWithText(StringBuilder code)
        {
            return isTextObject(code);
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
            if (!isTextObject(code))
                throw new ArgumentException("The code didn't start with text but an code object");

            var text = getStartTextPoint(code);
            code.Replace(text, "", 0, text.Length);
            var str = new StringBuilder(text);

            completeText(code, str);

            return new TextBody
            {
                TheText = str.ToString()
            };
        }

        private static bool isTextObject(StringBuilder code)
        {
            var doesNotStartWithBackslash = !Regex.IsMatch(code.ToString(), @"^\\");
            var startsWithAllowedCode = Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators));
            var isTextObject = doesNotStartWithBackslash || startsWithAllowedCode;
            return isTextObject;
        }

        private static string getStartTextPoint(StringBuilder code)
        {
            var textMach = Regex.Match(code.ToString(), @"^\\emph\{(.*?)\}");
            if (textMach.Success)
                return getStringWithEmpf(code, textMach);
            else
                textMach = serachForTextString(code);

            return textMach.Groups[1].Value;
        }

        private static string getStringWithEmpf(StringBuilder code, Match textMach)
        {
            var continuesString = code.ToString().Replace(textMach.Value, "");
            var nextPartMatch = serachForTextString(new StringBuilder(continuesString));
            return textMach.Value + nextPartMatch.Groups[1].Value;
        }

        private static Match serachForTextString(StringBuilder code)
        {
            var textMach = Regex.Match(code.ToString(), @"^(.*?|\s)\\");
            if (!textMach.Success)
                textMach = Regex.Match(code.ToString(), @"^(.*\s)");
            return textMach;
        }

        private static void completeText(StringBuilder code, StringBuilder str)
        {
            var hasModifedStr = false;

            if (hasUnfinishedMath(str) || nextIsMath(code))
                hasModifedStr = getFullMath(code, str);
            else if (stopedAtEscapedSequenc(code))
                hasModifedStr = getPassedEscapedChar(code, str);

            if(hasModifedStr)
                completeText(code, str);
        }

        private static bool hasUnfinishedMath(StringBuilder str)
        {
            return hasUnbalcedDollarMath(str) || hasUnblanacedNewLineMath(str);
        }

        private static bool hasUnbalcedDollarMath(StringBuilder str)
        {
            return (str.ToString().Count(x => x == '$') - Regex.Matches(str.ToString(), escapedDollar).Count) % 2 != 0;
        }

        private static bool hasUnblanacedNewLineMath(StringBuilder str)
        {
            return Regex.Matches(str.ToString(), newLineMathStart).Count != Regex.Matches(str.ToString(), newLineMathEnd).Count;
        }

        private static bool nextIsMath(StringBuilder code)
        {
            return Regex.IsMatch(code.ToString(), @"^" + newLineMathStart);
        }

        private static bool getFullMath(StringBuilder code, StringBuilder str)
        {
            var nextPart = getNextTextPart(code);
            str.AppendFormat(@"\{0}", nextPart);

            return true;
        }

        private static string getNextTextPart(StringBuilder code)
        {
            code.Remove(0, 1);
            var text = getStartTextPoint(code);

            if (!string.IsNullOrWhiteSpace(text))
                code.Replace(text, "", 0, text.Length);

            return text;
        }

        private static bool stopedAtEscapedSequenc(StringBuilder code)
        {
            return Regex.IsMatch(code.ToString(), @"^" + escapedDollar) || Regex.IsMatch(code.ToString(), @"^\\emph{");
        }

        private static bool getPassedEscapedChar(StringBuilder code, StringBuilder str)
        {
            str.AppendFormat(@"\{0}", getNextTextPart(code));
            return true;
        }

    }
}