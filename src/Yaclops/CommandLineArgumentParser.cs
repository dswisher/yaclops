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


        internal static void Parse(ISubCommand command, ArgumentList args, bool checkRequired)
        {
            var parser = new CommandLineArgumentParser(command, args);

            parser.Parse();
            parser.Verify(checkRequired);
        }



        private CommandLineArgumentParser(ISubCommand command, ArgumentList args)
        {
            _command = command;
            _args = args;

            // Push all positional parameters on a stack in reverse order
            foreach (var p in command.PositionalParameters().Reverse())
            {
                _positionalParameters.Push(p.Property);
            }

            // Add all named parameters to a list
            foreach (var p in command.NamedParameters())
            {
                if (!string.IsNullOrWhiteSpace(p.Attribute.ShortName))
                {
                    _namedParameters.Add(p.Attribute.ShortName, p.Property);
                    // TODO - if no short name is specified, should we use the first character as a default?
                }

                string name;
                if (!string.IsNullOrEmpty(p.Attribute.LongName))
                {
                    // TODO - what about case-sensitive names?? All this .ToLower() business may be bad...
                    name = "-" + p.Attribute.LongName.ToLower();
                }
                else
                {
                    name = "-" + p.Property.Name.Decamel('-').ToLower();
                }

                _namedParameters.Add(name, p.Property);
            }
        }



        private void Parse()
        {
            bool popList = false;

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
                    if (!AddToCollection(prop, val))
                    {
                        SetValue(prop, val);
                        _positionalParameters.Pop();
                    }
                    else
                    {
                        popList = true;
                    }
                }
            }

            if (popList)
            {
                _positionalParameters.Pop();
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



        private bool AddToCollection(PropertyInfo prop, string value)
        {
            // TODO - need better handling for collections! Shouldn't be limited to List<string>
            if (prop.PropertyType == typeof (List<string>))
            {
                var list = ((List<string>) prop.GetValue(_command));

                if (list == null)
                {
                    list = new List<string>();
                    prop.SetValue(_command, list);
                }

                list.Add(value);

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



        private void Verify(bool checkRequired)
        {
            while (_positionalParameters.Count > 0)
            {
                var p = _positionalParameters.Pop();

                if (checkRequired && IsRequired(p))
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
