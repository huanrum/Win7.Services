using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win7.Sqlite;

namespace Win7.Accounting
{
    public class AccountUser : IEntity
    {
        [DBNotUpdate, DBRegular(DBRegular.NotNull)]
        public string Name { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Password { set; get; }
    }
}
