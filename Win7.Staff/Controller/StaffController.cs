using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Win7.Staff
{
    public class StaffController : ApiController
    {
        [HttpPost]
        public object PostLogin(Dictionary<string, string> pn)
        {
            if (pn.ContainsKey("username") && pn.ContainsKey("password"))
            {
                var user = Sqlite.SqliteDataObject.Select<StaffUser>(e => e.Add("Name", pn["username"]).Add("Password", pn["password"])).FirstOrDefault();
                if (user != null)
                {
                    return user.To();
                }
            }

            return Staff.To(null, "9010103");;
        }

        [HttpGet]
        public object GetMenus()
        {
            var list = Sqlite.SqliteDataObject.Select<StaffMenu>(null);
            return list.Select(e => { e.Submenu = list.Where(l => l.StaffMenuFk == e.Id).ToList(); return e; }).Where(e => e.StaffMenuFk == null).To();
        }

        [HttpPost]
        public void PostData(IEnumerable<StaffMenu> menus)
        {

            Sqlite.SqliteDataObject.Update(menus);
            foreach (var menu in menus)
            {
                foreach (var sub in menu.Submenu)
                {
                    sub.StaffMenuFk = menu.Id;
                }
            }
            Sqlite.SqliteDataObject.Update(menus.SelectMany(e=>e.Submenu));
        }

        [HttpGet]
        public object GetJsonContent(string name)
        {
            return Sqlite.SqliteDataObject.Select<Sqlite.BaseJson>(e => e.Add("Name", name)).FirstOrDefault().To();
        }
    }
}
