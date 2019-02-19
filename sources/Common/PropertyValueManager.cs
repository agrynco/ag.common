using System;
using System.Reflection;

namespace Common
{
    public class PropertyValueManager
    {
        public static readonly char PROPERTY_SEPARATOR = '.';

        public static object GetValue(string fullPropertyName, object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Parameter 'source' must be different from null");
            }

            PropertyInfoAndObj propertyInfoAndObj =
                GetPropertyPropertyInfoAndObjForFinalProperty(source, fullPropertyName);
            return propertyInfoAndObj.PropertyInfo.GetValue(propertyInfoAndObj.OwnerOfProperty, null);
        }

        private static PropertyInfoAndObj GetPropertyPropertyInfoAndObjForFinalProperty(object obj,
            string fullPropertyName)
        {
            object referenceToOwnerOfProperty = GetReferenceToPropertyOwner(fullPropertyName, obj);
            var pathToPropertyParts = fullPropertyName.Split(PROPERTY_SEPARATOR);

            string newPropertyName = pathToPropertyParts.Length > 1
                ? pathToPropertyParts[pathToPropertyParts.Length - 1]
                : fullPropertyName;

            PropertyInfo propertyInfo = referenceToOwnerOfProperty.GetType().GetProperty(newPropertyName);

            if (propertyInfo == null)
            {
                throw new ThereIsNoPropertyException(fullPropertyName, obj);
            }

            return new PropertyInfoAndObj(propertyInfo, referenceToOwnerOfProperty);
        }

        private static object GetReferenceToPropertyOwner(string pathToProperty, object source)
        {
            var pathToPropertyParts = pathToProperty.Split(PROPERTY_SEPARATOR);
            if (pathToPropertyParts.Length > 1)
            {
                string ownerOfProperty = string.Join(PROPERTY_SEPARATOR.ToString(), pathToPropertyParts, 0,
                    pathToPropertyParts.Length - 1);

                return GetValue(ownerOfProperty, source);
            }

            return source;
        }

        internal class PropertyInfoAndObj
        {
            public PropertyInfoAndObj(PropertyInfo propertyInfo, object ownerOfProperty)
            {
                PropertyInfo = propertyInfo;
                OwnerOfProperty = ownerOfProperty;
            }

            public object OwnerOfProperty { get; }

            public PropertyInfo PropertyInfo { get; }

            /// <summary>
            /// Sets the <see cref="value"/> to the <see cref="fullPropertyName"/>
            /// </summary>
            /// <param name="destination">Object which contains the property with <see cref="fullPropertyName"/></param>
            /// <param name="fullPropertyName"> name of the property to be changed</param>
            /// <param name="value">Value of the property to be setted</param>
            public static void SetValue(object destination, string fullPropertyName, object value)
            {
                object valueToSet = value == DBNull.Value ? null : value;

                PropertyInfoAndObj propertyInfoAndObj =
                    GetPropertyPropertyInfoAndObjForFinalProperty(destination, fullPropertyName);

                propertyInfoAndObj.PropertyInfo.SetValue(propertyInfoAndObj.OwnerOfProperty, valueToSet, null);
            }
        }
    }
}