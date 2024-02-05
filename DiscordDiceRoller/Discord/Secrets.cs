using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    internal class Secrets
    {
        // Tokens should be considered secret data and never hard-coded.
        public string? DiscordToken;
        public ulong ServerId;
        public ulong ChannelId;
    }

    internal class SecretsManager
    {
        public static Secrets Get()
        {            
            string json = File.ReadAllText("secrets.json");
            Secrets result = JsonConvert.DeserializeObject<Secrets>(json);
            return result;
        }
    }
}
