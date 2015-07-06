using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

            foreach (var pos in _result.PositionalParameters)
            {
                Push(pos, target);
            }
        }



        private void Push(ParserPositionalParameterResult param, object target)
        {
            var propertyTarget = param.Parameter.PropertyTarget(target);

            var type = propertyTarget.GetType();

            var prop = type.GetProperty(param.Parameter.PropertyName);

            if (prop == null)
            {
                throw new CommandLineParserException("Property mismatch: command does not contain property with name '" + param.Parameter.PropertyName + "'.");
            }

            if (param.Parameter.IsList)
            {
                var listType = typeof(List<>);
                var genericArgs = prop.PropertyType.GetGenericArguments();
                var concreteType = listType.MakeGenericType(genericArgs);

                var list = prop.GetValue(propertyTarget);
                if (list == null)
                {
                    list = Activator.CreateInstance(concreteType);
                    prop.SetValue(propertyTarget, list);
                }

                var add = concreteType.GetMethod("Add");
                TypeConverter typeConverter = TypeDescriptor.GetConverter(genericArgs.First());
                foreach (var stringVal in param.Values)
                {
                    var val = typeConverter.ConvertFromString(stringVal);
                    add.Invoke(list, new[] { val });
                }
            }
            else
            {
                Push(propertyTarget, prop, param.Values.First());
            }
        }



        private void Push(ParserNamedParameterResult param, object target)
        {
            var propertyTarget = param.Parameter.PropertyTarget(target);

            var type = propertyTarget.GetType();

            var prop = type.GetProperty(param.Parameter.PropertyName);

            if (prop == null)
            {
                throw new CommandLineParserException("Property mismatch: command does not contain property with name '" + param.Parameter.PropertyName + "'.");
            }

            Push(propertyTarget, prop, param.Value);
        }



        private void Push(object target, PropertyInfo prop, string value)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
            object propValue = typeConverter.ConvertFromString(value);
            prop.SetValue(target, propValue);
        }
    }
}
