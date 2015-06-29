using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaclops.Model
{
    internal abstract class GroupOptionsEntry
    {
    }



    internal class GroupOptionsEntry<T> : GroupOptionsEntry
    {
        public GroupOptionsEntry(T options)
        {
            Options = options;
        }

        // TODO - how to identify a group?? A space-delimited string? (Ick!)

        public T Options { get; private set; }
    }
}
