using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Command_Specs
    {
        private Command SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Command(CommandType.item);
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
            SUT = new Command(CommandType.item);

            // Then
            Assert.IsEmpty(SUT.InnerElements);
        }

        [Test]
        public void constructor_should_set_type_to_given_type()
        {
            // When
            SUT = new Command(CommandType.item);

            // Then
            Assert.AreEqual(CommandType.item, SUT.Type);
        }

        #endregion Constrcutor
    }
}