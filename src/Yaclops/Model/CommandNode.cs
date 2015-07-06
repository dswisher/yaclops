using System.Collections.Generic;

namespace Yaclops.Model
{
    internal abstract class CommandNode
    {
        private readonly List<CommandNamedParameter> _namedParameters = new List<CommandNamedParameter>();
        private readonly List<CommandPositionalParameter> _positionalParameters = new List<CommandPositionalParameter>();

        protected CommandNode(CommandNode parent, string verb)
        {
            Parent = parent;
            Verb = verb;
        }


        public string Verb { get; private set; }
        public IList<CommandNamedParameter> NamedParameters { get { return _namedParameters; } }
        public IList<CommandPositionalParameter> PositionalParameters { get { return _positionalParameters; } }
        public CommandNode Parent { get; private set; }

        public virtual string HelpVerb { get { return Verb; } }

        public bool Hidden { get; set; }
        public string Summary { get; set; }
    }
}
