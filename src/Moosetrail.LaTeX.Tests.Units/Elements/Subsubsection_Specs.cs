using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Subsubsection_Specs
    {
        private Subsubsection SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Subsubsection();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElement()
        {
            Assert.IsInstanceOf<ContentContext>(SUT);
        }

        #region Constrcutor Empty

        [Test]
        public void constructor_should_set_elements_to_empty()
        {
            // When 
            SUT = new Subsubsection();

            // Then
            Assert.IsEmpty(SUT.Elements);
        }

        #endregion Constrcutor Empty

    }
}