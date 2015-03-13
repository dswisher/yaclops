using System;
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
            if (!CanAdd(item))
            {
                throw new InvalidOperationException(string.Format("Help build error: cannnot add type {0} to {1}.", item.GetType().Name, GetType().Name));
            }

            Items.Add(item);
            // TODO - should there be two base classes - one for containers and one for non-containers?
        }


        protected virtual bool CanAdd(AbstractDocumentItem item)
        {
            return true;
        }


        public List<AbstractDocumentItem> Items { get; private set; }
    }
}
