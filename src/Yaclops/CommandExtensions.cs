using System.Linq;

namespace Yaclops
{
    internal static class CommandExtensions
    {
        public static string Name(this ISubCommand command)
        {
            return command.GetType().Name.Decamel();
        }



        public static string Summary(this ISubCommand command)
        {
            var att = (SummaryAttribute)command.GetType().GetCustomAttributes(typeof(SummaryAttribute), true).FirstOrDefault();
            if (att == null)
            {
                return string.Empty;
            }

            return att.Summary;
        }



        public static string[] AsWords(this ISubCommand command)
        {
            return command.Name().Split(' ');
        }
    }
}
