using Moosetrail.LaTeX.CodeGenerator;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.CodeGenerator
{
    [TestFixture]
    public class EnumerateCodeGenerator_Specs
    {
        private EnumerateCodeGenerator SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new EnumerateCodeGenerator();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_CodeGenerator_Enumerate()
        {
            Assert.IsInstanceOf<CodeGenerator<Enumerate>>(SUT);
        }

        #region Generate

        [Test]
        public void generate_should_return_code_with_item()
        {
            // Given 
            var enumerate = new Enumerate();
            enumerate.AddItem("My item");

            // When 
            var result = SUT.Generate(enumerate);

            // Then
            Assert.AreEqual(@"\begin{enumerate}" + "\n\t" + @"\item My item" + "\n" + @" \end{enumerate}" + "\n", result);
        }

        [Test]
        public void generate_should_return_code_with_several_items()
        {
            // Given 
            var enumerate = new Enumerate();
            enumerate.AddItem("My item");
            enumerate.AddItem("My second item");

            // When 
            var result = SUT.Generate(enumerate);

            // Then
            Assert.AreEqual(@"\begin{enumerate}" + "\n\t" + 
                @"\item My item" + "\n \t" + 
                @"\item My second item" + "\n" + 
                @" \end{enumerate}" + "\n", result);
        }

        #endregion Generate
    }
}