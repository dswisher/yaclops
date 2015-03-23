using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Yaclops.Models;

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

            var desc = att.Description;

            // Strip off leading newline, if it has one
            if (desc.StartsWith(Environment.NewLine))
            {
                desc = desc.Substring(Environment.NewLine.Length);
            }

            return desc;
        }



        public static string[] AsWords(this ISubCommand command)
        {
            return command.Name().Split(' ');
        }



        private static string Description(this PropertyInfo property)
        {
            var att = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();

            if (att == null)
            {
                return null;
            }

            return att.Description;
        }



        public static IEnumerable<NamedParameterEntry> NamedParameters(this ISubCommand command)
        {
            return command.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof (CommandLineOptionAttribute), true).Any())
                .Select(x => new NamedParameterEntry
                {
                    Property = x,
                    Attribute = (CommandLineOptionAttribute) x.GetCustomAttributes(typeof (CommandLineOptionAttribute), true).First(),
                    Description = x.Description()
                });
        }



        public static IEnumerable<PositionalParameterEntry> PositionalParameters(this ISubCommand command)
        {
            return command.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof (CommandLineParameterAttribute), true).Any())
                .Select(x => new PositionalParameterEntry
                {
                    Property = x,
                    Attribute = (CommandLineParameterAttribute)x.GetCustomAttributes(typeof(CommandLineParameterAttribute), true).First(),
                    Required = x.GetCustomAttributes(typeof(RequiredAttribute), true).Any()
                });
        }
    }
}
