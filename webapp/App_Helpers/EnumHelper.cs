using LivellPayRoll.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace LivellPayRoll.App_Helpers
{
    public class EnumHelper
    {
        /// <summary>  
        /// 枚举转字典集合  
        /// </summary>  
        /// <typeparam name="T">枚举类名称</typeparam>  
        /// <param name="keyDefault">默认key值</param>  
        /// <param name="valueDefault">默认value值</param>  
        /// <returns>返回生成的字典集合</returns>  
        public static Dictionary<string, object> EnumListDic<T>(string keyDefault, string valueDefault = "")
        {
            Dictionary<string, object> dicEnum = new Dictionary<string, object>();
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return dicEnum;
            }
            if (!string.IsNullOrEmpty(keyDefault)) //判断是否添加默认选项  
            {
                dicEnum.Add(keyDefault, valueDefault);
            }
            string[] fieldstrs = System.Enum.GetNames(enumType); //获取枚举字段数组  
            foreach (var item in fieldstrs)
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组  
                if (arr != null && arr.Length > 0)
                {
                    description = ((DescriptionAttribute)arr[0]).Description;   //属性描述  
                }
                else
                {
                    description = item;  //描述不存在取字段名称  
                }
                dicEnum.Add(description, (int)System.Enum.Parse(enumType, item));  //不用枚举的value值作为字典key值的原因从枚举例子能看出来，其实这边应该判断他的值不存在，默认取字段名称  
            }
            return dicEnum;
        }
        /// <summary>  
        /// 获取枚举项描述信息 例如GetEnumDesc(Days.Sunday)  
        /// </summary>  
        /// <param name="en">枚举项 如Days.Sunday</param>  
        /// <returns></returns>  
        public static string GetEnumDesc(Type enumType,string ord)
        {
            Dictionary<string, string> EnumItemValueDesc = GetEnumItemValueDesc(enumType);
            return EnumItemValueDesc[ord];
        }
        ///<summary>  
        /// 获取枚举项+描述  
        ///</summary>  
        ///<param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>  
        ///<returns>键值对</returns>  
        public static Dictionary<string, string> GetEnumItemDesc(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                }
            }
            return dic;
        }
        ///<summary>  
        /// 获取枚举值+描述  
        ///</summary>  
        ///<param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>  
        ///<returns>键值对</returns>  
        public static Dictionary<string, string> GetEnumItemValueDesc(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type typeDescription = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = field.Name;
                    }
                    dic.Add(strValue, strText);
                }
            }
            return dic;
        }

        public static string GetStatus(string code) {
            string DicSta = GetEnumDesc(typeof(Status), code);
            return DicSta;
        }
    }
}