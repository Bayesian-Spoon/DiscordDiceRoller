using System.Reflection;

namespace DiscordDiceRoller
{
    public class DiceParser : Attribute
    {
        public readonly string slashCommand;

        public DiceParser(string _slashCommand)
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
                      .Where(m => m.GetCustomAttributes(typeof(DiceParser), false).Length > 0)
                      .ToArray();

            foreach (var method in methods)
            {
                object[] attributes = method.GetCustomAttributes(typeof(DiceParser), false);
                foreach (object attr in attributes)
                {
                    DiceParser DiceParser = (DiceParser)attr;
                    parsers.Add(DiceParser.slashCommand, method);
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
}
