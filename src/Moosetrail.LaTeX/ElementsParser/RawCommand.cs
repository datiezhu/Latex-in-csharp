using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.ElementsParser
{
    internal class RawCommand
    {
        //^\\([a-z]*)\[([0-9a-z]*)\]{([a-z]*)}|^\\([a-z]*){([a-z]*)}
        public RawCommand(Match match)
        {
            FullCommand = match.Value;
            if (match.Groups[1].Success)
            {
                CommandType = match.Groups[1].Value;
            }
            else if (match.Groups[4].Success)
            {
                CommandType = match.Groups[4].Value;
            }
            else
            {
                CommandType = "";
            }

            if (match.Groups[3].Success)
            {
                RequriredArguments = match.Groups[3].Value;
            }
            else if (match.Groups[5].Success)
            {
                RequriredArguments = match.Groups[5].Value;
            }
            else
            {
                RequriredArguments = "";
            }

            OptionalArguments = match.Groups[2].Success ? match.Groups[2].Value : "";
        }

        public string FullCommand { get; set; }

        public string CommandType { get; private set; }

        public string RequriredArguments { get; private set; }

        public string OptionalArguments { get; private set; }
    }
}