

namespace Yaclops.DocumentModel
{
    internal class ParagraphStyle
    {
        public ParagraphStyle()
        {
            Tabs = new int[0];
        }

        public int Indent { get; set; }
        public int[] Tabs { get; set; }
    }
}
