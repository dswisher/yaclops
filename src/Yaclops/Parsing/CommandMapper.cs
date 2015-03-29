using System.Collections.Generic;

namespace Yaclops.Parsing
{

    internal enum MapperState
    {
        Accepted,   // Input string resulted in a command
        Rejected,   // Rejected the input string
        Pending     // Still waiting more input
    }



    internal class CommandMapper
    {
        private readonly Entry _root = new Entry();
        private Entry _current;


        public CommandMapper(IEnumerable<ParserCommand> commands)
        {
            foreach (var command in commands)
            {
                AddCommand(command);
            }

            _current = _root;
            State = MapperState.Pending;
        }



        private void AddCommand(ParserCommand command)
        {
            Queue<string> queue = new Queue<string>();
            foreach (var s in command.Text.Split(' '))
            {
                queue.Enqueue(s);
            }

            _root.Add(queue, command);

            // TODO - add any aliases
        }



        public void Advance(string word)
        {
            if (_current.ContainsCommand(word))
            {
                State = MapperState.Accepted;
                Command = _current.GetCommand(word);
            }
            else if (_current.ContainsChild(word))
            {
                _current = _current.GetChild(word);
            }
            else
            {
                State = MapperState.Rejected;
            }
        }


        public MapperState State { get; private set; }
        public ParserCommand Command { get; private set; }


        private class Entry
        {
            private readonly Dictionary<string, ParserCommand> _commands = new Dictionary<string, ParserCommand>();
            private readonly Dictionary<string, Entry> _entries = new Dictionary<string, Entry>();

            public void Add(Queue<string> words, ParserCommand command)
            {
                string word = words.Dequeue();
                if (words.Count == 0)
                {
                    if (_entries.ContainsKey(word))
                    {
                        throw new ParserConfigurationException("Ambiguous command: {0}", word);
                    }

                    _commands.Add(word, command);
                }
                else
                {
                    if (_commands.ContainsKey(word))
                    {
                        throw new ParserConfigurationException("Ambiguous command: {0}", word);
                    }

                    Entry child;
                    if (!_entries.TryGetValue(word, out child))
                    {
                        child = new Entry();
                        _entries.Add(word, child);
                    }

                    child.Add(words, command);
                }
            }


            public bool ContainsCommand(string word)
            {
                return _commands.ContainsKey(word);
            }


            public bool ContainsChild(string word)
            {
                return _entries.ContainsKey(word);
            }


            public ParserCommand GetCommand(string word)
            {
                return _commands[word];
            }


            public Entry GetChild(string word)
            {
                return _entries[word];
            }
        }
    }
}
