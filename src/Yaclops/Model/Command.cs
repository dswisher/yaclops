
namespace Yaclops.Model
{
    internal abstract class Command : CommandNode
    {
        protected Command(string verb) : base(verb)
        {
        }
    }
}
