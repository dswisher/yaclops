using System;
using Yaclops;

namespace Sample.Helpers
{
    public static class CommandExtensions
    {
        public static void Dump<T>(this T command) where T : ISubCommand
        {
            // TODO - dump all the properties

            Console.WriteLine("Executing '{0}'...", command.GetType().Name.Replace("Command", string.Empty));
        }
    }
}
