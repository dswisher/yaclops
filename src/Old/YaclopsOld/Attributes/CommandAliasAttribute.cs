using System;


namespace Yaclops.Attributes
{
    /// <summary>
    /// Decorate a command with an alias
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandAliasAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="alias">The alias to add</param>
        public CommandAliasAttribute(string alias)
        {
            Alias = alias;
        }


        internal string Alias { get; private set; }
    }
}
