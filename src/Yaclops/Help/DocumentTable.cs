
using System;
using System.Collections.Generic;

namespace Yaclops.Help
{
    internal class DocumentTable : AbstractDocumentItem
    {
        public override void RenderTo(IConsole target)
        {
            List<int> widths = new List<int>();

            foreach (var item in Items)
            {
                var row = item as DocumentTableRow;
                if (row != null)
                {
                    int index = 0;
                    foreach (var child in row.Items)
                    {
                        var col = child as DocumentTableColumn;
                        if (col != null)
                        {
                            int width = col.Width;

                            if (widths.Count <= index)
                            {
                                widths.Add(width);
                            }
                            else
                            {
                                if (width > widths[index])
                                {
                                    widths[index] = width;
                                }
                            }

                            index += 1;
                        }
                    }
                }
            }

            // TODO - adjust the widths to fit the page

            // TODO - amend the style with the widths

            Console.Write("-> Widths: ");
            for (int i = 0; i < widths.Count; i++)
            {
                if (i > 0)
                {
                    Console.Write("  ");
                }
                Console.Write("{0}:{1}", i + 1, widths[i]);
            }
            Console.WriteLine();

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
