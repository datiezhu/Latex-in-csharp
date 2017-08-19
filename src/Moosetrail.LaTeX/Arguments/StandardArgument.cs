using System.Collections.Generic;

namespace Moosetrail.LaTeX.Arguments
{
    public class StandardArgument : Argument
    {
        public StandardArgument(string argument, IEnumerable<string> availableFor = null,
           IEnumerable<string> notAvailableFor = null, IEnumerable<string> notAvailableWhen = null)
        {
            Argument = argument;
            NotAvailableForRequriredArguments = notAvailableFor ?? new List<string>();
            AvailableForRequriredArguments = availableFor ?? new List<string>();
            NotAvailableTogeterWithOtherOptionals = notAvailableWhen ?? new List<string>();
        }

        public string Argument { get; set; }
        public IEnumerable<string> AvailableForRequriredArguments { get; }
        public IEnumerable<string> NotAvailableForRequriredArguments { get; }
        public IEnumerable<string> NotAvailableTogeterWithOtherOptionals { get; }

        public bool IsMatch(string argument)
        {
            return Argument == argument; 
        }
    }
}