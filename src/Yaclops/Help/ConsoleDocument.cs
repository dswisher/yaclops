

namespace Yaclops.Help
{
    internal class ConsoleDocument : AbstractDocumentItem
    {
        public override void RenderTo(IConsole target)
        {
            foreach (var item in Items)
            {
                item.RenderTo(target);
            }
        }
    }
}
