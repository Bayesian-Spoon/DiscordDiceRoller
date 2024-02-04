using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    public class DiceRoller
    {
        /// <summary>
        /// Rolls an N-sided die
        /// </summary>
        /// <param name="sides"></param>
        /// <returns>A number between 1 and sides, inclusive</returns>
        public static int Roll(int sides)
        {
            return Random.Next(sides);
        }

       
    }
}
