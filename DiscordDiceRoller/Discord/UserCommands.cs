using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller.Discord
{
    /*
     * Based on https://discordnet.dev/guides/text_commands/intro.html
     */
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// This returns the raw Discord handle, NOT the display name
        /// </summary>
        public string GetUserDiscordHandle()
        {
            return Context.User.Username;
        }

        /*
         * EXAMPLE COMMANDS
         */

        [Command("ping")]
        public Task PingAsync()
        {
            return ReplyAsync("pong!");
        }
        
        [Command("echo")]
        public Task EchoAsync([Remainder] string text)
        { 
            // Insert a ZWSP before the text to prevent triggering other bots!
            return ReplyAsync('\u200B' + text);
        }

        /*
         * DICE ROLLER
         */

        [Command("roll")]
        public Task Roll()
        {
            int roll = DiceRoller.Roll(20);
            string output = $"{GetUserDiscordHandle()} rolled a d20 and got a: {roll}";            
            return ReplyAsync(output);
        }
    }
}
