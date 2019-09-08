using System.IO;
using System.Collections.Specialized;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System;
using System.Linq;
using System.Web;
using System.Xml;
using System.Reflection;
using Win7.Sqlite;

namespace Win7.Accounting.Controller
{
    public class AccountConsumeController : BaseController<AccountConsume, AccountType>
    {
        public AccountConsumeController()
            : base(AccountConsume.LookUpFilters)
        {

        }
    }
}
