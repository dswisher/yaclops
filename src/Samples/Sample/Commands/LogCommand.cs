using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Show commit logs")]
    public class LogCommand : ISampleCommand
    {

        [NamedParameter, ShortName("v")]
        public bool? Verbose { get; set; }

    }
}
