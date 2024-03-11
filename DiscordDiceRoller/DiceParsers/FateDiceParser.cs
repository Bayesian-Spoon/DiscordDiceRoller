using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordDiceRoller.DiceRoller;

namespace DiscordDiceRoller.DiceParsers
{
    internal class FateDiceParser
    {
        [DiceParser("/fate")]
        public static RollResult Parse(string slashCommand, string args)
        {
            RollResult result = new RollResult(slashCommand, args);

            //Parse args:
            // Only argument is an (optional) target difficulty
            int? difficulty = null;
            if (!String.IsNullOrEmpty(args))
            {
                difficulty = int.Parse(args);
            }

            result.DiceRolled = $"4dF";

            //Roll 4dF
            for (int die = 0; die < 4; die++)
            {
                int dieResult = Roll(3) - 2; //-1, 0, or +1
                result.IndividualRolls.Add(dieResult);
            }

            //Calculate results
            result.NumericOutcome = result.IndividualRolls.Sum();

            if (difficulty == null)
            {
                result.DetailedOutcome = $"Total: {result.NumericOutcome}";
            }
            else
            {
                if (result.NumericOutcome >= difficulty)
                {
                    result.DetailedOutcome = "Success";
                }
                else
                {
                    result.DetailedOutcome = "Failure";
                }
                result.DetailedOutcome += $" against DC {difficulty} (Total: {result.NumericOutcome})";
            }

            //Show individual dice using dF formatting
            List<string> fateResults = new List<string>();
            foreach (int dieResult in result.IndividualRolls.OrderByDescending(x => x))
            {
                switch (dieResult)
                {
                    case -1:
                        fateResults.Add("-");
                        break;
                    case 0:
                        fateResults.Add("0");
                        break;
                    case 1:
                        fateResults.Add("+");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unexpected dF result: " + dieResult);
                }                                   
            }
            result.DetailedOutcome += $" ({String.Join(" ", fateResults)})";

            //Done
            return result;
        }
    }
}
