namespace Common.ToStringConverters
{
    public class BaseToStringConverter<T> : IParamValueToStringConverter
    {
        #region IParamValueToStringConverter Methods
        string IParamValueToStringConverter.Convert(object value)
        {
            return Convert((T) value);
        }
        #endregion

        #region Methods (public)
        public virtual string Convert(T value)
        {
            return System.Convert.ToString(value);
        }
        #endregion
    }
}