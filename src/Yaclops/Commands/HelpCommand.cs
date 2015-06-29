using System;
using Yaclops.Model;

namespace Yaclops.Commands
{
    internal class HelpCommand
    {
        private readonly CommandNode _start;

        public static HelpCommand Make(CommandNode start)
        {
            return new HelpCommand(start);
        }


        public HelpCommand(CommandNode start)
        {
            _start = start;
            // TODO
        }


        public void Execute()
        {
            // TODO
            Console.WriteLine("Help is not yet implemented! Node.verb={0}, type={1}", _start.Verb, _start.GetType().Name);
        }
    }
}
