using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Environment_Specs
    {
        private Environment SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Environment(EnvironmentType.enumerate);
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

        #region Constrcutor

        [Test]
        public void constructor_should_set_elements_to_empty()
        {
            // When 
            SUT = new Environment(EnvironmentType.enumerate);

            // Then
            Assert.IsEmpty(SUT.InnerElements);
        }

        [Test]
        public void constructor_should_set_type_to_given_type()
        {
            // When
            SUT = new Environment(EnvironmentType.enumerate);

            // Then
            Assert.AreEqual(EnvironmentType.enumerate, SUT.Type);
        }

        #endregion Constrcutor
    }
}