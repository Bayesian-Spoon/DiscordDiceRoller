using DiscordDiceRoller;
using DiscordDiceRoller.DiceParsers;

namespace DiscordDiceRoller_Tests
{
    /// <summary>
    /// Some of these tests are statistical. 
    /// For those tests, Number of Attempts has been chosen to ensure failure rates will be less than one in a million.
    /// </summary>
    [TestClass]
    public class ut_DiceRollerStatistics
    {
        [TestMethod]
        public void ut_RollStats_FateDieBalanced()
        {
            List<int> diceResults = new List<int>();

            int target = 1000;
            for (int i = 0; i < target * 4; i++)
            {
                diceResults.Add(FateDiceParser.RollFateDie());
            }

            int negativeCount = diceResults.Count(x => x == -1);
            int zeroCount = diceResults.Count(x => x == 0);
            int positiveCount = diceResults.Count(x => x == 1);

            Assert.IsTrue(negativeCount >= target);
            Assert.IsTrue(zeroCount >= target);
            Assert.IsTrue(positiveCount >= target);

            Console.WriteLine($"Negative: {negativeCount}");
            Console.WriteLine($"Zero: {zeroCount}");
            Console.WriteLine($"Positive: {positiveCount}");
        }

        [TestMethod]
        public void ut_RollStats_FateDiceBalanced()
        {
            List<int?> diceResults = new List<int?>();

            int target = 1000;
            int possibleOutcomes = 81;
            double marginOfError = 1.1;
            for (int i = 0; i < target * possibleOutcomes * marginOfError; i++)
            {
                diceResults.Add(FateDiceParser.Parse("", "").NumericOutcome);
            }

            Assert.IsTrue(diceResults.Count(x => x == -4) >= target * 1);
            Assert.IsTrue(diceResults.Count(x => x == -3) >= target * 4);
            Assert.IsTrue(diceResults.Count(x => x == -2) >= target * 10);
            Assert.IsTrue(diceResults.Count(x => x == -1) >= target * 16);
            Assert.IsTrue(diceResults.Count(x => x == 0) >= target * 19);
            Assert.IsTrue(diceResults.Count(x => x == 1) >= target * 16);
            Assert.IsTrue(diceResults.Count(x => x == 2) >= target * 10);
            Assert.IsTrue(diceResults.Count(x => x == 3) >= target * 4);
            Assert.IsTrue(diceResults.Count(x => x == 4) >= target * 1);

            for (int index = -4; index <= 4; index++)
            {
                Console.WriteLine($"{index} = {diceResults.Count(x => x == index)}");
            }
        }
    }
}