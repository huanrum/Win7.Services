using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Win7.Services.Model;

namespace Win7.Services.Controllers
{
    public class MessageController : ApiController
    {
        [HttpGet]
        public Object Get(int userId)
        {
            var listFrom = Sqlite.SqliteDataObject.Select<BaseMessage>(e => e.Add("FromUser", userId)).Where(e=>e.InsertDate.AddHours(4) > DateTime.Now);
            var listTo = Sqlite.SqliteDataObject.Select<BaseMessage>(e => e.Add("ToUser", userId)).Where(e => e.InsertDate.AddHours(4) > DateTime.Now);
            return listFrom.Concat(listTo);
        }

        [HttpPost]
        public Object Post(BaseMessage bm)
        {
            return Sqlite.SqliteDataObject.Update(bm);
        }
    }
}