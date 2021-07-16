using Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethod
{
    public static class EnumExtension
    {

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        // This extension method is broken out so you can use a similar pattern with 
        // other MetaData elements in the future. This is your base method for each.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }

        // This method creates a specific call to the above method, requesting the
        // Description MetaData attribute.
        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static int? SortOrder(this Enum value)
        {
            var attribute = value.GetAttribute<OrderAttribute>();
            return attribute.Order;
        }

        //public static IEnumerable<TEnum> EnumGetOrderedValues<TEnum>(this Type enumType)
        //{

        //    var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
        //    var orderedValues = new List<Tuple<int, TEnum>>();
        //    foreach (var field in fields)
        //    {
        //        var orderAtt = field.GetCustomAttributes(typeof(OrderAttribute), false).SingleOrDefault() as OrderAttribute;
        //        if (orderAtt != null)
        //        {
        //            orderedValues.Add(new Tuple<int, TEnum>(orderAtt.Order, (TEnum)field.GetValue(null)));
        //        }
        //    }

        //    return orderedValues.OrderBy(x => x.Item1).Select(x => x.Item2).ToList();
        //}
    }
}
