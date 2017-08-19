using System.Collections.Generic;

namespace Moosetrail.LaTeX.Arguments
{
    public interface Argument
    {
        IEnumerable<string> AvailableForRequriredArguments { get; }

        IEnumerable<string> NotAvailableForRequriredArguments { get; }

        IEnumerable<string> NotAvailableTogeterWithOtherOptionals { get; }

        bool IsMatch(string argument);
    }
}