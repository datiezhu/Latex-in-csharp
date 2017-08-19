using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.Arguments
{
    public class PatternArgument : Argument
    {
        public PatternArgument(string pattern, IEnumerable<string> availableFor = null, 
            IEnumerable<string> notAvailableFor = null)
        {
            Pattern = pattern;
            NotAvailableForRequriredArguments = notAvailableFor ?? new List<string>();
            AvailableForRequriredArguments = availableFor ?? new List<string>();
            NotAvailableTogeterWithOtherOptionals =  new List<string>();
        }

        public string Pattern { get; }

        public IEnumerable<string> AvailableForRequriredArguments { get; }

        public IEnumerable<string> NotAvailableForRequriredArguments { get; }

        public bool IsMatch(string argument)
        {
            return Regex.IsMatch(argument, Pattern);
        }

        public IEnumerable<string> NotAvailableTogeterWithOtherOptionals { get; }
    }
}