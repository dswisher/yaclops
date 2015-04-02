using System;
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
            Console.WriteLine("CommandPusher.Push is not yet implemented.");
            WriteDebugInfo();

            var type = command.GetType();

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
        }
    }
}
