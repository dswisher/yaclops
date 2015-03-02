using System;

namespace Yaclops
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SummaryAttribute : Attribute
    {

        public SummaryAttribute(string summary)
        {
            Summary = summary;
        }


        public string Summary { get; private set; }
    }
}
