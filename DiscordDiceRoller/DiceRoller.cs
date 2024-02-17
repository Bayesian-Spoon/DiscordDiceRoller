using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    public class DiceRoller
    {
        public enum RollType { Regular, BITD, Insomnia };
        public class RollResult
        {
            public readonly RollType type;
            public string? DiceRolled;
            public List<int> IndividualRolls = new List<int>();
            public string? DetailedOutcome;
            public int? NumericOutcome;

            public RollResult(RollType _type)
            {
                type = _type;
            }

            public string IndividualRollsJoined()
            {
                return string.Join(", ", IndividualRolls);
            }
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
            RollResult result = new RollResult(RollType.Regular);
            result.DiceRolled = $"{dice}d{sides}";

            for (int die = 0; die < dice; die++)
            {
                result.IndividualRolls.Add(Roll(sides));
            }

            result.NumericOutcome = result.IndividualRolls.Sum();
            result.DetailedOutcome = $"{result.NumericOutcome} ({result.IndividualRollsJoined()})";

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

        public static RollResult BladesInTheDark(int dice)
        { 
            RollResult result = new RollResult(RollType.Regular);
            result.DiceRolled = $"{dice}d6, Blades In The Dark style";

            //Roll dice
            for (int die = 0; die < dice; die++)
            {
                result.IndividualRolls.Add(Roll(6));
            }

            //Calculate outcome
            result.NumericOutcome= result.IndividualRolls.Max();
            switch(result.NumericOutcome)
            {
                case 1:
                case 2:
                case 3:
                    result.DetailedOutcome = "Fail";
                    break;
                case 4:
                case 5:
                    result.DetailedOutcome = "Mixed";
                    break;
                case 6:
                    //Multiple sixes produces a critical
                    if (result.IndividualRolls.Count(x => x == 6) > 1)
                    {
                        result.DetailedOutcome = "Critical";
                    }
                    else
                    {
                        result.DetailedOutcome = "Success";
                    }
                    break;
                default:
                    throw new ArgumentException($"Numeric Outcome was {result.NumericOutcome}, but should be 1-6");
            }

            //Append details to outcome
            result.DetailedOutcome += $" ({result.IndividualRollsJoined()})";

            //return
            return result;
        }
    }
}
