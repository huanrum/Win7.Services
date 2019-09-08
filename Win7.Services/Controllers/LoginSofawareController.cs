using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Win7.Sqlite;

namespace Win7.Services
{
    public class LoginSoftwareController : ApiController
    {
        [HttpGet]
        public IEnumerable<ColumnEntity<object>> GetColumns()
        {
            var users = Sqlite.SqliteDataObject.Select<Win7User>(e => e.Add("IsAdmin", 0));
            var sofawares = Sqlite.SqliteDataObject.Select<Win7Software>(null);//.Select(e => { e.Icon = e.Icon.Replace("@path:",""); return e; });
            return new ColumnEntity<object>[] { 
                new ColumnEntity<object>{Name = "UserFk",Type="Int32",Show = true,Title = new[]{"User Name",""},Regular = new[]{DBRegular.NotNull},Selection = users},
                new ColumnEntity<object>{Name = "SoftwareFk",Type="Int32",Show = true,Title = new[]{"Software Name",""},Regular = new[]{DBRegular.NotNull},Selection = sofawares},
                new ColumnEntity<object>{Name = "Id"},
                new ColumnEntity<object>{Name = "Version"},
                new ColumnEntity<object>{Name = "IsLive"},
                new ColumnEntity<object>{Name = "InsertBy"},
                new ColumnEntity<object>{Name = "InsertDate"},
                new ColumnEntity<object>{Name = "UpdateBy"},
                new ColumnEntity<object>{Name = "UpdateDate"}
            };
        }

        [HttpGet]
        public IEnumerable<int> GetData(int user)
        {
            return Sqlite.SqliteDataObject.Select<Win7UserSoftware>(e => e.Add("UserFk", user)).Select(e=>e.SoftwareFk);
        }

        [HttpPost]
        public bool PostData(IEnumerable<Win7UserSoftware> userSoftwares)
        {
            foreach (var software in userSoftwares.GroupBy(e => e.UserFk))
            {
                Sqlite.SqliteDataObject.Delete(Sqlite.SqliteDataObject.Select<Win7UserSoftware>(e => e.Add("UserFk", software.Key)), Request.Headers);
                Sqlite.SqliteDataObject.Update(software as IEnumerable<Win7UserSoftware>);
            }
            return true;
        }
    }
}