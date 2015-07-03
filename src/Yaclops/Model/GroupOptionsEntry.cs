using System;
using Yaclops.Reflecting;

namespace Yaclops.Model
{
    internal abstract class GroupOptionsEntry
    {
        public abstract Type OptionType { get; }
        public abstract IReflectedObject ReflectedObject { get; }
    }



    internal class GroupOptionsEntry<T> : GroupOptionsEntry
    {
        public GroupOptionsEntry(T options)
        {
            Options = options;
        }

        // TODO - how to identify a group?? A space-delimited string? (Ick!)

        public T Options { get; private set; }

        public override Type OptionType
        {
            get { return Options.GetType(); }
        }

        public override IReflectedObject ReflectedObject
        {
            get { return new ReflectedObject<T>(Options.GetType()); }
        }
    }
}
