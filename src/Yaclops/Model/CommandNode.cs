
namespace Yaclops.Model
{
    internal abstract class CommandNode
    {
        protected CommandNode(string verb)
        {
            Verb = verb;
        }


        public string Verb { get; private set; }
    }
}
