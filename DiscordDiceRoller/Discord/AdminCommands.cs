using Discord;

namespace DiscordDiceRoller
{
    internal class AdminCommands
    {
        IMessageChannel _channel;

        public AdminCommands(IMessageChannel channel)
        {
            _channel = channel;
        }

        public void ParseInput(string input)
        {
            //Check if this is an Admin slash command
            if (input[0] == '/')
            {       
                if (input == "/quit" || input == "/exit")
                {
                    Quit();
                    return;
                }
                else
                {                    
                    //Unknown command            
                    string output = "DEBUG: Unknown Admin command: " + input;
                    Console.WriteLine(output);
                }
            }
            else
            {
                //If not a command, simply send the message as-is
                SendMessage(input);
            }
        }

        public void SendMessage(string message)
        {
            _channel.SendMessageAsync(message);
        }

        /* Commands */        

        public void Quit()
        {
            SendMessage("Logging off");           
            Environment.Exit(0);
        }        
    }
}
