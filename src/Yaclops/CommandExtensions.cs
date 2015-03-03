using System.ComponentModel;
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



        public static string Description(this ISubCommand command)
        {
            // TODO - if this lacks attribute, it returns null. Summary returns empty string. Make 'em consistent.
            // Perhaps have them take a default value?

            var att = (DescriptionAttribute)command.GetType().GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (att == null)
            {
                return null;
            }

            return att.Description;
        }



        public static string[] AsWords(this ISubCommand command)
        {
            return command.Name().Split(' ');
        }
    }
}
