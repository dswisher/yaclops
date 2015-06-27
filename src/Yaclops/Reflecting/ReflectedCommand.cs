using System;
using System.Collections.Generic;
using Yaclops.Extensions;

namespace Yaclops.Reflecting
{
    internal interface IReflectedCommand
    {
        IList<string> Verbs { get; }
    }


    internal class ReflectedCommand<T> : IReflectedCommand
    {
        private readonly List<string> _verbs = new List<string>();
        private readonly Func<T> _factory;


        public ReflectedCommand(Type type, Func<T> factory)
        {
            _factory = factory;

            // TODO - look for attribute to override the name verbs
            _verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());

            // TODO - look for [Group] attribute to help build up verbs
            // TODO - pick apart the options and flags
            // TODO - pull out summary and description
        }


        public IList<string> Verbs { get { return _verbs; } }
    }
}
