using Moosetrail.LaTeX.CommandRules;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.CommandRules
{
    [TestFixture]
    public class DocumentclassRules_Specs
    {
        private DocumentclassRules SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new DocumentclassRules();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        #region NbrOfRequriredArguments

        // References: Unofficial Reference LaTeX2e p. 8

        [Test]
        public void nbrOfRequriredOptions_should_return_1()
        {
            // Then
            Assert.AreEqual(1, SUT.NbrOfRequriredArguments);
        }

        #endregion NbrOfRequriredArguments

        #region AllowedRequiredArguments

        private static readonly string[] StandardDocumentclcasses = {"article", "book", "letter", "report", "slides" };

        [Test]
        public void allowedRequriredArguments_should_contain_standardDocumentClasses_only()
        {
            // Then
            CollectionAssert.AreEquivalent(StandardDocumentclcasses, SUT.AllowedRequiredArguments);
        }

        #endregion AllowedRequiredArguments
    }
}