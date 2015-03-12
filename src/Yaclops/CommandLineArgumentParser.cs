using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Yaclops
{
    internal class CommandLineArgumentParser
    {
        private readonly ISubCommand _command;
        private readonly ArgumentList _args;
        private readonly Stack<PropertyInfo> _positionalParameters = new Stack<PropertyInfo>();
        private readonly Dictionary<string, PropertyInfo> _namedParameters = new Dictionary<string, PropertyInfo>(); 


        internal static void Parse(ISubCommand command, ArgumentList args)
        {
            var parser = new CommandLineArgumentParser(command, args);

            parser.Parse();
            parser.Verify();
        }



        private CommandLineArgumentParser(ISubCommand command, ArgumentList args)
        {
            _command = command;
            _args = args;

            var type = command.GetType();

            // Push all positional parameters on a stack in reverse order
            var props = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(CommandLineParameterAttribute), true).Any())
                .Reverse();

            foreach (var p in props)
            {
                _positionalParameters.Push(p);
            }

            // Add all named parameters to a list
            var namedProps = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(CommandLineOptionAttribute), true).Any());

            foreach (var p in namedProps)
            {
                var option = (CommandLineOptionAttribute)p.GetCustomAttributes(typeof(CommandLineOptionAttribute), true).First();
                if (!string.IsNullOrWhiteSpace(option.ShortName))
                {
                    _namedParameters.Add(option.ShortName, p);
                    // TODO - if no short name is specified, should we use the first character as a default?
                }

                string name;
                if (!string.IsNullOrEmpty(option.LongName))
                {
                    // TODO - what about case-sensitive names?? All this .ToLower() business may be bad...
                    name = "-" + option.LongName.ToLower();
                }
                else
                {
                    name = "-" + p.Name.Decamel('-').ToLower();
                }

                _namedParameters.Add(name, p);
            }
        }



        private void Parse()
        {
            while (_args.HasRemaining())
            {
                if (_args.Peek().StartsWith("-"))
                {
                    string option = _args.Pop();
                    string name = option.Substring(1);
                    string key = name.ToLower();

                    if (_namedParameters.ContainsKey(key))
                    {
                        var prop = _namedParameters[key];

                        string val = GetNamedValue(prop);

                        SetValue(prop, val);
                    }
                    else
                    {
                        throw new CommandLineParserException("Unknown option: '{0}').", option);
                    }
                }
                else
                {
                    // Positional parameter
                    if (_positionalParameters.Count == 0)
                    {
                        throw new CommandLineParserException("Too many parameters.");
                    }

                    var prop = _positionalParameters.Peek();
                    var val = _args.Pop();

                    // If a collection, add, otherwise set...
                    if (!CanAddTo(prop, val))
                    {
                        SetValue(prop, val);
                        _positionalParameters.Pop();
                    }
                }
            }
        }



        private string GetNamedValue(PropertyInfo prop)
        {
            if (prop.PropertyType == typeof (bool))
            {
                // TODO - peek at the next arg to see if it is the value (depending on type)
                return "true";
            }

            if (!_args.HasRemaining())
            {
                throw new CommandLineParserException("Must specify a value for named parameter: '{0}'", prop.Name);
            }

            return _args.Pop();
        }



        private bool CanAddTo(PropertyInfo prop, string value)
        {
            // TODO - need better handling for collections! Shouldn't be limited to List<string>
            if (prop.PropertyType == typeof (List<string>))
            {
                ((List<string>)prop.GetValue(_command)).Add(value);
                return true;
            }

            return false;
        }



        private void SetValue(PropertyInfo prop, string value)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
            object propValue = typeConverter.ConvertFromString(value);
            prop.SetValue(_command, propValue);
        }



        private void Verify()
        {
            while (_positionalParameters.Count > 0)
            {
                var p = _positionalParameters.Pop();

                if (IsRequired(p))
                {
                    throw new CommandLineParserException("You must specify a value for the '{0}' parameter.", p.Name);
                }
            }
        }


        private bool IsRequired(PropertyInfo prop)
        {
            return prop.GetCustomAttributes(typeof(RequiredAttribute), true).Any();
        }
    }
}
