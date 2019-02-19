#region Usings
using System;
#endregion

namespace Common.ToStringConverters
{
    public class DBNullToStringConverter : BaseToStringConverter<DBNull>
    {
        public override string Convert(DBNull value)
        {
            return string.Empty;
        }
    }
}