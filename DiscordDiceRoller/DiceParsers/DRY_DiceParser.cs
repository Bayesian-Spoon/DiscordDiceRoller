using static DiscordDiceRoller.DiceRoller;

namespace DiscordDiceRoller.DiceParsers
{
    public class DRYRollResult : RollResult
    {
        public DRYRollResult(string _slashCommand, string _rawArguments) : base(_slashCommand, _rawArguments) { }

        public List<int> DisciplineRolls = new List<int>();
        public List<int> ExhaustionRolls = new List<int>();
        public List<int> MadnessRolls = new List<int>();
        public List<int> PainRolls = new List<int>();

        public DRY_Outcome PlayerStrength;
        public DRY_Outcome Dominance;
    }

    public enum DRY_Outcome { Discipline, Exhaustion, Madness, Pain };

    internal class DRY_DiceParser
    {
        [DiceParser("/dry")]
        public static RollResult Parse(string slashCommand, string args)
        {
            DRYRollResult result = new DRYRollResult(slashCommand, args);

            //Parse args:
            // 4 arguments: Discipline, Exhaustion, Madness, and Pain
            string[] argList = args.Split(',');
            if (argList.Length != 4)
            {
                throw new ArgumentException("Expected 4 arguments, but only got " + argList.Length);
            }

            int disciplineDice = int.Parse(argList[0]);
            int exhaustionDice = int.Parse(argList[0]);
            int madnessDice = int.Parse(argList[0]);
            int painDice = int.Parse(argList[0]);

            result.DiceRolled = $"Discipline: {disciplineDice}. Exhaustion: {exhaustionDice}. Madness: {madnessDice}. Pain: {painDice}.";

            //Roll all four dice pools
            for (int die = 0; die < disciplineDice; die++)
            {
                result.DisciplineRolls.Add(Roll(6));
            }

            for (int die = 0; die < exhaustionDice; die++)
            {
                result.ExhaustionRolls.Add(Roll(6));
            }

            for (int die = 0; die < madnessDice; die++)
            {
                result.MadnessRolls.Add(Roll(6));
            }

            for (int die = 0; die < painDice; die++)
            {
                result.PainRolls.Add(Roll(6));
            }

            //Calculate successes
            int playerSuccesses = result.DisciplineRolls.Where(x => x <= 3).Count()
                                + result.ExhaustionRolls.Where(x => x <= 3).Count()
                                + result.MadnessRolls.Where(x => x <= 3).Count();
            int painSuccesses = result.PainRolls.Where(x => x <= 3).Count();

            bool playerWon = playerSuccesses >= painSuccesses;

            //Calculate Player Strength          
            Dictionary<string, List<int>> categories = new Dictionary<string, List<int>>();
            
            categories.Add(DRY_Outcome.Discipline.ToString(), result.DisciplineRolls);
            categories.Add(DRY_Outcome.Exhaustion.ToString(), result.ExhaustionRolls);
            categories.Add(DRY_Outcome.Madness.ToString(), result.MadnessRolls);

            result.PlayerStrength = (DRY_Outcome) Enum.Parse(typeof(DRY_Outcome), DetermineHighestResult(categories));

            //Calculate Dominance
            categories.Add(DRY_Outcome.Pain.ToString(), result.PainRolls);
            result.Dominance = (DRY_Outcome)Enum.Parse(typeof(DRY_Outcome), DetermineHighestResult(categories));

            //Descriptive outcomes
            //TODO

            //Done
            return result;
        }       
    }
}
