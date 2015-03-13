using System.Collections.Generic;

namespace Yaclops.Help
{
    internal abstract class AbstractDocumentItem
    {
        protected AbstractDocumentItem()
        {
            Items = new List<AbstractDocumentItem>();            
        }


        public abstract void RenderTo(IConsole target);


        public void Add(AbstractDocumentItem item)
        {
            Items.Add(item);
            // TODO - should be two base classes - one for containers and one for non-containers?
        }


        protected virtual bool CanAdd(AbstractDocumentItem item)
        {
            return true;
        }


        public List<AbstractDocumentItem> Items { get; private set; }
    }
}
