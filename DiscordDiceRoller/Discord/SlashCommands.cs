using Discord.WebSocket;

namespace DiscordDiceRoller.Discord
{
    internal class SlashCommands
    {
        static internal async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "roll":
                    await HandleRoll(command);
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

            //Roll
            int result = DiceRoller.Roll(dice, sides);

            //Respond
            var name = command.User.GlobalName;
            await command.RespondAsync($"{name} rolled {dice}d{sides} and got: {result}");
        }
    }
}
