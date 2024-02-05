using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordDiceRoller.Discord
{
    internal class DiscordClient
    {
        DiscordSocketClient client;
        Secrets secrets;

        public DiscordClient(Secrets _secrets)
        {
            secrets = _secrets;
        }

        public void Run()
        {            
            MainAsync().GetAwaiter().GetResult();
        }

        internal async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                //var config = new DiscordSocketClient() { GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.GuildMessages }; 
                //client = new DiscordSocketClient(config);

                client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                await client.LoginAsync(TokenType.Bot, secrets.DiscordToken);
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                // Announce bot login
                client.Ready += StartupCommands;

                // Allow you to send commands and messages via the command line
                client.Ready += AllowAdminCommands;

                // Run until terminated
                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task StartupCommands()
        {
            var server = client.GetGuild(secrets.ServerId);
            if (server != null)
            {
                var channel = server.GetChannel(secrets.ChannelId) as IMessageChannel;
                if (channel != null)
                {
                    channel.SendMessageAsync(client.CurrentUser.Username + " is online");
                }
            }
            return Task.CompletedTask;
        }      

        private Task AllowAdminCommands()
        {
            var server = client.GetGuild(secrets.ServerId);
            if (server != null)
            {
                var channel = server.GetChannel(secrets.ChannelId) as IMessageChannel;
                if (channel != null)
                {
                    AdminCommands Admin = new AdminCommands(channel);

                    while (true)
                    {
                        string input = Console.ReadLine();
                        Admin.ParseInput(input);
                    }
                }
            }
            return Task.CompletedTask;
        }


        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
