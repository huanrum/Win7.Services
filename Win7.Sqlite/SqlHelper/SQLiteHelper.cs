using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    internal class SQLiteHelper
    {
        static string _connectionString = "../services.db3";
        static SQLiteConnection _connection;

        public SQLiteHelper()
        {
            _connection = new SQLiteConnection(_connectionString);
        }
        public SQLiteHelper(string connectionString)
            : this()
        {
            _connectionString = connectionString;
        }

        public static void Open()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }
        public static void Close()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }
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
                    if (string.IsNullOrWhiteSpace(result) && str[i] == char.ToUpper(str[i]))
                    {
                        result += '_';
                    }
                    result += str[i];
                }
                return result;
            }
        }

        public static bool Execute(string sql)
        {
            return Execute(sql, null);
        }

        public static bool Execute(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteCommand command = ExcDbCommand(sql, parameters);
            Open();
            int result = command.ExecuteNonQuery();
            Close();
            return result > 0;
        }

        public static DataSet Query(string sql)
        {
            return Query(sql, null);
        }

        public static DataSet Query(string sql, params SQLiteParameter[] parameters)
        {
            //this.Open();
            SQLiteCommand command = ExcDbCommand(sql, parameters);
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);
            da.Fill(ds);
            //this.Close();
            return ds;
        }


        public static SQLiteDataReader Read(string sql)
        {
            return Read(sql, null);
        }
        public static SQLiteDataReader Read(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteCommand command = ExcDbCommand(sql, parameters);
            Open();
            SQLiteDataReader reader = command.ExecuteReader();
            //this.Close();
            return reader;
        }




        static SQLiteCommand ExcDbCommand(string sql, SQLiteParameter[] parameters)
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
