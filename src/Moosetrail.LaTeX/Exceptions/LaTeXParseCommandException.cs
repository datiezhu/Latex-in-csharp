namespace Moosetrail.LaTeX.Exceptions
{
    public class LaTeXParseCommandException : LaTeXException
    {
        public LaTeXParseCommandException() { }

        public LaTeXParseCommandException(string failingCodePart, string code, string message = null)
            :base(message)
        {
            FailingCode = failingCodePart;
            InArea = code.Substring(0, 100) + "...";
        }

        public string FailingCode { get; private set; }

        public string InArea { get; private set; }
    }
}