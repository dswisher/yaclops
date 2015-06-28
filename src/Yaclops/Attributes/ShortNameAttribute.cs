using System;


namespace Yaclops.Attributes
{
    /// <summary>
    /// Decorate a property with a shore name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ShortNameAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The short name</param>
        public ShortNameAttribute(string name)
        {
            Name = name;
        }


        internal string Name { get; private set; }
    }
}
