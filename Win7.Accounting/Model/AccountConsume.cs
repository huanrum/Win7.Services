using System;
using System.Collections.Generic;
using System.Text;
using Win7.Sqlite;

namespace Win7.Accounting
{
    public class AccountConsume : IEntity
    {
        public static DataFilters LookUpFilters = new DataFilters()
                                                     .Increase("@@Fk", e => e.Add("TypeFk", 1))
                                                     .Increase("ComsumerFk", e => e.Add("TypeFk", 2))
                                                     .Increase("ComsumptionFk", e => e.Add("TypeFk", 3))
                                                     .Increase("ComsumptionModeFk", e => e.Add("TypeFk", 4));

        public AccountConsume()
        {
            ConsumeDate = DateTime.Now;
        }


        [DBNotUpdate, DBRegular(DBRegular.NotNull), CTranslate("名称")]
        public string Name { set; get; }

        [DBNotUpdate, DBRegular(DBRegular.NotNull), CTranslate("消费日期")]
        public DateTime ConsumeDate { set; get; }

        [DBRegular(DBRegular.NotNull), CTranslate("金额")]
        public double Amount { set; get; }

        [CTranslate("说明")]
        public string Info { set; get; }

        [DBRegular(DBRegular.NotNull), CTranslate("消费者"), CTooltipAttribute("", "双击打开消费者列表")]
        public int ComsumerFk  { set; get; }

        [DBRegular(DBRegular.NotNull), CTranslate("消费类型"), CTooltipAttribute("", "双击打开消费类型列表")]
        public int ComsumptionFk { set; get; }

        [DBRegular(DBRegular.NotNull), CTranslate("消费方式"), CTooltipAttribute("", "双击打开消费方式列表")]
        public int ComsumptionModeFk { set; get; }

    }
}
