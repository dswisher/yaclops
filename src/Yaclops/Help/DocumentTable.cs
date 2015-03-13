
namespace Yaclops.Help
{
    internal class DocumentTable : AbstractDocumentItem
    {
        public override void RenderTo(IConsole target)
        {
            // TODO - go through the rows and determine the width of each column

            foreach (var item in Items)
            {
                // TODO - how to pass column widths down?
                item.RenderTo(target);
            }
        }


        protected override bool CanAdd(AbstractDocumentItem item)
        {
            return item.GetType() == typeof(DocumentTableRow);
        }
    }
}
