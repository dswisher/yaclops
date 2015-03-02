
namespace Yaclops
{
    internal static class CommandExtensions
    {
        public static string Name(this ICommand command)
        {
            return command.GetType().Name.Decamel();
        }



        public static string[] AsWords(this ICommand command)
        {
            return command.Name().Split(' ');
        }
    }
}
