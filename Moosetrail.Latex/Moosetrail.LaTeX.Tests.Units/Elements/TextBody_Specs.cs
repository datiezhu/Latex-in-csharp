using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class TextBody_Specs
    {
        private TextBody SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new TextBody();
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

        #region Constructor

        [Test]
        public void constructor_should_set_the_text_given()
        {
            // When 
            SUT = new TextBody("My text");

            // Then
            Assert.AreEqual("My text", SUT.TheText);
        }

        #endregion Constructor

        #region TheText

        [Test]
        public void theText_should_be_able_toset()
        {
            // When 
            SUT.TheText = "My text";

            // Then
            Assert.AreEqual("My text", SUT.TheText);
        }
        [Test]
        public void theText_should_trim_the_text_to_set()
        {
            // When 
            SUT.TheText = " My text ";

            // Then
            Assert.AreEqual("My text", SUT.TheText);
        }

        #endregion TheText

        #region Equals

        [Test]
        public void equals_should_return_true_if_same_text()
        {
            // Given 
            var other = setSutAndOtherToSame();

            // Then
            Assert.AreEqual(SUT, other);
        }

        [Test]
        public void equals_should_return_false_for_different_text()
        {
            // Given 
            var other = setSutAndOtherToSame();
            other.TheText = "My awesome text";

            // Then
            Assert.AreNotEqual(SUT, other);
        }

        #endregion Equals

        #region ToString

        [Test]
        public void toString_should_return_the_string()
        {
            // Given 
            SUT.TheText = "My text";

            // When
            var result = SUT.ToString();

            // Then
            Assert.AreEqual("My text", result);
        }

        #endregion ToString

        #region TestHelpers

        private TextBody setSutAndOtherToSame()
        {
            SUT = new TextBody("My text");
            var other = new TextBody("My text");
            return other;
        }


        #endregion TestHelpers
    }
} 