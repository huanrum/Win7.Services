using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Win7.Sqlite;

namespace Win7.Test
{
    public class Win7User : IEntity
    {
        public string Name { set; get; }

        public string Password { set; get; }
    }
}