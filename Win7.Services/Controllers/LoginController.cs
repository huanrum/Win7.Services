using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml;
using Win7.Sqlite;

namespace Win7.Services.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public class LoginController : ApiController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		[HttpGet]
		public object Login(string name,string password)
		{

            var user = Sqlite.SqliteDataObject.Select<Win7User>(e => e.Add("Name", name).Add("Password", password)).FirstOrDefault();
            if (user != null)
            {
                if (user.IsAdmin == true)
                {
                    return new { IsAdmin = true, Id = user.Id, Softwares = Sqlite.SqliteDataObject.Select<Win7Software>(null) };
                }
                else
                {
                    var userSoftwareIds = Sqlite.SqliteDataObject.Select<Win7UserSoftware>(e=>e.Add("UserFk",user.Id)).Select(e => e.SoftwareFk);

                    return new { IsAdmin = false, Id = user.Id, Softwares = Sqlite.SqliteDataObject.Select<Win7Software>(e => e.Add("Id", userSoftwareIds)) };
                }
            }
            else
            {
                return null;
            }
		}

        [HttpGet]
        public Object GetFrient(int userId, int seconds)
        {
            Sqlite.SqliteDataObject.Update(Sqlite.SqliteDataObject.Select<Win7User>(e => e.Add("Id", userId)).Select(e => { e.OnLineDate = DateTime.Now; return e; }));
            return Sqlite.SqliteDataObject.Select<Win7User>(null).Where(e => e.Id != userId && e.OnLineDate != null && e.OnLineDate.Value.AddSeconds(seconds) > DateTime.Now);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="password"></param>
		/// <returns></returns>
        [HttpGet]
        public string Register(string name, string password)
        {
            var user = Sqlite.SqliteDataObject.Select<Win7User>(e => e.Add("Name", name)).FirstOrDefault();
            if (user == null)
            {
                Sqlite.SqliteDataObject.Update(new Win7User { Name = name, Password = password });
                return null;
            }
            else
            {
                return name + " is exist!";
            }
        }


        public IEnumerable<string> GetDBData(string sql)
        {
            return sql.Split(';').Select(e=>Sqlite.SqliteDataObject.Select(e));
        }
	}
}