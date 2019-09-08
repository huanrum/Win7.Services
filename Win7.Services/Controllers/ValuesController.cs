using System.Collections.Generic;
using System.Web.Http;
using System.Linq;


namespace Win7.Services.Controllers
{
	public class ValuesController : ApiController
	{
		// GET api/values
		public IEnumerable<string> Get()
		{
			var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().Where(e => !e.IsDynamic && e.CodeBase.StartsWith("file:///" + System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\","/"))).Select(e => e.CodeBase.Replace("file:///",""));

			return assemblies;
		}

		// GET api/values/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}