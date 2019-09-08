using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win7.Sqlite.Model
{
    class BaseFunction
    {
        [DBNotUpdate,DBRegular("\\s"),CTranslate("函数名称")]
        public string Name { set; get; }

        [DBBase64,DBRegular("\\s"), CTranslate("函数")]
        public string Function { set; get; }

        public string Info { set; get; }
    }
}
