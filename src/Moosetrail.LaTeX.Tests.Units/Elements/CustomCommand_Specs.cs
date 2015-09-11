using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class CustomCommand_Specs
    {
        private CustomCommand SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new CustomCommand();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

         
    }
}