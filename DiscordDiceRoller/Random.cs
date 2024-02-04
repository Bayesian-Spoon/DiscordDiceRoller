using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDiceRoller
{
    /// <summary>
    /// Our random number generator. Since we're mostly dealing with dice, the default minimum is 1
    /// </summary>
    internal class Random
    {
        /// <summary>
        /// We use a singleton to ensure the random seed isn't reset between calls
        /// </summary>
        #region singleton
        private static readonly Random _instance = new Random();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Random()
        {
        }

        private Random()
        {
        }

        public static Random Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        System.Random random = new System.Random();

        /// <summary>
        /// Generate a number between 1 and <paramref name="max" />, inclusive
        /// </summary>
        public static int Next(int max)
        {
            return Next(1, max);
        }

        /// <summary>
        /// Generate a number between <paramref name="min" /> and <paramref name="max" />, inclusive.
        /// </summary>
        public static int Next(int min, int max)
        {
            return Instance.random.Next(min, max+1); //max+1 because System.Random is not inclusive of the max
        }
    }
}
