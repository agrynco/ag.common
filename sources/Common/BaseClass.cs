using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.ToStringConverters;

namespace Common
{
    public class BaseClass
    {
        public virtual BaseClass Assign(object source)
        {
            var propertyInfos = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute(typeof(IgnoreAttribute)) == null).ToArray();
            var absentProperties = new List<string>();
            foreach (PropertyInfo propertyInfo in propertyInfos)
                try
                {
                    propertyInfo.SetValue(this, PropertyValueManager.GetValue(propertyInfo.Name, source));
                }
                catch (ArgumentException ex)
                {
                    throw new CanNotAssignPropertyException(
                        string.Format("Can not assign property '{0}' from " + "instance of '{1}' to instance of '{2}'",
                            propertyInfo.Name, source.GetType(), GetType()),
                        ex);
                }
                catch (ThereIsNoPropertyException)
                {
                    absentProperties.Add(propertyInfo.Name);
                }

            if (absentProperties.Count > 0) throw new ThereIsNoPropertyException(absentProperties, source);

            return this;
        }

        public override string ToString()
        {
            return ToStringConverter.ConvertClass(this);
        }
    }

    public class BaseClass<T> : BaseClass
        where T : BaseClass<T>
    {
        public static PropertyInfo GetPropertyInfo(string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }
    }
}