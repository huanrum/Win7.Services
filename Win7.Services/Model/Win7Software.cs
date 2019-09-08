using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Win7.Sqlite;

namespace Win7.Services
{
    public class Win7Software : IEntity
    {
        [DBNotUpdate, DBRegular(DBRegular.NotNull)]
        public string Name { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Title { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Icon { set; get; }

        public int? OpenCount { set; get; }

        public string GroupName { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string FnUrl { set; get; }

        public string DialogSize { set; get; }

        public bool NoCanMax { set; get; }

        public bool Suspension { set; get; }

        public bool HideDesktop { set; get; }
    }
}