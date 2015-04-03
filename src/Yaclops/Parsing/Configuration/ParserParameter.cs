using System.Collections.Generic;

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

        public bool HasShortName(string text)
        {
            return _shortNames.Contains(text);
        }

        public bool HasLongName(string text)
        {
            return _longNames.Contains(text);
        }

        public ParserParameter ShortName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _shortNames.Add(name);
            }
            return this;
        }

        public ParserParameter LongName(string name)
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
