using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public class SqliteDataObject
    {
        /// <summary>
        /// 执行一条语句
        /// </summary>
        public bool Execute(string strSql)
        {
            return SQLiteHelper.Execute(strSql);
        }
        /// <summary>
        /// 返回一个DataSet结果集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataSet GetList(string strSql)
        {
            return SQLiteHelper.Query(strSql);
        }
        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public object GetScalar(string strSql)
        {
            var dataset = SQLiteHelper.Query(strSql);
            if(dataset.Tables != null && dataset.Tables.Count >0){
                if(dataset.Tables[0].Rows.Count > 0){
                    return dataset.Tables[0].Rows[0];
                }
            }
            return null;
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string strSql)
        {
            return SQLiteHelper.Execute(strSql);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<TEntity> GetList<TEntity>(string filter) where TEntity : class,new()
        {
            var relust = new List<TEntity>();
            var type = typeof(TEntity);
            var sql = "select * from " + SQLiteHelper.ToHump(type.Name) + (string.IsNullOrWhiteSpace(filter) ? "" : (" where " + filter));
            var dataset = SQLiteHelper.Query(sql);
            if(dataset.Tables != null && dataset.Tables.Count >0){
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    var entity = new TEntity();
                    foreach (var pro in type.GetProperties())
                    {
                        foreach (DataColumn column in dataset.Tables[0].Columns)
                        {
                            if (column.ColumnName.Replace("_", "").ToUpper() == pro.Name.ToUpper())
                            {
                                pro.SetValue(entity, row[column]);
                            }
                        }
                    }
                    relust.Add(entity);
                }
            }
            return relust;
        }
    }
}
