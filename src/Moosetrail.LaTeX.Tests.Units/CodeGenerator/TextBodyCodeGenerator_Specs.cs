using Moosetrail.LaTeX.CodeGenerator;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.CodeGenerator
{
    [TestFixture]
    public class TextBodyCodeGenerator_Specs
    {
        private TextBodyCodeGenerator SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new TextBodyCodeGenerator();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_CodeGenerator_TextBody()
        {
            Assert.IsInstanceOf<CodeGenerator<TextBody>>(SUT);
        }

        #region Generate

        [Test]
        public void generate_should_return_the_text()
        {
            // Given 
            var text = new TextBody("My awesome \n text");

            // When 
            var result = SUT.Generate(text);

            // Then
            Assert.AreEqual("My awesome \n text", result);
        }

        #endregion Generate
    }
}