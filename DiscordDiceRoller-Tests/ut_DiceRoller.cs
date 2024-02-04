using DiscordDiceRoller;

namespace DiscordDiceRoller_Tests
{
    /// <summary>
    /// Some of these tests are statistical. 
    /// For those tests, Number of Attempts has been chosen to ensure failure rates will be less than one in a million.
    /// </summary>
    [TestClass]
    public class ut_DiceRoller
    {
        [TestMethod]
        public void ut_Roll_CanProduceAllResults()
        {
            int expectedMinimum = 1;
            int expectedMaximum = 6;

            HashSet<int> numbersGenerated = new HashSet<int>();

            for (int attempt = 0; attempt < 100; attempt++)
            {               
                int result = DiceRoller.Roll(expectedMaximum);
                numbersGenerated.Add(result);

                if (numbersGenerated.Count == expectedMaximum) break;
            }

            //We should have exactly $expectedMaximum results
            Assert.AreEqual(expectedMaximum, numbersGenerated.Count);           

            //If we passed the previous test AND have all expected outcomes, we also know we didn't get any unexpected outcomes
            for (int outcome = expectedMinimum; outcome <= expectedMaximum; outcome++)
            {
                Assert.IsTrue(numbersGenerated.Contains(outcome), $"Checking for {outcome}");
            }

            //This makes the test itself easier to debug/validate
            Console.WriteLine("Test produced all expected outputs: " + String.Join(", ", numbersGenerated.OrderBy(x => x)));
        }
    }
}