using System;


namespace Yaclops.Attributes
{
    /// <summary>
    /// Decorate a class or property with a long name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class LongNameAttribute : Attribute
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The long name</param>
        public LongNameAttribute(string name)
        {
            Name = name;
        }


        internal string Name { get; private set; }
    }
}
