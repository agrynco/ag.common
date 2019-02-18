#region Usings
using System;
#endregion

namespace Common.ToStringConverters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreConvertToStringAttribute : Attribute
    {
    }
}