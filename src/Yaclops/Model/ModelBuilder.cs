using System.Collections.Generic;
using Yaclops.Reflecting;

namespace Yaclops.Model
{
    internal class ModelBuilder
    {

        public ModelBuilder()
        {
            Root = new CommandRoot();
        }


        public void AddTypes<T>(IEnumerable<TypeEntry<T>> types)
        {
            foreach (var type in types)
            {
                AddType(new ReflectedCommand<T>(type.Type, type.Factory));
            }
        }


        public void AddType(IReflectedCommand reflected)
        {
            CommandGroup group = Root;

            for (int i = 0; i < reflected.Verbs.Count - 1; i++)
            {
                group = group.GetOrAddGroup(reflected.Verbs[i]);
            }

            var command = group.AddCommand(reflected.Verbs[reflected.Verbs.Count - 1]);

            // TODO - add options and func and whatnot to the command
        }


        public CommandRoot Root { get; private set; }
    }
}
