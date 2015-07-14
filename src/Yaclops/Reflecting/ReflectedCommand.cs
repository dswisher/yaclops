using System;
using System.Collections.Generic;
using Yaclops.Attributes;
using Yaclops.Extensions;


namespace Yaclops.Reflecting
{
    internal interface IReflectedCommand : IReflectedObject
    {
        IReadOnlyList<string> Verbs { get; }
    }


    internal interface IReflectedCommand<T> : IReflectedCommand
    {
        Func<T> Factory { get; }
        string Summary { get; }
    }



    internal class ReflectedCommand<T> : ReflectedObject, IReflectedCommand<T>
    {
        private readonly List<string> _verbs = new List<string>();


        public ReflectedCommand(Type type, Func<T> factory) : base(type)
        {
            Factory = factory;

            var nameAttribute = type.FindAttribute<LongNameAttribute>();
            if (nameAttribute != null)
            {
                _verbs.AddRange(nameAttribute.Name.Split(' '));
            }
            else
            {
                _verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());
            }

            // TODO - look for new [Group] attribute to help build up verbs

            var summaryAtt = type.FindAttribute<SummaryAttribute>();
            if (summaryAtt != null)
            {
                Summary = summaryAtt.Summary;
            }

            // TODO - extract description
        }



        public IReadOnlyList<string> Verbs { get { return _verbs; } }
        public Func<T> Factory { get; private set; }

        public string Summary { get; private set; }
    }
}
