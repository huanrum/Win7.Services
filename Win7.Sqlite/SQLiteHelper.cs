using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    internal class SQLiteHelper : IDisposable
    {
        SQLiteConnection _connection;

        #region 初始化
        public SQLiteHelper(string file)
        {
            _connection = new SQLiteConnection("Data Source=" + file);
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }
        public SQLiteHelper()
        {
            _connection = new SQLiteConnection("Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "\\services.db3");
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public void Dispose()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }
        #endregion

        /// <summary>
        /// 转驼峰格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHump(string str)
        {
            if (str.Contains('_'))
            {
                return str.Replace("_", "");
            }
            else
            {
                var result = "";
                for (var i = 0; i < str.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(result) && 64 < str[i] && str[i]<91)
                    {
                        result += '_';
                    }
                    result += str[i];
                }
                return result;
            }
        }

        public bool Execute(string sql)
        {
            return Execute(sql, null);
        }

        public bool Execute(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteCommand command = ExcDbCommand(sql, parameters);
            int result = command.ExecuteNonQuery();
            return result > 0;
        }


        public int MaxId(string table)
        {
            var dataset = Query("select max(Id) from " + table);
            if (dataset.Tables != null && dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows != null && dataset.Tables[0].Rows.Count > 0)
                {
                    if (dataset.Tables[0].Rows[0] != null && dataset.Tables[0].Rows[0][0] != null)
                    {
                        try
                        {
                            return (int)(long)dataset.Tables[0].Rows[0][0];
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return 0;
        }

        public DataSet Query(string sql, params SQLiteParameter[] parameters)
        {
            //this.Open();
            SQLiteCommand command = ExcDbCommand(sql, parameters);
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);
            da.Fill(ds);
            //this.Close();
            return ds;
        }

        public SQLiteDataReader Read(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteCommand command = ExcDbCommand(sql, parameters);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
        }




        private SQLiteCommand ExcDbCommand(string sql, SQLiteParameter[] parameters)
        {
            SQLiteCommand command = new SQLiteCommand(sql, _connection);
            command.CommandType = CommandType.Text;
            if (parameters == null || parameters.Length == 0)
                return command;
            foreach (SQLiteParameter param in parameters)
            {
                if (param != null)
                    command.Parameters.Add(param);
            }
            return command;
        }

       
    }
}
