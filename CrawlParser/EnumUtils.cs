using System.ComponentModel;
using System.Reflection;

namespace CrawlParser
{
    /// <summary>
    /// Various extensions for enumerated types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumUtils<T>
    {
        /// <summary>
        /// Get the description associated to the enumerated value.
        /// </summary>
        /// <param name="enumValue">Enumerated value.</param>
        /// <param name="defDesc">Default description to return in the event that one is not defined.</param>
        /// <returns></returns>
        public static string GetDescription(T enumValue, string defDesc)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            FieldInfo fieldInfo; // The enumerated value.

            //
            fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            // Was a field value returned?
            if (null != fieldInfo)
            {
                /*******************************************************
                * CONSTANTS
                * ------------------------------------------------------
                * (none)
                ********************************************************/
                object[] attrs; // The result of an attempt to get the description attribute.

                // Get description attribute.
                attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);

                // If value returned then get description and return.
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return defDesc;
        }

        /// <summary>
        /// Get the description associated to the enumerated value.
        /// </summary>
        /// <param name="enumValue">Enumerated value.</param>
        /// <returns></returns>
        public static string GetDescription(T enumValue)
        {
            return GetDescription(enumValue, string.Empty);
        }
    }
}
