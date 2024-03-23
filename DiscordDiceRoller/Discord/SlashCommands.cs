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
                case "test":
                    await HandleTest(command);
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
            await command.RespondAsync($"{GetDisplayName(command)} rolled {dice}d{sides} and got: {result}");
        }

        static internal async Task HandleFate(SocketSlashCommand command)
        {
            //Roll
            FateRollResult result = FateDiceParser.Parse("/roll", "");

            //Respond
            await command.RespondAsync($"{GetDisplayName(command)} rolled {result.DiceRolled}. {result.DetailedOutcome}");
        }

        static internal async Task HandleTest(SocketSlashCommand command)
        {
            //Respond
            await command.RespondAsync($"GlobalName is {command.User.GlobalName}. Username is {command.User.Username}. DisplayName is {GetDisplayName(command)}.");
        }
        
        static string GetDisplayName(SocketSlashCommand command)
        {
            if (DiscordServerSingleton.Instance.Server != null)
            {
                //Default to server nickname if we can
                return DiscordServerSingleton.Instance.Server.GetUser(command.User.Id).DisplayName;
            }
            else
            {
                //Otherwise use the global username
                return command.User.GlobalName;
            }
        }
    }
}
