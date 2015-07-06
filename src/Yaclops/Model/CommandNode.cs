using System.Collections.Generic;

namespace Yaclops.Model
{
    internal abstract class CommandNode
    {
        private readonly List<CommandNamedParameter> _namedParameters = new List<CommandNamedParameter>();
        private readonly List<CommandPositionalParameter> _positionalParameters = new List<CommandPositionalParameter>();

        protected CommandNode(string verb)
        {
            Verb = verb;
        }


        public string Verb { get; private set; }
        public IList<CommandNamedParameter> NamedParameters { get { return _namedParameters; } }
        public IList<CommandPositionalParameter> PositionalParameters { get { return _positionalParameters; } }

        public string Summary { get; set; }
    }
}
