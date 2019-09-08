using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    /// <summary>
    /// 数据列的信息
    /// </summary>
    /// <typeparam name="TypeTable"></typeparam>
    public class ColumnEntity<TypeTable>
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 列标题
        /// </summary>
        [DBNotMap]
        public IEnumerable<string> Title { set; get; }
        /// <summary>
        /// 列提示
        /// </summary>
        [DBNotMap]
        public IEnumerable<string> Tooltip { set; get; }
        /// <summary>
        /// 列类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 下拉选项
        /// </summary>
        public IEnumerable<TypeTable> Selection { set; get; }
        /// <summary>
        /// 是否更新
        /// </summary>
        public bool Update { set; get; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { set; get; }
        /// <summary>
        /// 数据判断
        /// </summary>
        public IEnumerable<string> Regular { set; get; }
        
    }
}
