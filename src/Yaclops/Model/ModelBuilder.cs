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


        public void AddType<T>(IReflectedCommand<T> reflected)
        {
            CommandGroup group = Root;

            for (int i = 0; i < reflected.Verbs.Count - 1; i++)
            {
                group = group.GetOrAddGroup(reflected.Verbs[i]);
            }

            var command = group.AddCommand(reflected.Verbs[reflected.Verbs.Count - 1], reflected.Factory);

            foreach (var reflectedParam in reflected.NamedParameters)
            {
                var commandParam = new CommandNamedParameter(reflectedParam.PropertyName, reflectedParam.IsBool);

                commandParam.LongNames.AddRange(reflectedParam.LongNames);
                commandParam.ShortNames.AddRange(reflectedParam.ShortNames);

                command.NamedParameters.Add(commandParam);
            }

            foreach (var reflectedParam in reflected.PositionalParameters)
            {
                var commandParam = new CommandPositionalParameter(reflectedParam.PropertyName, reflectedParam.IsList);

                command.PositionalParameters.Add(commandParam);
            }
        }


        public CommandRoot Root { get; private set; }
    }
}
