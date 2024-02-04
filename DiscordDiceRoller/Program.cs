namespace DiscordDiceRoller
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Introduction
            Console.WriteLine("Welcome to the Discord Dice Roller");
            Console.WriteLine("Type /roll to roll 1d6");
            Console.WriteLine("Type /help for instructions");
            Console.WriteLine("Type /exit to end this session");

            //Main loop
            string input = Console.ReadLine();
            while (input != "/exit")
            {
                //Roll dice
                if (input == "/roll")
                {
                    Console.WriteLine(string.Format("Rolled 1d6 and got a {0}", DiceRoller.Roll(6)));
                }

                //Show help
                else if (input == "/help")
                {
                    Console.WriteLine("Type /roll to roll 1d6");
                    Console.WriteLine("Type /help for instructions");
                    Console.WriteLine("Type /exit to end this session");
                }

                //Invalid Input
                else
                {
                    Console.WriteLine(String.Format("Unknown command: {0}", input));
                }

                //Get next instruction
                input = Console.ReadLine();
            }

            //Goodbye
            Console.WriteLine("Goodbye!");
        }        
    }
}