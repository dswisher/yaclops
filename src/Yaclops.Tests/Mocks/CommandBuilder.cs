using System;
using Yaclops.Model;

namespace Yaclops.Tests.Mocks
{
    internal class CommandBuilder
    {
        private readonly Func<object> _factory = () => new object();
        private Command _lastCommand;

        public CommandBuilder()
        {
            Root = new CommandRoot("Command");
        }


        public CommandRoot Root { get; private set; }


        public CommandBuilder ExternalCommand(string verb)
        {
            _lastCommand = Root.AddExternalCommand(verb, _factory);
            return this;
        }


        public CommandBuilder WithNamedBool(string name)
        {
            var param = new CommandNamedParameter(name, true, x => x);
            param.ShortNames.Add(name.Substring(0, 1).ToLower());
            param.LongNames.Add(name);
            _lastCommand.NamedParameters.Add(param);
            return this;
        }


        public CommandBuilder WithPositionalString(string name, bool required = false)
        {
            var param = new CommandPositionalParameter(name, false, required, x => x);
            _lastCommand.PositionalParameters.Add(param);
            return this;
        }
    }
}
