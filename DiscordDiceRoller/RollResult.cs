using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    public class RollResult
    {
        public readonly string SlashCommand;
        public readonly string RawArguments;

        public string? DiceRolled;
        public List<int> IndividualRolls = new List<int>();
        public string? DetailedOutcome;
        public int? NumericOutcome;

        public RollResult(string _slashCommand, string _rawArguments)
        {
            SlashCommand = _slashCommand;
            RawArguments = _rawArguments;
        }

        public string IndividualRollsJoined()
        {
            return string.Join(", ", IndividualRolls);
        }

        public string IndividualRollsSorted()
        {
            return string.Join(", ", IndividualRolls.OrderBy(x => x));
        }
    }
}
