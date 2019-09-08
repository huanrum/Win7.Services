using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Win7.Sqlite;

namespace Win7.Services
{
    public class Win7UserSoftware:IEntity
    {
        [DBRegular(DBRegular.NotNull)]
        public int UserFk { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public int SoftwareFk { set; get; }
    }
}