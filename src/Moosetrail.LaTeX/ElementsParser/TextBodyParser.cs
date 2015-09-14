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
    public class TextBodyParser : LaTeXElementParser, LaTexElementParser<TextBody>
    {
        private const string EscapedDollar = @"(\\\$)";
        private const string NewLineMathStart = @"\\\[";
        private const string NewLineMathEnd = @"\\\]";
        private const string InLineMathStart = @"\\\(";
        private const string InLineMathEnd = @"\\\)";

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators => new List<string>
        {
            @"\\emph{",
            @"\\\(",
            @"\\\["
        };

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTexElementParser<TextBody>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public TextBody GetEmptyElement()
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

        private static bool isTextObject(StringBuilder code)
        {
            var doesNotStartWithBackslash = !Regex.IsMatch(code.ToString(), @"^\\");
            var startsWithAllowedCode = Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators));
            var isTextObject = doesNotStartWithBackslash || startsWithAllowedCode;
            return isTextObject;
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(TextBody element, params LaTeXElement[] children)
        {
            throw new NotSupportedException("TextBody can't have childre");
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
        public TextBody ParseCode(StringBuilder code)
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


      

        private static string getStartTextPoint(StringBuilder code)
        {
            var textMach = Regex.Match(code.ToString(), @"^\\emph\{(.*?)\}");
            if (textMach.Success)
                return getStringWithEmpf(code, textMach);
            else if (Regex.IsMatch(code.ToString(), CodeParser.CreateCodeStartPattern(CodeIndicators)))
            {
                return $@"\{getNextTextPart(code)}";
            }
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
                textMach = Regex.Match(code.ToString(), @"^(.*|\s)");
            return textMach;
        }

        private static void completeText(StringBuilder code, StringBuilder str)
        {
            while (true)
            {
                var hasModifedStr = false;

                if (hasUnfinishedMath(str) || nextIsMath(code))
                    hasModifedStr = getFullMath(code, str);
                else if (stopedAtEscapedSequenc(code))
                    hasModifedStr = getPassedEscapedChar(code, str);

                if (hasModifedStr)
                    continue;
                break;
            }
        }

        private static bool hasUnfinishedMath(StringBuilder str)
        {
            return hasUnbalcedDollarMath(str) || hasUnblanacedNewLineMath(str) || hasUnblanacedInLineMath(str);
        }

        private static bool hasUnbalcedDollarMath(StringBuilder str)
        {
            return (str.ToString().Count(x => x == '$') - Regex.Matches(str.ToString(), EscapedDollar).Count) % 2 != 0;
        }

        private static bool hasUnblanacedNewLineMath(StringBuilder str)
        {
            return Regex.Matches(str.ToString(), NewLineMathStart).Count != Regex.Matches(str.ToString(), NewLineMathEnd).Count;
        }

        private static bool hasUnblanacedInLineMath(StringBuilder str)
        {
            return Regex.Matches(str.ToString(), InLineMathStart).Count != Regex.Matches(str.ToString(), InLineMathEnd).Count;
        }

        private static bool nextIsMath(StringBuilder code)
        {
            return Regex.IsMatch(code.ToString(), @"^" + NewLineMathStart) || Regex.IsMatch(code.ToString(), @"^" + InLineMathStart);
        }

        private static bool getFullMath(StringBuilder code, StringBuilder str)
        {
            var nextPart = getNextTextPart(code);
            if (!String.IsNullOrWhiteSpace(nextPart))
                str.AppendFormat(@"\{0}", nextPart);
            else if (String.IsNullOrEmpty(nextPart))
            {
                str.Append(@"\\");
                nextPart = getNextTextPart(code);
                str.AppendFormat(@"{0}", nextPart);
            }
            else
            {
                str.Append(" ");
            }
                
            return true;
        }

        private static string getNextTextPart(StringBuilder code)
        {
            code.Remove(0, 1);
            var text = getStartTextPoint(code);

            if (!string.IsNullOrWhiteSpace(text))
                code.Replace(text, "", 0, text.Length);

            var s = code.ToString().TrimStart();
            code.Clear();
            code.Append(s);

            return text;
        }

        private static bool stopedAtEscapedSequenc(StringBuilder code)
        {
            return Regex.IsMatch(code.ToString(), @"^" + EscapedDollar) || Regex.IsMatch(code.ToString(), @"^\\emph{");
        }

        private static bool getPassedEscapedChar(StringBuilder code, StringBuilder str)
        {
            str.AppendFormat(@"\{0}", getNextTextPart(code));
            return true;
        }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTeXElementParser.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        LaTeXElement LaTeXElementParser.GetEmptyElement()
        {
            return GetEmptyElement();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        void LaTeXElementParser.SetChildElement(LaTeXElement element, params LaTeXElement[] children)
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
        LaTeXElement LaTeXElementParser.ParseCode(StringBuilder code)
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

    }
}