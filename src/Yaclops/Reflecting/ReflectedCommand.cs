using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Attributes;
using Yaclops.Extensions;

#pragma warning disable 618


namespace Yaclops.Reflecting
{
    internal interface IReflectedCommand
    {
        IReadOnlyList<string> Verbs { get; }
        IReadOnlyList<ReflectedNamedParameter> NamedParameters { get; }
    }


    internal interface IReflectedCommand<T> : IReflectedCommand
    {
        Func<T> Factory { get; }
    }



    internal class ReflectedCommand<T> : IReflectedCommand<T>
    {
        private readonly List<string> _verbs = new List<string>();
        private readonly List<ReflectedNamedParameter> _namedParameters = new List<ReflectedNamedParameter>();
        private readonly List<ReflectedPositionalParameter> _positionalParameters = new List<ReflectedPositionalParameter>();


        public ReflectedCommand(Type type, Func<T> factory)
        {
            Factory = factory;

            // TODO - look for attribute to override the name verbs
            _verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());

            // TODO - look for [Group] attribute to help build up verbs
            // TODO - pull out positional parameters
            // TODO - pull out summary and description

            ExtractNamedParameters(type);
            ExtractPositionalParameters(type);
        }



        public IReadOnlyList<string> Verbs { get { return _verbs; } }
        public IReadOnlyList<ReflectedNamedParameter> NamedParameters { get { return _namedParameters; } }
        public IReadOnlyList<ReflectedPositionalParameter> PositionalParameters { get { return _positionalParameters; } }
        public Func<T> Factory { get; private set; }



        private void ExtractPositionalParameters(Type type)
        {
            var props = type.FindProperties<PositionalParameterAttribute>();

            foreach (var prop in props)
            {
                var posParam = new ReflectedPositionalParameter(prop.Name, prop.IsList());
                _positionalParameters.Add(posParam);

                // TODO - pick up the description (if any)
            }
        }



        private void ExtractNamedParameters(Type type)
        {
            var props = type.FindProperties<NamedParameterAttribute>();

            foreach (var prop in props)
            {
                var namedAtt = prop.FindAttribute<NamedParameterAttribute>().First();

                var namedParam = new ReflectedNamedParameter(prop.Name, prop.IsBool());
                _namedParameters.Add(namedParam);

                var longNames = prop.FindAttribute<LongNameAttribute>();
                if (string.IsNullOrEmpty(namedAtt.LongName) && !longNames.Any())
                {
                    namedParam.AddLongName(string.Join("-", prop.Name.Decamel()).ToLower());
                }
                else
                {
                    if (!string.IsNullOrEmpty(namedAtt.LongName))
                    {
                        namedParam.AddLongName(namedAtt.LongName);
                    }

                    foreach (var att in longNames)
                    {
                        namedParam.AddLongName(att.Name);
                    }
                }

                var shortNames = prop.FindAttribute<ShortNameAttribute>();
                if (!string.IsNullOrEmpty(namedAtt.ShortName))
                {
                    namedParam.AddShortName(namedAtt.ShortName);
                }

                foreach (var att in shortNames)
                {
                    namedParam.AddShortName(att.Name);
                }

                // TODO - pick up the description (if any)
            }
        }
    }
}
