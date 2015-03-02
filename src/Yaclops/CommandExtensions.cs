
namespace Yaclops
{
    internal static class CommandExtensions
    {
        public static string Name(this ISubCommand command)
        {
            return command.GetType().Name.Decamel();
        }



        public static string[] AsWords(this ISubCommand command)
        {
            return command.Name().Split(' ');
        }
    }
}
