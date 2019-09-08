using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win7.Sqlite;

namespace Win7.Staff
{
    public class StaffUser : IEntity
    {
        [DBNotUpdate, DBRegular(DBRegular.NotNull)]
        public string Name { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Password { set; get; }

        [DBNotMap]
        public string Token { get { return (Id + "+" + DateTime.Now.Ticks).ToMD5();}}
        [DBNotMap]
        public long SessionId{ get{return DateTime.Now.Ticks; }}
    }
}
