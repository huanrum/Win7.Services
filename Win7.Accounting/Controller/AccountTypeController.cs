using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Win7.Sqlite;


namespace Win7.Accounting
{
    public class AccountTypeController : BaseController<AccountType, AccountType>
    {
        public AccountTypeController()
            : base(AccountConsume.LookUpFilters, column =>
            {
                var filters = AccountConsume.LookUpFilters;
                if (filters.ContainsKey(column))
                {
                    var typeFks = filters[column].GetValues<int>();
                    return new AccountType() { TypeFk = typeFks.FirstOrDefault() };
                }
                else
                {
                    return new AccountType();
                }
            })
        {

        }
    }
}
