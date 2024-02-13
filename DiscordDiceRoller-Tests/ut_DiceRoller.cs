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

        [DataTestMethod]
        [DataRow(3, 6)]
        [DataRow(5, 1)]
        [DataRow(2, 20)]
        public void ut_Roll_Multiple_CanProduceAllResults(int dice, int sides)
        {
            int expectedMinimum = dice;
            int expectedMaximum = dice * sides;
            int expectedOutcomes = expectedMaximum - expectedMinimum + 1;

            //Calculate how many attempts we need, with a minimum of 1K
            int numberOfPermutations = (int)Math.Pow(sides, dice);
            int numberOfAttempts = (int)Math.Pow(numberOfPermutations, 2);
            numberOfAttempts = Math.Max(numberOfAttempts, 1000);

            //Generate numbers!
            HashSet<int> numbersGenerated = new HashSet<int>();
            for (int attempt = 0; attempt < numberOfAttempts; attempt++)
            {
                int result = DiceRoller.Roll(dice, sides);
                numbersGenerated.Add(result);

                if (numbersGenerated.Count == expectedOutcomes) break;
            }

            //We should have exactly the expected number of results
            Assert.AreEqual(expectedOutcomes, numbersGenerated.Count);

            //If we passed the previous test AND have all expected outcomes, we also know we didn't get any unexpected outcomes
            for (int outcome = expectedMinimum; outcome <= expectedMaximum; outcome++)
            {
                Assert.IsTrue(numbersGenerated.Contains(outcome), $"Checking for {outcome}");
            }

            //This makes the test itself easier to debug/validate
            Console.WriteLine("Test produced all expected outputs: " + String.Join(", ", numbersGenerated.OrderBy(x => x)));
        }

        [DataTestMethod]
        [DataRow(3, 6)]
        [DataRow(5, 1)]
        [DataRow(2, 20)]
        public void ut_Roll_Multiple_ApproximatelyAverage(int dice, int sides)
        {
            int expectedMinimum = dice;
            int expectedMaximum = dice * sides;
            int expectedOutcomes = expectedMaximum - expectedMinimum + 1;

            //Calculate the expected average
            double expectedAveragePerDie = ((double)sides + 1) / 2;
            double expectedAverage = expectedAveragePerDie * dice;

            //Calculate how many attempts we need, with a minimum of 1K
            int numberOfPermutations = (int)Math.Pow(sides, dice);
            int numberOfAttempts = (int)Math.Pow(numberOfPermutations, 2);
            numberOfAttempts = Math.Max(numberOfAttempts, 1000);

            //Generate numbers!
            int sum = 0;
            for (int attempt = 0; attempt < numberOfAttempts; attempt++)
            {
                int result = DiceRoller.Roll(dice, sides);
                sum += result;
            }

            //Actual average
            double average = (double)sum / numberOfAttempts;

            //Variance
            double variation = Math.Abs(expectedAverage - average);
            double allowableVariation = 1.0; //TODO: We can get more sophisticated here
            Assert.IsTrue(variation <= allowableVariation);
        }
    }
}