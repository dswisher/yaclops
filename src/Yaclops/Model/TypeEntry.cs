using System;


namespace Yaclops.Model
{
    internal class TypeEntry<T>
    {
        public Type Type { get; set; }
        public Func<T> Factory { get; set; }
    }
}
