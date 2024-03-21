using Discord.WebSocket;
using DiscordDiceRoller.DiceParsers;

namespace DiscordDiceRoller.Discord
{
    //TODO: Upgrade this to use the DynamicParser model

    internal class SlashCommands
    {
        static internal async Task SlashCommandHandler(SocketSlashCommand command)
        {
            //TODO: Pass args and command to DynamicParser.Parse();            
            //string?[] args = command.Data.Options.Select(x => x.Value.ToString()).ToArray();

            switch (command.Data.Name)
            {
                case "roll":
                    await HandleRoll(command);
                    break;
                case "fate":
                    await HandleFate(command);
                    break;
                default:
                    await command.RespondAsync($"Unknown command: {command.Data.Name}");
                    break;
            }

        }

        static internal async Task HandleRoll(SocketSlashCommand command)
        {
            //Parameter: Dice
            int dice;
            var diceParamater = command.Data.Options.FirstOrDefault(x => x.Name == "dice")?.Value;
            if (diceParamater != null)
            {
                dice = (int)(long)diceParamater;
            }
            else
            {
                dice = 1;
            }

            //Parameter: Sides
            int sides;
            var sidesParamater = command.Data.Options.FirstOrDefault(x => x.Name == "sides")?.Value;
            if (sidesParamater != null)
            {
                sides = (int)(long)sidesParamater;
            }
            else
            {
                sides = 20;
            }

            //TODO: Convert this to use BasicDiceParser.Parse()?

            //Roll
            int result = DiceRoller.Roll(dice, sides);

            //Respond
            var name = command.User.GlobalName;
            await command.RespondAsync($"{name} rolled {dice}d{sides} and got: {result}");
        }

        static internal async Task HandleFate(SocketSlashCommand command)
        {
            //Roll
            FateRollResult result = FateDiceParser.Parse("/roll", "");

            //Respond
            var name = command.User.GlobalName;
            await command.RespondAsync($"{name} rolled {result.DiceRolled} and got: {result.NumericOutcome} ({result.IndividualRollsSorted()})");
        }
    }
}
