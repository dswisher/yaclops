
using System.Collections.Generic;

namespace Yaclops.Reflecting
{
    internal class ReflectedNamedParameter
    {
        private readonly HashSet<string> _longNames = new HashSet<string>();
        private readonly HashSet<string> _shortNames = new HashSet<string>();


        public ReflectedNamedParameter(string propertyName, bool isBool)
        {
            PropertyName = propertyName;
            IsBool = isBool;
        }


        public string PropertyName { get; private set; }
        public string Description { get; set; }
        public bool IsBool { get; private set; }
        public IEnumerable<string> LongNames { get { return _longNames; } }
        public IEnumerable<string> ShortNames { get { return _shortNames; } }


        public bool HasLongName(string name)
        {
            return _longNames.Contains(name);
        }

        public bool HasShortName(string name)
        {
            return _shortNames.Contains(name);
        }

        public void AddLongName(string name)
        {
            _longNames.Add(name);
        }

        public void AddShortName(string name)
        {
            _shortNames.Add(name);
        }
    }
}
