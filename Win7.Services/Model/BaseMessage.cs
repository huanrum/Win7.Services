using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Win7.Sqlite;

namespace Win7.Services.Model
{
    public class BaseMessage : IEntity
    {
        [DBRegular(DBRegular.NotNull)]
        public int FromUser { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public int ToUser { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Content { set; get; }
    }
}