using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tongyu.Smart.Client.ViewModels.Setting;

namespace Tongyu.Smart.Models
{
    public static class EnumExtension
    {
        public static List<EnumDescription<T>> GetEnumList<T>()
        {
            var list = new List<EnumDescription<T>>();
            Type type = typeof(T);
            foreach (T item in Enum.GetValues(type))
            {
                var valueText = item.ToString();
                var descText = item.ToString();
                var field = type.GetField(descText);
                if (field != null)
                {
                    var browsableAttr = Attribute.GetCustomAttribute(field, typeof(BrowsableAttribute)) as BrowsableAttribute;
                    if (browsableAttr != null && !browsableAttr.Browsable)
                        continue;
                    var valueAttr = Attribute.GetCustomAttribute(field, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                    if (valueAttr != null)
                        valueText = valueAttr.Value.ToString();
                    var descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (descAttr != null)
                        descText = descAttr.Description;
                }
                list.Add(new EnumDescription<T>() { Item = item, Value = valueText, Description = descText });
            }
            return list;
        }
        /// <summary>
        /// 字符串表示转换成等效的枚举对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要转换的枚举名称或基础值的字符串表示形式</param>
        /// <param name="result">由 value 表示的 TEnum 类型的对象</param>
        /// <returns></returns>
        public static bool ToEnum<T>(string value, out T result) where T : struct
        {
            return Enum.TryParse<T>(value, true, out result);
        }
    }
}
