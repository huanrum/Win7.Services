using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public abstract class IEntity
    {
        /// <summary>
        /// Id不显示且不能为空
        /// </summary>
        [DBHide, DBRegular(DBRegular.NotNull)]
        public int Id { set; get; }

        /// <summary>
        /// 版本号，每次修改都加1
        /// </summary>
        [DBHide, DBRegular(DBRegular.NotNull)]
        public int Version { set; get; }
        /// <summary>
        /// 数据创建人
        /// </summary>
        [DBHide,DBRegular(DBRegular.NotNull)]
        public int InsertBy { set; get; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        [DBHide,DBRegular(DBRegular.NotNull)]
        public DateTime InsertDate { set; get; }
        /// <summary>
        /// 数据更新人
        /// </summary>
        [DBHide]
        public int? UpdateBy { set; get; }
        /// <summary>
        /// 数据更新时间
        /// </summary>
        [DBHide]
        public DateTime? UpdateDate { set; get; }
        /// <summary>
        /// 数据是否处于活动状态，只显示true的数据
        /// </summary>
        [DBHide]
        public bool IsLive { set; get; }

    }
}
