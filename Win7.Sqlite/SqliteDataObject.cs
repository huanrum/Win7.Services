using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public partial class SqliteDataObject
    {
        /// <summary>
        /// 执行一条语句
        /// </summary>
        [ObsoleteAttribute]
        public static bool Execute(string strSql)
        {
            using (var sqlite = new SQLiteHelper())
            {
                return sqlite.Execute(strSql);
            }
        }
        /// <summary>
        /// 返回一个DataSet结果集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [ObsoleteAttribute]
        public static DataSet GetList(string strSql)
        {
            using (var sqlite = new SQLiteHelper())
            {
                return sqlite.Query(strSql);
            }
        }
        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [ObsoleteAttribute]
        public static DataRow GetScalar(string strSql)
        {
            using (var sqlite = new SQLiteHelper())
            {
                var dataset = sqlite.Query(strSql);
                if (dataset.Tables != null && dataset.Tables.Count > 0)
                {
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        return dataset.Tables[0].Rows[0];
                    }
                }
                return null;
            }
        }

        

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        [ObsoleteAttribute]
        public static bool Exists(string strSql)
        {
            using (var sqlite = new SQLiteHelper())
            {
                return sqlite.Execute(strSql);
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        [ObsoleteAttribute]
        public static bool Exists<TEntity>(string filter)
        {
            using (var sqlite = new SQLiteHelper())
            {
                return sqlite.Execute("select * from " + SQLiteHelper.ToHump(typeof(TEntity).Name) + (string.IsNullOrWhiteSpace(filter) ? "" : (" where " + filter)));
            }
        }

        /// <summary>
        /// 查询数据库并转换成TEntity的对象列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> Select<TEntity>(Action<DataFilter> filter) where TEntity : IEntity, new()
        {
            var dataFilter = new DataFilter();
            if (filter != null) filter(dataFilter);
            return Select<TEntity>(dataFilter.ToFilter());
        }

        
       
        /// <summary>
        /// 更新数据到数据库(Version=0是插入 , version>0是更新)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="httpHeader"></param>
        /// <returns></returns>
        public static TEntity Update<TEntity>(TEntity entity, HttpHeaders httpHeader = null) where TEntity : IEntity
        {
            using (var sqlite = new SQLiteHelper())
            {
                return UpdateData(sqlite,entity, httpHeader);
            }
        }
        /// <summary>
        /// 更新数据到数据库(Version=0是插入 , version>0是更新)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="httpHeader"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> Update<TEntity>(IEnumerable<TEntity> entities, HttpHeaders httpHeader = null) where TEntity : IEntity
        {
            using (var sqlite = new SQLiteHelper())
            {
                foreach (var entity in entities)
                {
                    UpdateData(sqlite,entity, httpHeader);
                }
                return entities;
            }
        }

        
       

        /// <summary>
        /// 删除数据(实际是把IsLive设置为false)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<TEntity>(IEnumerable<TEntity> entities, HttpHeaders httpHeader = null) where TEntity : IEntity
        {
            using (var sqlite = new SQLiteHelper())
            {
                foreach (var entity in entities)
                {
                    entity.IsLive = false;
                    UpdateData(sqlite, entity, httpHeader);
                }
                return true; 
            }
        }
        /// <summary>
        /// 删除数据(实际是把IsLive设置为false)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<TEntity>(TEntity entity, HttpHeaders httpHeader = null) where TEntity : IEntity
        {
            using (var sqlite = new SQLiteHelper())
            {
                entity.IsLive = false;
                UpdateData(sqlite, entity, httpHeader);
                return true; 
            }
        }

        /// <summary>
        /// 获取列的名称以及相关数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TypeTable"></typeparam>
        /// <param name="filters"></param>
        /// <param name="httpHeader"></param>
        /// <returns></returns>
        public static IEnumerable<ColumnEntity<TypeTable>> GetColumns<TEntity, TypeTable>(HttpHeaders httpHeader = null) where TypeTable : IEntity, new()
        {
            using (var sqlite = new SQLiteHelper())
            {
                var type = typeof(TEntity);
                var field = type.GetField("LookUpFilters");
                var filters = field == null ? null : field.GetValue(null) as Dictionary<string, Action<DataFilter>>;
                var columns = new List<ColumnEntity<TypeTable>>();

                return type.GetProperties().Select(property =>
                {
                    var column = new ColumnEntity<TypeTable>
                    {
                        Name = property.Name,
                        Title = property.GetCustomAttributes().Where(e => e is CTranslateAttribute).SelectMany(e => (e as CTranslateAttribute).Translate),
                        Tooltip = property.GetCustomAttributes().Where(e => e is CTooltipAttribute).SelectMany(e => (e as CTooltipAttribute).Tooltip),
                        Type = property.PropertyType.IsConstructedGenericType ? property.PropertyType.GetGenericArguments()[0].Name : property.PropertyType.Name,
                        Regular = property.GetCustomAttributes().Where(e => e is DBRegularAttribute).Select(e => (e as DBRegularAttribute).Regular),
                        Show = !property.GetCustomAttributes().Any(e => e is DBHideAttribute),
                        Update = !property.GetCustomAttributes().Any(e => e is DBNotUpdateAttribute)
                    };
                    if (filters != null && filters.ContainsKey(property.Name))
                    {
                        column.Selection = Select<TypeTable>(e => filters[property.Name](e));
                    }
                    return column;
                });
            }
        }

        #region private
        private static IEnumerable<TEntity> Select<TEntity>(string filter, bool isTrue = true) where TEntity : IEntity, new()
        {
            using (var sqlite = new SQLiteHelper())
            {
                var relust = new List<TEntity>();
                var type = typeof(TEntity);
                var sql = "select * from " + SQLiteHelper.ToHump(type.Name) + " where IsLive=" + (isTrue ? 1 : 0) + (string.IsNullOrWhiteSpace(filter) ? "" : (" and " + filter));
                var dataset = sqlite.Query(sql);
                if (dataset.Tables != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        var entity = new TEntity();
                        foreach (var pro in type.GetProperties())
                        {
                            foreach (DataColumn column in dataset.Tables[0].Columns)
                            {
                                if (column.ColumnName.Replace("_", "").ToUpper() == pro.Name.ToUpper() && row[column] != DBNull.Value)
                                {
                                    if (pro.GetCustomAttributes().Any(e => e is DBBase64Attribute))
                                    {
                                        pro.SetValue(entity, "base64:" + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(row[column].ToString())), null);
                                    }
                                    else
                                    {
                                         pro.SetValue(entity, row[column], null);
                                    }
                                }
                            }
                        }
                        relust.Add(entity);
                    }
                }
                return relust;
            }
        }
        private static TEntity UpdateData<TEntity>(SQLiteHelper sqlite, TEntity entity, HttpHeaders httpHeader = null) where TEntity : IEntity
        {
            var userId = 0;
            if (httpHeader != null && httpHeader.Contains("User"))
            {
                foreach (var use in httpHeader.GetValues("User"))
                {
                    int.TryParse(use, out userId);
                    if (userId > 0) break;
                }
            }
            if (entity.Version > 0)
            {
                entity.Version = entity.Version + 1;
                entity.UpdateBy = userId > 0 ? userId : entity.UpdateBy;
                entity.UpdateDate = DateTime.Now;
                _Update(sqlite, entity);
            }
            else
            {
                entity.Id = sqlite.MaxId(SQLiteHelper.ToHump(typeof(TEntity).Name)) + 1;
                entity.Version = 1;
                entity.InsertBy = userId > 0 ? userId : entity.InsertBy;
                entity.InsertDate = DateTime.Now;
                entity.IsLive = true;
                _Insert(sqlite, entity);
            }
            return entity;
        }
        private static TEntity _Insert<TEntity>(SQLiteHelper sqlite,TEntity entity) where TEntity : IEntity
        {
            var type = typeof(TEntity);
            var fields = "";
            var values = "";
            foreach (var pro in type.GetProperties())
            {
                if (pro.GetCustomAttributes().Any(e => e is DBNotMapAttribute))
                {
                    continue;
                }

                var value = pro.GetValue(entity, null);
                if (value != null)
                {
                    if (!string.IsNullOrWhiteSpace(fields))
                    {
                        fields += ",";
                    }
                    if (!string.IsNullOrWhiteSpace(values))
                    {
                        values += ",";
                    }
                    if (pro.PropertyType == typeof(DateTime))
                    {
                        value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (pro.PropertyType == typeof(Boolean))
                    {
                        value = ((bool)value) ? 1 : 0;
                    }
                    fields += pro.Name.DBKeyword();
                    values += (pro.PropertyType.IsValueType && pro.PropertyType != typeof(DateTime) ? "" : "\"") + value + (pro.PropertyType.IsValueType && pro.PropertyType != typeof(DateTime) ? "" : "\"");
                }
            }
            sqlite.Execute("insert into " + SQLiteHelper.ToHump(type.Name) + " (" + fields + ") values (" + values + ")");
            return entity;
        }

        private static TEntity _Update<TEntity>(SQLiteHelper sqlite,TEntity entity) where TEntity : IEntity
        {
            var type = typeof(TEntity);
            var sets = "";
            foreach (var pro in type.GetProperties())
            {
                if (pro.GetCustomAttributes().Any(e => e is DBNotMapAttribute))
                {
                    continue;
                }
                var value = pro.GetValue(entity);
                var isNotAddFlag = pro.PropertyType.IsValueType && (pro.PropertyType != typeof(DateTime) && pro.PropertyType != typeof(DateTime?));
                if (!string.IsNullOrWhiteSpace(sets))
                {
                    sets += ",";
                }
                if (value == null)
                {
                    value = "null";
                }
                else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
                {
                    value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (pro.PropertyType == typeof(Boolean))
                {
                    value = ((bool)value) ? 1 : 0;
                }

                sets += pro.Name.DBKeyword() + "=" + (isNotAddFlag ? "" : "\"") + value + (isNotAddFlag ? "" : "\"");
            }
            sqlite.Execute("update  " + SQLiteHelper.ToHump(type.Name) + " set " + sets + " where ID=" + entity.Id);
            return entity;
        }
        #endregion

    }
}
