using System.Collections.Generic;

namespace Yaclops.Model
{
    internal abstract class CommandNode
    {
        private readonly List<CommandNamedParameter> _namedParameters = new List<CommandNamedParameter>();

        protected CommandNode(string verb)
        {
            Verb = verb;
        }


        public string Verb { get; private set; }
        public IList<CommandNamedParameter> NamedParameters { get { return _namedParameters; } }
    }
}
