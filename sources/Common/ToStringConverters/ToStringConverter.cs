#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
#endregion

namespace Common.ToStringConverters
{
    public class ToStringConverter : IParamValueToStringConverter
    {
        #region Static Fields (private)
        private static ToStringConverter _instance;
        #endregion

        #region Fields (private)
        private readonly Dictionary<Type, IParamValueToStringConverter> _converters;
        #endregion

        #region Constructors
        private ToStringConverter()
        {
            _converters = new Dictionary<Type, IParamValueToStringConverter>();
            RegisterAllConverters();
        }
        #endregion

        #region Static Properties (public)
        public static ToStringConverter Instance => _instance ?? (_instance = new ToStringConverter());
        #endregion

        #region IParamValueToStringConverter Methods
        public string Convert(object value)
        {
            if (value == null) return "null";
            Type type = value.GetType();
            type = type.IsEnum ? typeof(Enum) : type;

            if (_converters.ContainsKey(type)) return _converters[type].Convert(value);

            throw new KeyNotFoundException($"Converter for type '{type}' is not registered.");
        }
        #endregion

        #region Static Methods (public)
        public static string ConvertClass(object obj)
        {
            if (obj == null) return Instance.Convert(null);
            var alreadyToStringConverted = new Dictionary<object, object>();
            return ConvertClass(obj, alreadyToStringConverted);
        }
        #endregion

        #region Static Methods (private)
        private static PropertyInfo[] GetPropertiesToBeConverted(Type type)
        {
            var propertyInfos = type.GetProperties();

            return propertyInfos
                .Where(x => x.GetCustomAttributes(typeof(IgnoreConvertToStringAttribute), false).Length == 0).ToArray();
        }

        private static string ConvertClass(object obj, Dictionary<object, object> alreadyToStringConverted)
        {
            if (!alreadyToStringConverted.ContainsKey(obj))
            {
                alreadyToStringConverted.Add(obj, obj);
                var result = new StringBuilder();
                result.Append('(');

                var propertuesToBeConverted = GetPropertiesToBeConverted(obj.GetType());
                foreach (PropertyInfo propertyInfo in propertuesToBeConverted)
                {
                    if (result.Length > 1) result.Append("; ");
                    try
                    {
                        object propertyValue = propertyInfo.GetValue(obj, null);

                        if (propertyValue is IEnumerable && IsNotString(propertyInfo.PropertyType))
                        {
                            result.Append(propertyInfo.Name).Append(" = [");
                            const string item_separator = ", ";
                            foreach (object item in (IEnumerable) propertyValue)
                            {
                                ProcessValue(null, item, result, alreadyToStringConverted);
                                result.Append(item_separator);
                            }

                            if (result.ToString().EndsWith(item_separator))
                                result.Length = result.Length - item_separator.Length;
                            result.Append("]");
                        }
                        else
                        {
                            ProcessValue(propertyInfo.Name, propertyValue, result, alreadyToStringConverted);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.Error(typeof(ToStringConverter), ex.Message);

                        result.Append(result.Append(string.Format("{0} = {1}", propertyInfo.Name, ex.Message)));
                    }
                }

                result.Append(')');
                return result.ToString();
            }

            return "Circular reference";
        }

        private static bool IsClass(object propertyValue)
        {
            return propertyValue != null && !propertyValue.GetType().IsValueType &&
                   IsNotString(propertyValue.GetType());
        }

        private static bool IsNotString(Type propertyType)
        {
            return propertyType != typeof(string);
        }

        private static void ProcessValue(string propertyName, object propertyValue, StringBuilder result,
            Dictionary<object, object> alreadyToStringConverted)
        {
            var valuesForFormattedString = new List<string>();
            string formatPattern;
            if (propertyName == null)
            {
                formatPattern = "{0}";
            }
            else
            {
                formatPattern = "{0} = {1}";
                valuesForFormattedString.Add(propertyName);
            }

            if (IsClass(propertyValue))
                valuesForFormattedString.Add(ConvertClass(propertyValue, alreadyToStringConverted));
            else
                valuesForFormattedString.Add(Instance.Convert(propertyValue));

            result.Append(string.Format(formatPattern, valuesForFormattedString.ToArray()));
        }
        #endregion

        #region Methods (protected)
        protected void Register(Type type, IParamValueToStringConverter converter)
        {
            _converters.Add(type, converter);
        }

        protected virtual void RegisterAllConverters()
        {
            Register(typeof(Guid), new GuidToStringConverter());
            Register(typeof(Int32), new BaseToStringConverter<Int32>());
            Register(typeof(uint), new BaseToStringConverter<uint>());
            Register(typeof(Boolean), new BaseToStringConverter<Boolean>());
            Register(typeof(string), new StringToStringConverter());
            Register(typeof(decimal), new BaseToStringConverter<decimal>());
            Register(typeof(Int64), new BaseToStringConverter<Int64>());
            Register(typeof(DateTime), new BaseToStringConverter<DateTime>());
            Register(typeof(DBNull), new DBNullToStringConverter());
            Register(typeof(int[]), new EnumerableToStringConverter());
            Register(typeof(Enum), new BaseToStringConverter<Enum>());
            Register(typeof(double), new BaseToStringConverter<double>());
            Register(typeof(Char), new BaseToStringConverter<Char>());
            Register(typeof(TimeSpan), new BaseToStringConverter<TimeSpan>());
        }

        protected void Unregister(Type type)
        {
            _converters.Remove(type);
        }
        #endregion
    }
}