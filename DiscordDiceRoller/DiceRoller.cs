using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{  
    public class DiceRoller
    {
        /// <summary>
        /// Rolls an <paramref name="sides" />-sided die
        /// </summary>
        /// <param name="sides"></param>
        /// <returns>A number between 1 and sides, inclusive</returns>
        public static int Roll(int sides)
        {
            return Random.Next(sides);
        }

        /// <summary>
        /// Rolls <paramref name="dice" /> dice, with <paramref name="sides" />-sides
        /// </summary>
        /// <param name="sides"></param>
        /// <returns>The sum of the dice rolled</returns>
        public static int Roll(int dice, int sides)
        {
            int result = 0;
            for (int die = 0; die < dice; die++)
            {
                result += Roll(sides);
            }
            return result;
        }
    }
}
