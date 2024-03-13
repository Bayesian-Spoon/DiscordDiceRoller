using static DiscordDiceRoller.DiceRoller;

namespace DiscordDiceRoller.DiceParsers
{
    internal class BITD_DiceParser
    {
        [DiceParser("/bitd")]
        public static RollResult Parse(string slashCommand, string args)
        {
            RollResult result = new RollResult(slashCommand, args);

            //Parse args:
            // Only argument should be an integer number of dice
            int dice = int.Parse(args);

            result.DiceRolled = $"{dice}d6, Blades In The Dark style";

            //Roll dice
            for (int die = 0; die < dice; die++)
            {
                result.IndividualRolls.Add(Roll(6));
            }

            //Calculate outcome
            result.NumericOutcome = result.IndividualRolls.Max();
            switch (result.NumericOutcome)
            {
                case 1:
                case 2:
                case 3:
                    result.DetailedOutcome = "Failure";
                    break;
                case 4:
                case 5:
                    result.DetailedOutcome = "Mixed";
                    break;
                case 6:
                    //Multiple sixes produces a critical
                    if (result.IndividualRolls.Count(x => x == 6) > 1)
                    {
                        result.DetailedOutcome = "Critical Success";
                    }
                    else
                    {
                        result.DetailedOutcome = "Success";
                    }
                    break;
                default:
                    throw new ArgumentException($"Numeric Outcome was {result.NumericOutcome}, but should be 1-6");
            }
            
            result.DetailedOutcome += $" ({result.IndividualRollsSorted()})";

            //Done
            return result;
        }
    }
}
