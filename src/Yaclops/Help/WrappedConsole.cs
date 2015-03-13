using System;


namespace Yaclops.Help
{
    /// <summary>
    /// Interface for the bits of the console we use, so we can mock it easily in unit tests
    /// </summary>
    internal interface IConsole
    {
        void Write(string content);

        void WriteLine();
        void WriteLine(string content);
    }


    /// <summary>
    /// IConsole implementation that writes to the console
    /// </summary>
    internal class WrappedConsole : IConsole
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }

        public void Write(string content)
        {
            Console.Write(content);
        }
    }
}
