using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    internal class CommandLineClient
    {
        public void Run()
        {
            //Introduction
            Console.WriteLine("Welcome to the Discord Dice Roller");
            ShowHelp();

            //Main loop
            string input = Console.ReadLine();
            while (input != "/exit")
            {
                if (input != null)
                {
                    //Roll dice
                    if (input == "/roll")
                    {
                        DiceRoller.RollResult result = DiceRoller.DetailedRoll(3, 6);
                        Console.WriteLine($"Rolled {result.DiceRolled} and got {result.DetailedOutcome}");
                    }
                    else if (input.StartsWith("/roll"))
                    {                        
                        string diceInput = input.Substring(5); // this choice of index supports "/roll3d6" as well as the expected "/roll 3d6"
                        //Console.WriteLine(diceInput);
                        DiceRoller.RollResult result = DiceRoller.Parse(diceInput);
                        Console.WriteLine($"Rolled {result.DiceRolled} and got {result.DetailedOutcome}");
                    }

                    //Show help
                    else if (input == "/help")
                    {
                        ShowHelp();
                    }

                    //Invalid Input
                    else
                    {
                        Console.WriteLine(String.Format("Unknown command: {0}", input));
                    }
                }

                //Get next instruction
                input = Console.ReadLine();
            }

            //Goodbye
            Console.WriteLine("Goodbye!");
        }

        public static void ShowHelp()
        {
            Console.WriteLine("Type /roll to roll 3d6");
            Console.WriteLine("Type /help for instructions");
            Console.WriteLine("Type /exit to end this session");
        }
    }
}
