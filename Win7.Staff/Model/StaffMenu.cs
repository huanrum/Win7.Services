using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win7.Sqlite;

namespace Win7.Staff
{
    public class StaffMenu : IEntity
    {
        [DBRegular(DBRegular.NotNull)]
        public string Name { set; get; }
        [DBRegular(DBRegular.NotNull)]
        public string Index { set; get; }
        [DBRegular(DBRegular.NotNull)]
        public string Link { set; get; }
        public string Icon { set; get; }
        public string Type { set; get; }
        public string AccessRight { set; get; }



        public string Controller { set; get; }


        public int? StaffMenuFk { set; get; }
        [DBNotMap]
        public IEnumerable<StaffMenu> Submenu { set; get; }
    }
}
