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
    public class CommandParser : LaTeXElementParser, LaTexElementParser<Command>
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        public static IEnumerable<string> CodeIndicators { get; }
        private static readonly IEnumerable<string> BeginCommands;

        static CommandParser()
        {
            var beginCommands = new List<string>();
            foreach (var command in EnumUtil.GetValues<CommandType>())
            {
                beginCommands.Add(@"\\" + command);
                beginCommands.Add("\\\\" + command);
                beginCommands.Add(@"\\\\" + command);
            }
            BeginCommands = beginCommands;
            CodeIndicators = new List<string>(BeginCommands);
        }

        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> LaTexElementParser<Command>.CodeIndicators => CodeIndicators;

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. Returns the created object and the code after the element was ended
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// A Tuple with the newly parsed object. The second object is a string with the parsed code removed from the code given as an argument to the function
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        public Tuple<Command, string> ParseCode(string code)
        {
            var commandMatch = Regex.Match(code, @"^\\([a-z]*)\[([0-9a-z,]*)\]{([a-z]*)}|^\\([a-z]*){([a-z]*)}");
            var foundCommand = new RawCommand(commandMatch);

            var couldFindType = Enum.TryParse(foundCommand.CommandType, out CommandType commandType);

            if (!couldFindType)
                throw new LaTeXParseCommandException(commandMatch.Value, code);

            RuleBook.TryGetValue(commandType, out CommandRules.CommandRules rules);

            var command = new Command(
                commandType, 
                getRequriredArguments(rules, foundCommand, code),
                getOptionalArguments(rules, foundCommand, code));

            return Tuple.Create(command, code);
        }

        private static readonly Dictionary<CommandType, CommandRules.CommandRules> RuleBook = new Dictionary<CommandType, CommandRules.CommandRules>
        {
            {CommandType.documentclass, new DocumentclassRules()}
        };

        private static IEnumerable<string> getRequriredArguments(CommandRules.CommandRules rules, RawCommand rawCommand, string code)
        {
            if(rules.AllowedRequiredArguments.Contains(rawCommand.RequriredArguments))
                return new List<string> { rawCommand.RequriredArguments };
            else 
                throw new LaTeXParseCommandException(rawCommand.FullCommand, code,
                    $"The requrired argument '{rawCommand.RequriredArguments}' isn't a known argument for the command '{rawCommand.CommandType}'");
        }

        private static IEnumerable<string> getOptionalArguments(CommandRules.CommandRules rules, RawCommand rawCommand, string code)
        {
            var arguments = new List<string>();
            foreach (var rawArgument in getAllOptionalArguments(rawCommand))
            {
                var argumentRules = getArgumentRules(rules, rawArgument);

                if (argumentCantExistWithRequriredArguments(rawCommand, argumentRules))
                {
                    handleInvalidOptionalForRequriredArgument(rawCommand, rawArgument);
                }
                if (argumentCantCoexistWithOtherOptional(argumentRules, arguments))
                {
                    handleInvalidOptionalArgumentPair(rawCommand, argumentRules, arguments, rawArgument);
                }

                arguments.Add(rawArgument);
            }

            return arguments;
        }

        private static string[] getAllOptionalArguments(RawCommand rawCommand)
        {
            return rawCommand.OptionalArguments
                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static Argument getArgumentRules(CommandRules.CommandRules rules, string rawArgument)
        {
            var matchingArgumentRules = rules.OptionalArguments.FirstOrDefault(rule => rule.IsMatch(rawArgument));
            return matchingArgumentRules;
        }

        private static bool argumentCantExistWithRequriredArguments(RawCommand rawCommand, Argument matchingArgumentRules)
        {
            return matchingArgumentRules.NotAvailableForRequriredArguments.Any() &&
                   matchingArgumentRules.NotAvailableForRequriredArguments.Contains(rawCommand.RequriredArguments) || matchingArgumentRules.AvailableForRequriredArguments.Any() && 
                   !matchingArgumentRules.AvailableForRequriredArguments.Contains(rawCommand.RequriredArguments);
        }

        private static void handleInvalidOptionalForRequriredArgument(RawCommand rawCommand, string rawArgument)
        {
            throw new InvalidLatexCodeException(
                $"The command '{rawCommand.CommandType}' with the requrired argument/s '{rawCommand.RequriredArguments}' can't have the optional argument of '{rawArgument}'");
        }

        private static bool argumentCantCoexistWithOtherOptional(Argument matchingArgumentRules, List<string> arguments)
        {
            return matchingArgumentRules.NotAvailableTogeterWithOtherOptionals.Any(x => arguments.Contains(x));
        }

        private static void handleInvalidOptionalArgumentPair(RawCommand rawCommand, Argument matchingArgumentRules,
            ICollection<string> arguments, string rawArgument)
        {
            var invalidOptional =
                matchingArgumentRules.NotAvailableTogeterWithOtherOptionals.First(arguments.Contains);
            throw new InvalidLatexCodeException(
                $"The command '{rawCommand.CommandType}' can't have the both the optional arguments of '{invalidOptional}' and '{rawArgument}'");
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
        public Command GetEmptyElement()
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
        public void SetChildElement(Command element, params LaTeXElement[] children)
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
        public Command ParseCode(StringBuilder code)
        {
            throw new System.NotSupportedException();
        }

        #endregion Depricated or to be depricated
    }
}