using System.ComponentModel;
using Yaclops.Parsing;

namespace Yaclops.Injecting
{
    internal class PropertyInjector
    {
        private readonly ParseResult _result;

        public PropertyInjector(ParseResult result)
        {
            _result = result;
        }


        public void Populate(object target)
        {
            foreach (var named in _result.NamedParameters)
            {
                Push(named, target);
            }

            // TODO - positional parameters
        }



        private void Push(ParserNamedParameterResult param, object target)
        {
            var type = target.GetType();

            var prop = type.GetProperty(param.Parameter.PropertyName);

            if (prop == null)
            {
                throw new CommandLineParserException("Property mismatch: command does not contain property with name '" + param.Parameter.PropertyName + "'.");
            }

            TypeConverter typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
            object propValue = typeConverter.ConvertFromString(param.Value);
            prop.SetValue(target, propValue);
        }
    }
}
