namespace Common.ToStringConverters
{
    public class BaseToStringConverter<T> : IParamValueToStringConverter
    {
        string IParamValueToStringConverter.Convert(object value)
        {
            return Convert((T) value);
        }

        public virtual string Convert(T value)
        {
            return System.Convert.ToString(value);
        }
    }
}