using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    public class DiceRoller
    {
        public class RollResult
        {
            public string? DiceRolled;
            public List<int> IndividualRolls = new List<int>();
            public string? DetailedOutcome;
            public int? NumericOutcome;
        }

        /// <summary>
        /// Rolls an N-sided die
        /// </summary>
        /// <param name="sides"></param>
        /// <returns>A number between 1 and sides, inclusive</returns>
        public static int Roll(int sides)
        {
            return Random.Next(sides);
        }

        public static int Roll(int dice, int sides)
        {
            int result = 0;
            for (int die = 0; die < dice; die++)
            {
                result += Roll(sides);
            }
            return result;
        }

        public static RollResult DetailedRoll(int dice, int sides)
        {
            RollResult result = new RollResult();
            result.DiceRolled = $"{dice}d{sides}";

            for (int die = 0; die < dice; die++)
            {
                result.IndividualRolls.Add(Roll(sides));
            }

            result.NumericOutcome = result.IndividualRolls.Sum();
            result.DetailedOutcome = $"{result.NumericOutcome} ({String.Join(", ", result.IndividualRolls)})";

            return result;
        }

        public static RollResult Parse(string input)
        {
            if (!input.Contains("d"))
            {
                throw new ArgumentException("Expected format is {dice}d{sides}, e.g. 3d6");
            }
            else
            {
                string[] splitInput = input.Split('d');
                if (splitInput.Length != 2)
                {
                    throw new ArgumentException("Expected format is {dice}d{sides}, e.g. 3d6");
                }
                else
                {
                    return DetailedRoll(int.Parse(splitInput[0]), int.Parse(splitInput[1]));
                }

            }
        }
    }
}
