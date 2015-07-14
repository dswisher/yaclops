using Sample.Commands;
using Yaclops.Attributes;

namespace Sample.Abstractions
{
    // Test case to make sure abstract classes do not get picked up by parser builder
    public abstract class AbstractCommand : ISampleCommand
    {
        [NamedParameter]
        public bool Test { get; set; }
    }
}
