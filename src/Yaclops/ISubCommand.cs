
namespace Yaclops
{
    /// <summary>
    /// The interface that a subcommand must implement.
    /// </summary>
    public interface ISubCommand
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        void Execute();
    }
}
