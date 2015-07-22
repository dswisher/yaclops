using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Yaclops.Attributes;
using Yaclops.Extensions;

#pragma warning disable 618


namespace Yaclops.Reflecting
{
    internal interface IReflectedObject
    {
        IReadOnlyList<ReflectedNamedParameter> NamedParameters { get; }
        IReadOnlyList<ReflectedPositionalParameter> PositionalParameters { get; }

    }



    internal class ReflectedObject : IReflectedObject
    {
        private readonly List<ReflectedNamedParameter> _namedParameters = new List<ReflectedNamedParameter>();
        private readonly List<ReflectedPositionalParameter> _positionalParameters = new List<ReflectedPositionalParameter>();



        public ReflectedObject(Type type)
        {
            ExtractNamedParameters(type);
            ExtractPositionalParameters(type);

            // TODO - pull out summary and description
        }



        public IReadOnlyList<ReflectedNamedParameter> NamedParameters { get { return _namedParameters; } }
        public IReadOnlyList<ReflectedPositionalParameter> PositionalParameters { get { return _positionalParameters; } }



        private void ExtractNamedParameters(Type type)
        {
            var props = type.FindProperties<NamedParameterAttribute>();

            var shortHash = new HashSet<string>();
            var longHash = new HashSet<string>();

            foreach (var prop in props)
            {
                var namedAtt = prop.FindAttribute<NamedParameterAttribute>().First();

                var namedParam = new ReflectedNamedParameter(prop.Name, prop.IsBool());
                _namedParameters.Add(namedParam);

                var descrip = prop.FindAttribute<DescriptionAttribute>().FirstOrDefault();
                if (descrip != null)
                {
                    namedParam.Description = descrip.Description;
                }

                var longNames = prop.FindAttribute<LongNameAttribute>();
                if (string.IsNullOrEmpty(namedAtt.LongName) && !longNames.Any())
                {
                    string name = string.Join("-", prop.Name.Decamel());
                    DupCheck(type, longHash, name, false);
                    namedParam.AddLongName(name);
                }
                else
                {
                    if (!string.IsNullOrEmpty(namedAtt.LongName))
                    {
                        DupCheck(type, longHash, namedAtt.LongName, false);
                        namedParam.AddLongName(namedAtt.LongName);
                    }

                    foreach (var att in longNames)
                    {
                        DupCheck(type, longHash, att.Name, false);
                        namedParam.AddLongName(att.Name);
                    }
                }

                var shortNames = prop.FindAttribute<ShortNameAttribute>();
                if (!string.IsNullOrEmpty(namedAtt.ShortName))
                {
                    DupCheck(type, shortHash, namedAtt.ShortName, true);
                    namedParam.AddShortName(namedAtt.ShortName);
                }

                foreach (var att in shortNames)
                {
                    DupCheck(type, shortHash, att.Name, true);
                    namedParam.AddShortName(att.Name);
                }

                // TODO - pick up the description (if any)
            }
        }



        private void DupCheck(Type type, HashSet<string> hash, string name, bool shortName)
        {
            if (hash.Contains(name))
            {
                throw new CommandLineParserException("Type {0} contains duplicate {1} name: {2}",
                    type.Name, shortName ? "short" : "long", name);
            }

            hash.Add(name);
        }



        private void ExtractPositionalParameters(Type type)
        {
            var props = type.FindProperties<PositionalParameterAttribute>();

            foreach (var prop in props)
            {
                var posParam = new ReflectedPositionalParameter(prop.Name, prop.IsList() || prop.IsHashSet(), prop.IsRequired());
                _positionalParameters.Add(posParam);

                var descrip = prop.FindAttribute<DescriptionAttribute>().FirstOrDefault();
                if (descrip != null)
                {
                    posParam.Description = descrip.Description;
                }
            }
        }
    }
}
