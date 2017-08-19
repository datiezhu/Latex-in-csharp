using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moosetrail.LaTeX.Arguments;
using Moosetrail.LaTeX.CommandRules;
using Moosetrail.LaTeX.Elements;
using Moosetrail.LaTeX.Exceptions;
using Moosetrail.LaTeX.Helpers;

namespace Moosetrail.LaTeX.ElementsParser
{
    public class ArgumentCommandParser : LaTeXElementParser, LaTexElementParser<ArgumentCommand>
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators { get; }

        private static readonly IEnumerable<string> BeginCommands = new[]
        {
            @"\\",
            "\\\\",
            @"\\\\"
        };

        static ArgumentCommandParser()
        {
            var codeIndicators = new List<string>();
            foreach (var command in EnumUtil.GetValues<ArgumentCommandType>())
            {
                codeIndicators.AddRange(BeginCommands.Select(beginCommand => beginCommand + command));
            }
            CodeIndicators = codeIndicators;
        }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTexElementParser<ArgumentCommand>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. Returns the created object and the code after the element was ended
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// A Tuple with the newly parsed object. The second object is a string with the parsed code removed from the code given as an argument to the function
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        public Tuple<ArgumentCommand, string> ParseCode(string code)
        {
            var commandMatch = Regex.Match(code, createPattern());
            var foundCommand = new RawArgumentCommand(commandMatch);

            var couldFindType = Enum.TryParse(foundCommand.CommandType, out ArgumentCommandType commandType);

            if (!couldFindType)
                throw new LaTeXParseCommandException(commandMatch.Value, code, $"Didn't recognize the command {foundCommand.CommandType}");

            RuleBook.TryGetValue(commandType, out CommandRules.ArgumentCommandRules rules);

            var command = new ArgumentCommand(
                commandType, 
                getRequriredArguments(rules, foundCommand, code),
                getOptionalArguments(rules, foundCommand, code));

            var updatedCode = code.Remove(0, foundCommand.FullCommand.Length);

            return Tuple.Create(command, updatedCode);
        }

        private string createPattern()
        {
            var pattern = new StringBuilder();

            foreach (var beginCommand in BeginCommands)
            {
                pattern.Append(@"^" + beginCommand + @"([a-zA-Z]*)\[([0-9a-zA-Z,]*)\]{([a-zA-Z]*)}");
                pattern.Append("|");
                pattern.Append(@"^" + beginCommand + "([a-zA-Z]*){([a-zA-Z]*)}");
                pattern.Append("|");
            }
            pattern.Remove(pattern.Length - 1, 1);
            return pattern.ToString();
        }

        private static readonly Dictionary<ArgumentCommandType, CommandRules.ArgumentCommandRules> RuleBook = new Dictionary<ArgumentCommandType, CommandRules.ArgumentCommandRules>
        {
            {ArgumentCommandType.documentclass, new DocumentclassRules()},
            {ArgumentCommandType.usepackage, new UsepackageRules() }
        };

        private static IEnumerable<string> getRequriredArguments(CommandRules.ArgumentCommandRules rules, RawArgumentCommand rawArgumentCommand, string code)
        {
            if(!rules.AllowedRequiredArguments.Any() || rules.AllowedRequiredArguments.Contains(rawArgumentCommand.RequriredArguments))
                return new List<string> { rawArgumentCommand.RequriredArguments };
            else 
                throw new LaTeXParseCommandException(rawArgumentCommand.FullCommand, code,
                    $"The requrired argument '{rawArgumentCommand.RequriredArguments}' isn't a known argument for the command '{rawArgumentCommand.CommandType}'");
        }

        private static IEnumerable<string> getOptionalArguments(CommandRules.ArgumentCommandRules rules, RawArgumentCommand rawArgumentCommand, string code)
        {
            var arguments = new List<string>();
            foreach (var rawArgument in getAllOptionalArguments(rawArgumentCommand))
            {
                if (rules.OptionalArguments.Any())
                {
                    var argumentRules = getArgumentRules(rules, rawArgument);

                    if (argumentRules == null)
                    {
                        throw new LaTeXParseCommandException(rawArgumentCommand.FullCommand, code,
                   $"The optional argument '{rawArgument}' isn't a known argument for the command '{rawArgumentCommand.CommandType}'");
                    }
                    if (argumentCantExistWithRequriredArguments(rawArgumentCommand, argumentRules))
                    {
                        handleInvalidOptionalForRequriredArgument(rawArgumentCommand, rawArgument);
                    }
                    if (argumentCantCoexistWithOtherOptional(argumentRules, arguments))
                    {
                        handleInvalidOptionalArgumentPair(rawArgumentCommand, argumentRules, arguments, rawArgument);
                    }
                }

                arguments.Add(rawArgument);
            }

            return arguments;
        }

        private static string[] getAllOptionalArguments(RawArgumentCommand rawArgumentCommand)
        {
            return rawArgumentCommand.OptionalArguments
                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static Argument getArgumentRules(CommandRules.ArgumentCommandRules rules, string rawArgument)
        {
            var matchingArgumentRules = rules.OptionalArguments.FirstOrDefault(rule => rule.IsMatch(rawArgument));
            return matchingArgumentRules;
        }

        private static bool argumentCantExistWithRequriredArguments(RawArgumentCommand rawArgumentCommand, Argument matchingArgumentRules)
        {
            return matchingArgumentRules.NotAvailableForRequriredArguments.Any() &&
                   matchingArgumentRules.NotAvailableForRequriredArguments.Contains(rawArgumentCommand.RequriredArguments) || matchingArgumentRules.AvailableForRequriredArguments.Any() && 
                   !matchingArgumentRules.AvailableForRequriredArguments.Contains(rawArgumentCommand.RequriredArguments);
        }

        private static void handleInvalidOptionalForRequriredArgument(RawArgumentCommand rawArgumentCommand, string rawArgument)
        {
            throw new InvalidLatexCodeException(
                $"The command '{rawArgumentCommand.CommandType}' with the requrired argument/s '{rawArgumentCommand.RequriredArguments}' can't have the optional argument of '{rawArgument}'");
        }

        private static bool argumentCantCoexistWithOtherOptional(Argument matchingArgumentRules, List<string> arguments)
        {
            return matchingArgumentRules.NotAvailableTogeterWithOtherOptionals.Any(x => arguments.Contains(x));
        }

        private static void handleInvalidOptionalArgumentPair(RawArgumentCommand rawArgumentCommand, Argument matchingArgumentRules,
            ICollection<string> arguments, string rawArgument)
        {
            var invalidOptional =
                matchingArgumentRules.NotAvailableTogeterWithOtherOptionals.First(arguments.Contains);
            throw new InvalidLatexCodeException(
                $"The command '{rawArgumentCommand.CommandType}' can't have the both the optional arguments of '{invalidOptional}' and '{rawArgument}'");
        }

        #region Depricated or to be depricated

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(LaTeXElement element, params LaTeXElement[] children)
        {
            throw new System.NotImplementedException();
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
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        public ArgumentCommand GetEmptyElement()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        public void SetChildElement(ArgumentCommand element, params LaTeXElement[] children)
        {
            throw new NotImplementedException();
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
        public ArgumentCommand ParseCode(StringBuilder code)
        {
            throw new System.NotSupportedException();
        }

        #endregion Depricated or to be depricated
    }
}