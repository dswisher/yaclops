using System;
using System.Collections.Generic;
using System.Linq;
using CommonMark.Syntax;

namespace Yaclops.Markdown.ConsoleMarkup
{
    internal class AstConverter
    {
        private readonly List<MarkupItem> _items = new List<MarkupItem>();


        public IEnumerable<MarkupItem> Convert(Block block)
        {
            var stack = new Stack<Block>();

            while (block != null)
            {
                switch (block.Tag)
                {
                    case BlockTag.Document:
                        break;

                    case BlockTag.AtxHeader:
                        EnsureLine();
                        // TODO - push header style
                        SpewInline(block.InlineContent);
                        // TODO - pop header style
                        break;

                    case BlockTag.List:
                        // TODO - push style or some such? Level - what about nested lists?
                        break;

                    case BlockTag.ListItem:
                        EnsureLine();
                        // TODO - different bullet characters?
                        _items.Add(new TextItem("*"));
                        break;

                    case BlockTag.Paragraph:
                        SpewInline(block.InlineContent);
                        break;

                    default:
                        throw new NotImplementedException("BlockTag '" + block.Tag + "' is not yet implemented.");
                }

                if (block.FirstChild != null)
                {
                    if (block.NextSibling != null)
                    {
                        stack.Push(block.NextSibling);
                    }

                    block = block.FirstChild;
                }
                else if (block.NextSibling != null)
                {
                    block = block.NextSibling;
                }
                else if (stack.Count > 0)
                {
                    block = stack.Pop();
                }
                else
                {
                    block = null;
                }
            }

            return _items;
        }



        private void EnsureLine()
        {
            if ((_items.Count > 0) && (_items.Last().GetType() != typeof(NewlineItem)))
            {
                _items.Add(new NewlineItem());
            }
        }



        private void SpewInline(Inline inline)
        {
            var stack = new Stack<Inline>();

            while (inline != null)
            {
                switch (inline.Tag)
                {
                    case InlineTag.String:
                        // TODO - need to wrap long text, unless we're in a header or something?
                        _items.Add(new TextItem(inline.LiteralContent.Trim()));
                        break;

                    case InlineTag.Code:
                        // TODO - set code style
                        _items.Add(new TextItem(inline.LiteralContent.Trim()));
                        break;

                    default:
                        throw new NotImplementedException("InlineTag '" + inline.Tag + "' is not yet implemented.");
                }

                if (inline.FirstChild != null)
                {
                    if (inline.NextSibling != null)
                    {
                        stack.Push(inline.NextSibling);
                    }

                    inline = inline.FirstChild;
                }
                else if (inline.NextSibling != null)
                {
                    inline = inline.NextSibling;
                }
                else if (stack.Count > 0)
                {
                    inline = stack.Pop();
                }
                else
                {
                    inline = null;
                }
            }
        }
    }
}
