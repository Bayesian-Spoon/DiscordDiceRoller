using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DiscordDiceRoller.DiceRoller;

namespace DiscordDiceRoller
{
    public class RollParser : Attribute
    {
        public readonly string slashCommand;

        public RollParser(string _slashCommand)
        {
            this.slashCommand = _slashCommand;
        }
    }


    internal class DynamicParser
    {
        public static Dictionary<string, MethodInfo> GetParsers()
        {
            Dictionary<string, MethodInfo> parsers = new Dictionary<string, MethodInfo>();

            Assembly assembly = Assembly.GetExecutingAssembly();

            var methods = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(RollParser), false).Length > 0)
                      .ToArray();

            foreach (var method in methods)
            {
                object[] attributes = method.GetCustomAttributes(typeof(RollParser), false);
                foreach (object attr in attributes)
                {
                    RollParser rollParser = (RollParser)attr;
                    parsers.Add(rollParser.slashCommand, method);
                }
            }

            return parsers;
        }

        public static RollResult? Parse(string slashCommand, string args)
        {
            var method = GetParsers().FirstOrDefault(x => x.Key == slashCommand).Value;

            if (method == null)
            {
                //TODO
                throw new ArgumentException("Uknown slash command: " + slashCommand);
            }
            else
            {
                var obj = Activator.CreateInstance(method.DeclaringType);
                return method.Invoke(obj, new object[] { slashCommand, args }) as RollResult;
            }
        }
    }

    public class RollParser_Example1
    {
        [RollParser("/roll")]
        public static RollResult Parse(string slashCommand, string args)
        {
            RollResult rollResult = new RollResult(RollType.Regular);
            rollResult.DetailedOutcome = $"Example /roll invoked with args {args}";

            return rollResult;
        }
    }

    public class RollParser_Example2
    {
        [RollParser("/blades")]
        public static RollResult Parse(string slashCommand, string args)
        {
            RollResult rollResult = new RollResult(RollType.Regular);
            rollResult.DetailedOutcome = $"Example /blades invoked with args {args}";

            return rollResult;
        }
    }

}
