using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win7.Sqlite;

namespace Win7.Accounting
{
    public class AccountType:IEntity
    {
        public static DataFilters LookUpFilters = new DataFilters().Increase("TypeFk", e => e.Add("TypeFk", 1));


        [DBNotUpdate, DBRegular(DBRegular.NotNull), CTranslate("名称")]
        public string Name { set; get; }

        [CTranslate("说明")]
        public string Info { set; get; }

        [DBNotUpdate, DBRegular(DBRegular.NotNull), CTranslate("数据类型")]
        public int TypeFk { set; get; }

    }
}
