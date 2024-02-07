using DiscordDiceRoller.Discord;

namespace DiscordDiceRoller
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if TRUE
            //Easy debug override for console mode
            new CommandLineClient().Run();
            return;
#endif

            if (args.Length == 0)
            {
                //Try to get secrets and connect to Discord
                try
                {
                    Secrets secrets = SecretsManager.Get();
                    if (secrets != null)
                    {
                        new DiscordClient(secrets).Run();
                        return;
                    }
                }
                catch
                {
                    //Fall over to next section
                }

                //If that fails, fall back to local
                new CommandLineClient().Run();
                return;
            }
            else
            {
                /*
                if (args[0].ToLower() == "discord")
                {
                    Secrets secrets = new Secrets();
                    new DiscordClient(secrets).Run();
                }
                else if (args[0].ToLower() == "local")
                {
                    new CommandLineClient().Run();
                }
                else
                {
                    Console.WriteLine($"Unknown mode: {args[0]}");
                    Console.WriteLine("Use 'discord' for DiscordClient mode, or 'local' for local console usage");
                    Console.WriteLine("If no arguments are provided, the client will try Discord first, then local");
                    Console.WriteLine();

                    Console.WriteLine("Press ENTER to exit");

                    Console.ReadLine();
                }
                */
            }
        }        
    }
}