using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Win7.Sqlite
{
    /// <summary>
    /// API基类 GetColumns，PostData，GetData，DeleteData
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public abstract class BaseController<TEntity,TType> : ApiController where TEntity:IEntity,new() where TType : IEntity, new()
    {
        protected DataFilters _filters = null;
        protected Func<string,TEntity> _create = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="create">use to new</param>
        public BaseController(DataFilters filters, Func<string, TEntity> create = null)
        {
            _filters = filters;
            _create = create;
        }

        /// <summary>
        /// 获取所有列的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Object GetColumns()
        {
            return Sqlite.SqliteDataObject.GetColumns<TEntity, TType>(Request.Headers);
        }

        /// <summary>
        /// 提交数据(新增或修改)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public Object PostData(TEntity entity)
        {
            try
            {
                return Sqlite.SqliteDataObject.Update(entity, Request.Headers);
            }
            catch
            {
                return "Save Faile !";
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<TEntity> GetData(string column = null)
        {
            if (!string.IsNullOrWhiteSpace(column))
            {
                return Sqlite.SqliteDataObject.Select<TEntity>(_filters[column]).Concat(new[] { _create == null ? new TEntity() : _create(column)});
            }
            else
            {
                return Sqlite.SqliteDataObject.Select<TEntity>(null).Concat(new[] { new TEntity() });
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteData(TEntity entity)
        {
            return Sqlite.SqliteDataObject.Delete(entity, Request.Headers);
        }
    }
}
