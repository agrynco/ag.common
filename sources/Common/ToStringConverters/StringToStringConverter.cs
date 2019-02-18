#region Usings
#endregion

#region Usings
#endregion

namespace Common.ToStringConverters
{
    public class StringToStringConverter : BaseToStringConverter<string>
    {
        #region Methods (public)
        public override string Convert(string value)
        {
            if (value != null) value = value.Replace("'", "''");
            return string.Format("'{0}'", value);
        }
        #endregion
    }
}