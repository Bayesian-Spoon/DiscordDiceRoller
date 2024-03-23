using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Discord.Net;
using Newtonsoft.Json;
using System;

namespace DiscordDiceRoller.Discord
{
    public sealed class DiscordServerSingleton
    {
        private static readonly DiscordServerSingleton instance = new DiscordServerSingleton();        
        public SocketGuild? Server;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DiscordServerSingleton()
        {
        }

        private DiscordServerSingleton()
        {
        }

        public static DiscordServerSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        internal void Initialize(SocketGuild server)
        {
            Server = server;            
        }
    }


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
                var config = new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.All
                };
                client = new DiscordSocketClient(config);
                client.SlashCommandExecuted += SlashCommands.SlashCommandHandler;
                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                await client.LoginAsync(TokenType.Bot, secrets.DiscordToken);
                await client.StartAsync();

                // Create Slash Commands
                client.Ready += CreateSlashCommands;

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
                //This can't be run before startup
                DiscordServerSingleton.Instance.Initialize(server);

                //Ping Channel with "online" message
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
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

        public async Task CreateSlashCommands()
        {
            var guild = client.GetGuild(secrets.ServerId);

            //Enable this block to delete existing commands
            //Useful for removing legacy commands - Discord will remember every command previously registered on a particular server
#if false
            try
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
#endif


            //Roll command
            var rollCommand = new SlashCommandBuilder()
                .WithName("roll")
                .WithDescription("Roll some dice! Defaults to 1d20 if not otherwise specified")
                .AddOption("dice", ApplicationCommandOptionType.Integer, "How many dice to roll", isRequired: false)
                .AddOption("sides", ApplicationCommandOptionType.Integer, "How many sides the dice has", isRequired: false)
                ;

            try
            {
                await guild.CreateApplicationCommandAsync(rollCommand.Build());
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }

            //Fate command
            var fateCommand = new SlashCommandBuilder()
                .WithName("fate")
                .WithDescription("Roll 4dF. Will produce a result between -4 and +4, with a strong bias towards 0.")
                ;

            try
            {
                await guild.CreateApplicationCommandAsync(fateCommand.Build());
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }

            //Test command
            var testCommand = new SlashCommandBuilder()
               .WithName("test")
               .WithDescription("Outputs debugging data.")
               ;

            await guild.CreateApplicationCommandAsync(testCommand.Build());
        }
    }
}
