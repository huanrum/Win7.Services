using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite
{
    public class BaseJson : IEntity
    {
        [DBRegular(DBRegular.NotNull)]
        public string Name { set; get; }

        public string Info { set; get; }

        [DBRegular(DBRegular.NotNull)]
        public string Content { set; get; }
    }
}
