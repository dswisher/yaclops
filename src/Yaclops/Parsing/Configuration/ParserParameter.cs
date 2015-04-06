using System.Collections.Generic;
using System.Linq;

namespace Yaclops.Parsing.Configuration
{
    internal abstract class ParserParameter
    {
        private readonly HashSet<string> _shortNames = new HashSet<string>();
        private readonly HashSet<string> _longNames = new HashSet<string>();
        private bool _longNameIsDefault;

        protected ParserParameter(string key)
        {
            Key = key;

            // Set default long name
            _longNames.Add(key.Decamel('-').ToLower());
            _longNameIsDefault = true;
        }

        public string Key { get; private set; }
        public string Name { get { return _longNames.First(); } }
        public string Description { get; set; }

        public IEnumerable<string> LongNames { get { return _longNames; } }
        public IEnumerable<string> ShortNames { get { return _shortNames; } }

        public bool HasShortName(string text)
        {
            return _shortNames.Contains(text);
        }

        public bool HasLongName(string text)
        {
            return _longNames.Contains(text);
        }

        public ParserParameter AddShortName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _shortNames.Add(name);
            }
            return this;
        }

        public ParserParameter AddLongName(string name)
        {
            if (_longNameIsDefault)
            {
                _longNameIsDefault = false;
                _longNames.Clear();
            }

            _longNames.Add(name);

            return this;
        }
    }
}
