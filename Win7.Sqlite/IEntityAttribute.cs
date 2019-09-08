using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    /// <summary>
    /// 字段的翻译
    /// </summary>
    public class CTranslateAttribute : System.Attribute
    {
        /// <summary>
        /// 所有翻译类型
        /// </summary>
        public static IEnumerable<string> GetTranslate = new string[] { "English","中文"};

        //[0]English，[1]中文
        public CTranslateAttribute(params string[] translates)
        {
            var _translates = new string[GetTranslate.Count()];
            foreach (var translate in translates)//[\u4e00-\u9fa5]+
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(translate, "[\u4e00-\u9fa5]"))
                {
                    _translates[1] = translate;
                }
                else
                {
                    _translates[0] = translate;
                }
            }
            Translate = _translates;
        }

        public IEnumerable<string> Translate { set; get; }

        
    }

    /// <summary>
    /// 字段的提示
    /// </summary>
    public class CTooltipAttribute : System.Attribute 
    {
        public CTooltipAttribute(params string[] translates)
        {
            var _translates = new string[CTranslateAttribute.GetTranslate.Count()];
            foreach (var translate in translates)//[\u4e00-\u9fa5]+
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(translate, "[\u4e00-\u9fa5]"))
                {
                    _translates[1] = translate;
                }
                else
                {
                    _translates[0] = translate;
                }
            }
            Tooltip = _translates;
        }

        public IEnumerable<string> Tooltip { set; get; }
    }

    /// <summary>
    /// Base64
    /// </summary>
    public class DBBase64Attribute : System.Attribute
    {}

    /// <summary>
    /// 不需要更数据库交互的字段
    /// </summary>
    public class DBNotMapAttribute : System.Attribute
    {}

    /// <summary>
    /// 隐藏的字段
    /// </summary>
    public class DBHideAttribute : System.Attribute
    { }

    /// <summary>
    /// 不允许更新的字段
    /// </summary>
    public class DBNotUpdateAttribute : System.Attribute
    { }

    /// <summary>
    /// 添加正则表达式判断
    /// </summary>
    public class DBRegularAttribute : System.Attribute
    {
        public DBRegularAttribute(string flag)
        {
            Regular = flag;
        }

        public string Regular { set; get; }
    }

    public class DBRegular
    {
        /// <summary>
        /// 非空字段
        /// </summary>
        public const string NotNull = @"^[\s\S]+$";
    }
}
