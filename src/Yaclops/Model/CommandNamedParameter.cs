﻿using System.Collections.Generic;


namespace Yaclops.Model
{
    internal class CommandNamedParameter
    {
        private readonly List<string> _longNames = new List<string>();
        private readonly List<string> _shortNames = new List<string>();

        public CommandNamedParameter(string propertyName, bool isBool)
        {
            PropertyName = propertyName;
            IsBool = isBool;
        }

        public string PropertyName { get; private set; }
        public bool IsBool { get; private set; }
        public List<string> LongNames { get { return _longNames; } }
        public List<string> ShortNames { get { return _shortNames; } }
    }
}