using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Attributes;
using Yaclops.Extensions;


namespace Yaclops.Reflecting
{
    internal interface IReflectedCommand : IReflectedObject
    {
        IReadOnlyList<string> Verbs { get; }
        IReadOnlyList<ReflectedPositionalParameter> PositionalParameters { get; }
    }


    internal interface IReflectedCommand<T> : IReflectedCommand
    {
        Func<T> Factory { get; }
        string Summary { get; }
    }



    internal class ReflectedCommand<T> : ReflectedObject<T>, IReflectedCommand<T>
    {
        private readonly List<string> _verbs = new List<string>();
        private readonly List<ReflectedPositionalParameter> _positionalParameters = new List<ReflectedPositionalParameter>();


        public ReflectedCommand(Type type, Func<T> factory) : base(type)
        {
            Factory = factory;

            // TODO - look for attribute to override the name verbs
            _verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());

            // TODO - look for [Group] attribute to help build up verbs

            var summaryAtt = type.FindAttribute<SummaryAttribute>();
            if (summaryAtt != null)
            {
                Summary = summaryAtt.Summary;
            }

            // TODO - extract description

            ExtractPositionalParameters(type);
        }



        public IReadOnlyList<string> Verbs { get { return _verbs; } }
        public IReadOnlyList<ReflectedPositionalParameter> PositionalParameters { get { return _positionalParameters; } }
        public Func<T> Factory { get; private set; }

        public string Summary { get; private set; }



        private void ExtractPositionalParameters(Type type)
        {
            var props = type.FindProperties<PositionalParameterAttribute>();

            foreach (var prop in props)
            {
                var posParam = new ReflectedPositionalParameter(prop.Name, prop.IsList(), prop.IsRequired());
                _positionalParameters.Add(posParam);

                // TODO - pick up the description (if any)
            }
        }
    }
}
