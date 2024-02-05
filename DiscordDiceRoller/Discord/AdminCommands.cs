using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (input[0] == '!')
            {       
                if (input == "!quit" || input == "!exit")
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
