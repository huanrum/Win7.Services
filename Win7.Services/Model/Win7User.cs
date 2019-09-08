using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Win7.Sqlite;

namespace Win7.Services
{
    public class Win7User : IEntity
    {
        [DBNotUpdate, DBRegular(DBRegular.NotNull)]
        public string Name { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Password { set; get; }

        public bool IsAdmin { set; get; }

        public DateTime? OnLineDate { set; get; }
    }
}