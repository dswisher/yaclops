
namespace Yaclops.Help
{
    internal class DocumentTableRow : AbstractDocumentItem
    {
        public override void RenderTo(IConsole target)
        {
            bool first = true;
            foreach (var item in Items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    // TODO - pull separator from style
                    target.Write(" - ");
                }

                item.RenderTo(target);
            }

            target.WriteLine();
        }
    }
}
