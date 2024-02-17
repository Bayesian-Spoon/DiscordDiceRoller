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
            string? input = Console.ReadLine();
            while (input != "/exit")
            {
                if (input != null)
                {
                    //Show help
                    if (input == "/help")
                    {
                        ShowHelp();
                    }
                    else
                    {
                        try
                        {
                            DiceRoller.RollResult? result;
                            int firstSpace = input.IndexOf(' ');
                            if (firstSpace == -1)
                            {
                                result = DynamicParser.Parse(input, "");
                            }
                            else
                            {
                                result = DynamicParser.Parse(input.Substring(0, firstSpace), input.Substring(firstSpace + 1));
                            }

                            if (result != null)
                            {
                                Console.WriteLine(result.DetailedOutcome);
                            }
                            else
                            {
                                //This should never happen
                                Console.WriteLine("Parse succeeded, but we didn't get any results!");
                                Console.WriteLine($"Input was: {input}");
                            }
                        }
                        catch (ArgumentException e)
                        {
                            //Parsing failed
                            Console.WriteLine(e.Message);
                        }
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
