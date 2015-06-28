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


        public ReflectedCommand(Type type, Func<T> factory)
        {
            Factory = factory;

            // TODO - look for attribute to override the name verbs
            _verbs.AddRange(type.Name.Replace("Command", string.Empty).Decamel());

            // TODO - look for [Group] attribute to help build up verbs
            // TODO - pull out positional parameters
            // TODO - pull out summary and description

            ExtractNamedParameters(type);
        }



        public IReadOnlyList<string> Verbs { get { return _verbs; } }
        public IReadOnlyList<ReflectedNamedParameter> NamedParameters { get { return _namedParameters; } }
        public Func<T> Factory { get; private set; }



        private void ExtractNamedParameters(Type type)
        {
            var namedProps = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof (NamedParameterAttribute), true).Any());

            foreach (var prop in namedProps)
            {
                var namedAtt = (NamedParameterAttribute)prop.GetCustomAttributes(typeof(NamedParameterAttribute), true).First();

                ReflectedNamedParameter namedParam = new ReflectedNamedParameter(prop.Name, prop.IsBool());
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
