using static DiscordDiceRoller.DiceRoller;

namespace DiscordDiceRoller.DiceParsers
{
    internal class BasicDiceParser
    {
        /// <summary>
        /// Rolls <paramref name="dice" /> dice, with <paramref name="sides" />-sides
        /// </summary>
        /// <param name="sides"></param>
        /// <returns>A RollResult object detailing the results of the the dice roll</returns>
        [DiceParser("/roll")]
        public static RollResult Parse(string slashCommand, string args)
        {
            RollResult result = new RollResult(slashCommand, args);

            //Parse args:
            // Expecting a single arg in the form of {dice}d{sides}
            if (!args.Contains("d"))
            {
                throw new ArgumentException("Expected format is {dice}d{sides}, e.g. 3d6");
            }
            
            string[] splitInput = args.Split('d');
            if (splitInput.Length != 2)
            {
                throw new ArgumentException("Expected format is {dice}d{sides}, e.g. 3d6");
            }
            
            int dice = int.Parse(splitInput[0]);
            int sides = int.Parse(splitInput[1]);

            //Roll dice            
            result.DiceRolled = $"{dice}d{sides}";

            for (int die = 0; die < dice; die++)
            {
                result.IndividualRolls.Add(Roll(sides));
            }

            //Calculate outcome
            result.NumericOutcome = result.IndividualRolls.Sum();
            result.DetailedOutcome = $"{result.NumericOutcome} ({result.IndividualRollsJoined()})";

            //Done
            return result;
        }
    }
}
