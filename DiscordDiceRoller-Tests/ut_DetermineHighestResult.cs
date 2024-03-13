using DiscordDiceRoller;

namespace DiscordDiceRoller_Tests
{
    [TestClass]
    public class ut_DetermineHighestResult
    {
        [TestMethod]
        public void ut_DHR_HighBeatsLow()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 6 });
            categories.Add("Category 2", new List<int> { 5 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 1", result);
        }

        [TestMethod]
        public void ut_DHR_ConsiderMultipleDice()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 6, 6, 6 });
            categories.Add("Category 2", new List<int> { 6, 6, 5 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 1", result);
        }

        [TestMethod]
        public void ut_DHR_ConsiderUnsortedDice()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 4, 5, 6 });
            categories.Add("Category 2", new List<int> { 6, 3, 5 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 1", result);
        }

        [TestMethod]
        public void ut_DHR_ResolveDifferentLengths()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 5 });
            categories.Add("Category 2", new List<int> { 5, 1 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 2", result);
        }

        [TestMethod]
        public void ut_DHR_ResolveUnsortedDifferentLengths()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 5 });
            categories.Add("Category 2", new List<int> { 1, 5 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 2", result);
        }

        [TestMethod]
        public void ut_DHR_ResolveFullTies()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 5, 5, 5 });
            categories.Add("Category 2", new List<int> { 5, 5, 5 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 1", result);
        }

        [TestMethod]
        public void ut_DHR_NumerousCategories()
        {
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            categories.Add("Category 1", new List<int> { 6, 6, 6, 6 });
            categories.Add("Category 2", new List<int> { 6, 6, 6, 5 });
            categories.Add("Category 3", new List<int> { 6, 6, 5 });
            categories.Add("Category 4", new List<int> { 6, 5 });
            categories.Add("Category 5", new List<int> { 5 });

            string result = DiceRoller.DetermineHighestResult(categories);
            Assert.AreEqual("Category 1", result);
        }
    }
}
