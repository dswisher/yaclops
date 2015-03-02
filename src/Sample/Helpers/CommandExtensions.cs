using System;
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
                    // TODO - if a collection, dump out the values, rather than the type!

                    Console.WriteLine("   {0,15}: {1}", p.Name, p.GetValue(command));
                }
            }
        }
    }
}
