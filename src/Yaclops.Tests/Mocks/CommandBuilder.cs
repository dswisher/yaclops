using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yaclops.Model;

namespace Yaclops.Tests.Mocks
{
    internal class CommandBuilder
    {
        private readonly Func<object> _factory = () => new object();
        private Command _lastCommand;

        public CommandBuilder()
        {
            Root = new CommandRoot();
        }


        public CommandRoot Root { get; private set; }


        public CommandBuilder ExternalCommand(string verb)
        {
            _lastCommand = Root.AddExternalCommand(verb, _factory);
            return this;
        }


        public CommandBuilder WithNamedBool(string name)
        {
            var param = new CommandNamedParameter(name, true);
            param.ShortNames.Add(name.Substring(0, 1).ToLower());
            param.LongNames.Add(name);
            _lastCommand.NamedParameters.Add(param);
            return this;
        }


        public CommandBuilder WithPositionalString(string name)
        {
            var param = new CommandPositionalParameter(name, false);
            _lastCommand.PositionalParameters.Add(param);
            return this;
        }
    }
}
