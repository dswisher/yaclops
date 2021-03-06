﻿using System;
using System.Linq;
using System.Text;
using Yaclops.Extensions;
using Yaclops.Help;

namespace Yaclops.Formatting
{
    internal class ConsoleFormatter
    {
        private readonly IConsole _console;
        private int _position;
        private bool _needSeparator;



        public ConsoleFormatter(IConsole console)
        {
            _console = console;
        }



        public void Format(Document doc)
        {
            foreach (var item in doc.Children)
            {
                int indent = item.Style.Indent + item.Style.FirstLineIndent;

                foreach (var block in item.Children)
                {
                    if (string.IsNullOrEmpty(block.Text))
                    {
                        continue;
                    }

                    var words = block.Text.SplitText();

                    foreach (var word in words)
                    {
                        if (word == "\t")
                        {
                            var spaces = item.Style.Tabs.FirstOrDefault(x => x > _position) - _position - 1;
                            if (spaces > 0)
                            {
                                // TODO - what if the tab stop is greater than the width?
                                _console.Write(new string(' ', spaces));
                                _position += spaces;
                                _needSeparator = false;
                            }

                            continue;
                        }

                        if (_position + word.Length + (_needSeparator ? 1 : 0) > _console.Width)
                        {
                            NewLine();
                            indent = item.Style.Indent;
                            // TODO - indent; need to handle edge case of indent + first word > width
                        }
                        else if (_needSeparator)
                        {
                            Whitespace(1);
                        }

                        WriteText(word, indent);
                    }
                }

                NewLine();
            }
        }



        private void Whitespace(int length)
        {
            _console.Write(new string(' ', length));
            _position += length;
        }



        private void WriteText(string text, int indent)
        {
            if (_position < indent)
            {
                _console.Write(new string(' ', indent - _position));
                _position = indent;
            }

            // TODO - handle indent!
            _console.Write(text);
            _needSeparator = true;
            _position += text.Length;
        }



        private void NewLine()
        {
            _console.WriteLine();
            _position = 0;
            _needSeparator = false;
        }
    }
}
