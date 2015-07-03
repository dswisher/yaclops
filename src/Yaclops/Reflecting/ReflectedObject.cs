using System;
using System.Collections.Generic;
using System.Linq;
using Yaclops.Attributes;
using Yaclops.Extensions;

#pragma warning disable 618


namespace Yaclops.Reflecting
{
    internal interface IReflectedObject
    {
        IReadOnlyList<ReflectedNamedParameter> NamedParameters { get; }
    }



    internal class ReflectedObject<T> : IReflectedObject
    {

        private readonly List<ReflectedNamedParameter> _namedParameters = new List<ReflectedNamedParameter>();



        public ReflectedObject(Type type)
        {
            ExtractNamedParameters(type);

            // TODO - pull out summary and description
        }



        public IReadOnlyList<ReflectedNamedParameter> NamedParameters { get { return _namedParameters; } }



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
