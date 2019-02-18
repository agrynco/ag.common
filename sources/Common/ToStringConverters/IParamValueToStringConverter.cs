#region Usings
#endregion

#region Usings
#endregion

namespace Common.ToStringConverters
{
    public interface IParamValueToStringConverter
    {
        #region Abstract Methods
        string Convert(object value);
        #endregion
    }
}