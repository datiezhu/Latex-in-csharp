using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Chapter_Specs
    {
        private Chapter SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Chapter();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElement()
        {
            Assert.IsInstanceOf<LaTeXElement>(SUT);
        }

        #region Constrcutor Empty

        [Test]
        public void constructor_should_set_elements_to_empty()
        {
            // When 
            SUT = new Chapter();

            // Then
            Assert.IsEmpty(SUT.Elements);
        }

        #endregion Constrcutor Empty

    }
}