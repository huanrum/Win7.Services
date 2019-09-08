using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public partial class SqliteDataObject
    {
        private static Func<DataRow,object> NewClass(DataTable table)
        {
            var classSource = new StringBuilder();
            classSource.Append(string.Format("public   class   {0} \n",table.TableName));
            classSource.Append("{\n");
            foreach (DataColumn column in table.Columns)
            {
                classSource.Append(string.Format("public {0} {1}", column.DataType.Name, column.ColumnName) + " {set;get;}\n");
            }
            classSource.Append("}\n");

            //设置编译参数。   
            var paras = new CompilerParameters { GenerateExecutable = true, GenerateInMemory = true };

            paras.ReferencedAssemblies.Add("System.dll");
            paras.ReferencedAssemblies.Add("System.Data.dll");

            //编译代码,获取编译后的程序集。   
            var assembly = new CSharpCodeProvider().CompileAssemblyFromSource(paras, classSource.ToString()).CompiledAssembly;

            return new Func<DataRow,object>(row =>
            {
                var entity = assembly.CreateInstance(table.TableName);
                foreach (var pro in entity.GetType().GetProperties())
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName.Replace("_", "").ToUpper() == pro.Name.ToUpper() && row[column] != DBNull.Value)
                        {
                            pro.SetValue(entity, row[column], null);
                        }
                    }
                }
                return entity;
            });
        }



        public static string Select(string table)
        {
            using (var sqlite = new SQLiteHelper())
            {
                var relust = "";

                var sql = "select * from " + SQLiteHelper.ToHump(table);
                if (table.ToLower().Contains("select"))
                {
                    sql = table;
                }
                var dataset = sqlite.Query(sql);
                if (dataset.Tables != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        var entity = "";
                        foreach (DataColumn column in dataset.Tables[0].Columns)
                        {
                            if (!string.IsNullOrWhiteSpace(entity))
                            {
                                entity += ",";
                            }
                            if (column.DataType.IsArray)
                            {
                                entity += string.Format("\"{0}\":\"{1}\"", column.ColumnName, "base64:"+Convert.ToBase64String(row[column] as byte[]));
                            }
                            else
                            {
                                entity += string.Format("\"{0}\":\"{1}\"", column.ColumnName, row[column] == null ? null : row[column].ToString().Replace("\"", "\\\"").Replace("\n", ""));
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(relust))
                        {
                            relust += ",";
                        }
                        relust += "{" + entity + "}";
                    }
                }
                return "[" + relust + "]";
            }
        }
    }
}
