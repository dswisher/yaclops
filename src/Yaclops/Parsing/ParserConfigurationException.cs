using System;

namespace Yaclops.Parsing
{
    public class ParserConfigurationException : Exception
    {

        public ParserConfigurationException(string format, params object[] args)
            : base(string.Format(format, args))
        {            
        }

    }
}
