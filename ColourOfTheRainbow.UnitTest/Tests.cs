using NUnit.Framework;

namespace ColourOfTheRainbow.UnitTest
{
    public class Tests
    {
        [Test]
        public void CheckInputTest_SetInput()
        {
            // ARRANGE
            ColourService colourService = new ColourService();
            string[] args = new string[] { "Red" };

            // ACT
            colourService.CheckInput(args);

            // ASSERT
            Assert.AreEqual(args[0], colourService.Input);
        }

        public void CheckInputTest_NumericalInputsDoesNotSetInput()
        {
            // ARRANGE
            ColourService colourService = new ColourService();
            string[] args = new string[] { "Red4" };

            // ACT
            colourService.CheckInput(args);

            // ASSERT
            Assert.AreEqual(null, colourService.Input);
        }

        [Test]
        public void ProvideColourCodeTest()
        {
            // ARRANGE

            // ACT

            // ASSERT
            Assert.Pass();
        }

        [Test]
        public void CreateColourListFromConfigTest()
        {
            // ARRANGE

            // ACT

            // ASSERT
            Assert.Pass();
        }
    }
}