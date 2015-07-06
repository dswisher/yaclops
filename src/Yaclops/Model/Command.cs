
namespace Yaclops.Model
{
    internal abstract class Command : CommandNode
    {
        protected Command(CommandNode parent, string verb)
            : base(parent, verb)
        {
        }
    }
}
