using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public static class Ex
    {
        /// <summary>
        /// 获取DataFilter里面的值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> GetValues<TKey>(this Action<DataFilter> filter)
        {
            var data = new DataFilter();
            filter(data);
            return data.GetValues<TKey>();
        }

        /// <summary>
        /// 数据库避免关键字
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string DBKeyword(this string sender)
        {
            var keywords = new []{"class","index"};
            if (keywords.Contains(sender.ToLower()))
            {
                return sender + "__";
            }
            else
            {
                return sender;
            }
        }
    }

    /// <summary>
    /// DataFilter字典
    /// </summary>
    public class DataFilters : Dictionary<string, Action<DataFilter>>
    {
        public DataFilters Increase(string colomn,Action<DataFilter> filter)
        {
            this.Add(colomn, filter);
            return this;
        }

        public DataFilters Increase(Dictionary<string, Action<DataFilter>> filters)
        {
            foreach (var item in filters)
            {
                this.Add(item.Key, item.Value);
            }
            return this;
        }
    }

    /// <summary>
    /// 可转换的判断条件
    /// </summary>
    public class DataFilter 
    {
        private Dictionary<string, Object> data = new Dictionary<string, object>();

        /// <summary>
        /// 添加一个判断，最后转换为 a = ...
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataFilter Add(string key,object value)
        {
            data.Add(key, value);
            return this;
        }
        /// <summary>
        /// 添加一个判断，最后转换为 a in (...)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataFilter Add(string key, IEnumerable<object> value)
        {
            data.Add(key, value);
            return this;
        }

        /// <summary>
        /// 获取判断
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public IEnumerable<TKey> GetValues<TKey>()
        {
            return data.Values.Where(e => e is TKey).Cast<TKey>();
        }

        /// <summary>
        /// 转换成字符串的判断条件
        /// </summary>
        /// <returns></returns>
        public string ToFilter()
        {
            var result = new List<string>();
            foreach (var entity in data)
            {
                if (entity.Value != null)
                {
                    if (!(entity.Value is string) && (entity.Value is IEnumerable))
                    {
                        result.Add(entity.Key.DBKeyword() + " in (" + Join(entity.Value as IEnumerable, ",") + ")");
                    }
                    else
                    {
                        result.Add(entity.Key.DBKeyword() + "=" + ValueToString(entity.Value));
                    }
                }
            }

            return Join(result, " and ");
        }

        private string ValueToString(object value)
        {
            var type = value.GetType();
            if(type.IsValueType)
            {
                 return ""+value;
            }
             else if(type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return "\""+((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")+"\"";
            }
            else
            {
                return "\"" + value + "\"";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="get"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        private string Join(IEnumerable entities, string split)
        {
            var result = "";
            foreach (var entity in entities)
            {
                if (entity != null)
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        result += split;
                    }
                    result += entity;
                }
            }
            return result;
        }
    }
}
