using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Win7.Accounting
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public AccountUser Login(string name, string password)
        {
            var user = Sqlite.SqliteDataObject.Select<AccountUser>(e => e.Add("Name", name).Add("Password", password)).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public IEnumerable<string> GetTranslate()
        {
            return Win7.Sqlite.CTranslateAttribute.GetTranslate;
        }
    }
}
