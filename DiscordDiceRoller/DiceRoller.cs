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

        /*
        * Finds the highest pool amongst multiple possible dice pools. 
        * In the case of a tie, the order within the original dictionary will be the tie-breaker.
        * 
        * Examples:
        *  Discipline (6) beats Exhaustion (5, 4, 4) because it has the higher first result
        *  Discipline (6,5) beats Exhaustion (6, 4) because it has the higher second result
        *  Discipline (6, 1) beats Exhaustion (6) because Discipline has more dice
        *  Discipline (6) beats Exhaustion (6) because it wins the tie-breaker
        */
        public static string DetermineHighestResult(Dictionary<string, List<int>> categories)
        {
            //Ensure our results are sorted
            Dictionary<string, int[]> sortedCategories = new Dictionary<string, int[]>();
            foreach (string category in categories.Keys)
            {
                sortedCategories.Add(category, categories[category].OrderByDescending(x => x).ToArray());
            }

            //Keep track of contenders
            // Category names should always be unique, so we can use a HashSet here
            HashSet<string> viableCategories = sortedCategories.Keys.ToHashSet();
            HashSet<string> categoriesToRemove = new HashSet<string>();

            int largestPool = categories.Values.Select(x => x.Count).Max();
            for (int index = 0; index < largestPool; index++)
            {
                int highestDie = 0;
                foreach (string category in viableCategories)
                {
                    if (sortedCategories[category].Length <= index)
                    {
                        //Remove if there's no more dice in this category
                        categoriesToRemove.Add(category);
                    }
                    else if (sortedCategories[category][index] < highestDie)
                    {
                        //Remove if this isn't the highest roll
                        categoriesToRemove.Add(category);
                    }
                    else
                    {
                        highestDie = sortedCategories[category][index];
                    }
                }

                //Prune
                foreach (string removal in categoriesToRemove)
                {
                    viableCategories.Remove(removal);
                }
                categoriesToRemove.Clear();

                //Now that we know the real highest die, do another removal pass
                foreach (string category in viableCategories)
                {
                    if (sortedCategories[category][index] < highestDie)
                    {
                        //Remove if this isn't the highest roll
                        categoriesToRemove.Add(category);
                    }
                }

                //Prune again
                foreach (string removal in categoriesToRemove)
                {
                    viableCategories.Remove(removal);
                }
                categoriesToRemove.Clear();

                //Check if we have a winner
                if (viableCategories.Count == 1)
                {
                    return viableCategories.First();
                }
            }

            //Ties are resolved based on the order within the original dictionary
            return viableCategories.First();
        }
    }
}
