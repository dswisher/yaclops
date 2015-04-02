using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops;

namespace Sample.Helpers
{
    public static class CommandExtensions
    {
        public static void Dump(this ISubCommand command)
        {
            var type = command.GetType();

            Console.WriteLine();
            Console.WriteLine("*** {0} ***", type.Name.Replace("Command", string.Empty));

            var props = type.GetProperties();

            if (props.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Properties:");

                foreach (var p in props)
                {
                    string val;
                    // TODO - handle more than just List<string>!
                    if (p.PropertyType == typeof (List<string>))
                    {
                        var list = (List<string>)p.GetValue(command);
                        val = list == null ? "<null>" : string.Join(", ", list);
                    }
                    else
                    {
                        var raw = p.GetValue(command);
                        val = raw != null ? p.GetValue(command).ToString() : "<null>";
                    }

                    Console.WriteLine("   {0,15}: {1}", p.Name, val);
                }
            }
        }
    }
}
