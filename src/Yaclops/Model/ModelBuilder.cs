using System;
using System.Collections.Generic;
using Yaclops.Commands;
using Yaclops.Reflecting;

namespace Yaclops.Model
{
    internal class ModelBuilder
    {

        public ModelBuilder()
        {
            Root = new CommandRoot();
        }


        public CommandRoot Root { get; private set; }


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

            var command = group.AddExternalCommand(reflected.Verbs[reflected.Verbs.Count - 1], reflected.Factory);

            command.Summary = reflected.Summary;

            AddNamedParameters(reflected, command, x => x);
            AddPositionalParameters(reflected, command, x => x);
        }



        public void AddInternalCommands()
        {
            CommandGroup yaclops = Root.GetOrAddGroup("yaclops");

            yaclops.Hidden = true;

            yaclops.AddInternalCommand("dump", (r, n) => new YaclopsDumpTreeCommand(r).Execute());
        }



        public void AddGroupOptions(IEnumerable<GroupOptionsEntry> groupOptions)
        {
            foreach (var entry in groupOptions)
            {
                // TODO - support options on more than just the root group
                CommandGroup group = Root;

                var reflected = entry.ReflectedObject;

                AddNamedParameters(reflected, group, entry.PropertyTarget);
                AddPositionalParameters(reflected, group, entry.PropertyTarget);
            }
        }



        private void AddPositionalParameters(IReflectedObject reflected, CommandNode target, Func<object, object> propertyTarget)
        {
            // TODO - handle positional parameters, too, at least for global parser!
            foreach (var reflectedParam in reflected.PositionalParameters)
            {
                var commandParam = new CommandPositionalParameter(reflectedParam.PropertyName, reflectedParam.IsList, reflectedParam.IsRequired, propertyTarget);

                target.PositionalParameters.Add(commandParam);
            }
        }



        private void AddNamedParameters(IReflectedObject reflected, CommandNode target, Func<object, object> propertyTarget)
        {
            foreach (var reflectedParam in reflected.NamedParameters)
            {
                var commandParam = new CommandNamedParameter(reflectedParam.PropertyName, reflectedParam.IsBool, propertyTarget);

                commandParam.LongNames.AddRange(reflectedParam.LongNames);
                commandParam.ShortNames.AddRange(reflectedParam.ShortNames);

                target.NamedParameters.Add(commandParam);
            }
        }
    }
}
