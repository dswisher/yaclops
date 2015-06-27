using System;
using System.Collections.Generic;
using Yaclops.Extensions;

namespace Yaclops.Reflecting
{
    internal class ReflectedCommand<T>
    {
        private readonly Func<T> _factory;


        public ReflectedCommand(Type type, Func<T> factory)
        {
            _factory = factory;
            Verbs = new List<string>();

            // TODO - look for attribute to override the name verbs
            Verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());

            // TODO - look for [Group] attribute to help build up verbs
            // TODO - pick apart the options and flags
            // TODO - pull out summary and description
        }


        public List<string> Verbs { get; private set; }
    }
}
