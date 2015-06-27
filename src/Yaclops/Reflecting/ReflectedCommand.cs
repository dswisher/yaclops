using System;
using System.Collections.Generic;
using Yaclops.Extensions;

namespace Yaclops.Reflecting
{
    internal interface IReflectedCommand
    {
        IList<string> Verbs { get; }
    }


    internal interface IReflectedCommand<T> : IReflectedCommand
    {
        Func<T> Factory { get; }
    }



    internal class ReflectedCommand<T> : IReflectedCommand<T>
    {
        private readonly List<string> _verbs = new List<string>();


        public ReflectedCommand(Type type, Func<T> factory)
        {
            Factory = factory;

            // TODO - look for attribute to override the name verbs
            _verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());

            // TODO - look for [Group] attribute to help build up verbs
            // TODO - pick apart the options and flags
            // TODO - pull out summary and description
        }


        public IList<string> Verbs { get { return _verbs; } }
        public Func<T> Factory { get; private set; }
    }
}
