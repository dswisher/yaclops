using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Show various types of objects")]
    public class ShowCommand : ISampleCommand
    {
        [NamedParameter]
        public string Format { get; set; }
    }
}
