using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Yaclops.Parsing;

namespace Yaclops.Reflecting
{
    /// <summary>
    /// Push parse results onto an ISubCommand instance.
    /// </summary>
    /// <remarks>
    /// This is the bridge between ParseResult and an ISubCommand instance. It populates
    /// the properties of an ISubCommand based on the results of the parse.
    /// </remarks>
    internal class CommandPusher
    {
        private readonly ParseResult _result;


        public CommandPusher(ParseResult result)
        {
            _result = result;
        }


        public void Push(ISubCommand command)
        {
            // WriteDebugInfo();

            var type = command.GetType();

            // TODO - reset all values to their defaults?
            // TODO - initialize collections to empty?

            foreach (var item in _result.CommandValues)
            {
                var prop = type.GetProperty(item.Name);

                if (prop == null)
                {
                    throw new CommandLineParserException("Property mismatch: command does not contain property with name '" + item.Name + "'.");
                }

                TypeConverter typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
                object propValue = typeConverter.ConvertFromString(item.Value);
                prop.SetValue(command, propValue);
            }

            foreach (var item in _result.CommandListValues)
            {
                var prop = type.GetProperty(item.Name);

                if (prop == null)
                {
                    throw new CommandLineParserException("Property mismatch: command does not contain property with name '" + item.Name + "'.");
                }

                if (prop.PropertyType == typeof (List<string>))
                {
                    var list = ((List<string>)prop.GetValue(command));

                    // TODO - if collections are being initialized, we can skip this check
                    if (list == null)
                    {
                        list = new List<string>();
                        prop.SetValue(command, list);
                    }

                    list.AddRange(item.Values);
                }
                else
                {
                    throw new NotImplementedException("Collections other than List<string> are not yet handled.");
                }
            }
        }



        private void WriteDebugInfo()
        {
            Console.WriteLine("Debug info:");

            Console.WriteLine("Global Values:");
            if (_result.GlobalValues.Any())
            {
                foreach (var item in _result.GlobalValues)
                {
                    Console.WriteLine("   {0} = '{1}'", item.Name, item.Value);
                }
            }
            else
            {
                Console.WriteLine("   (none)");
            }

            Console.WriteLine("Command Values:");
            if (_result.CommandValues.Any())
            {
                foreach (var item in _result.CommandValues)
                {
                    Console.WriteLine("   {0} = '{1}'", item.Name, item.Value);
                }
            }
            else
            {
                Console.WriteLine("   (none)");
            }

            Console.WriteLine("Command List Values:");
            if (_result.CommandListValues.Any())
            {
                foreach (var item in _result.CommandListValues)
                {
                    Console.WriteLine("   {0} = {1}", item.Name, string.Join(", ", item.Values.Select(x => "'" + x + "'")));
                }
            }
            else
            {
                Console.WriteLine("   (none)");
            }
        }
    }
}
