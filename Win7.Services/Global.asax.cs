using System.Web.Http;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using Newtonsoft.Json.Serialization;

namespace Win7.Services
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			GlobalConfiguration.Configuration.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{action}",
				defaults: new { id = RouteParameter.Optional }
			);
			GlobalConfiguration.Configuration.EnableSystemDiagnosticsTracing();

			var assemblies = System.AppDomain.CurrentDomain.GetAssemblies()
				.Where(e => !e.IsDynamic && e.CodeBase.StartsWith("file:///" + System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/")))
				.Select(e => e.CodeBase.Replace("file:///", "").Replace("/","\\").ToLower()).ToList();

			assemblies.Add(System.AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll");
			AddDll(assemblies,System.AppDomain.CurrentDomain.BaseDirectory);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			var json = GlobalConfiguration.Configuration.Formatters.FirstOrDefault(e => e is System.Net.Http.Formatting.JsonMediaTypeFormatter);
            
            GlobalConfiguration.Configuration.Formatters.Clear();
			GlobalConfiguration.Configuration.Formatters.Add(json);
            
			new Thread(() => {
				while (true)
				{
					AddDll(assemblies, System.AppDomain.CurrentDomain.BaseDirectory);
					Thread.Sleep(1000 * 60 * 30);
				}
				
			}).Start();
		}

		private void AddDll(IList<string> baseFiles,string directory)
		{
			foreach(var file in Directory.GetFiles(directory,"*.dll"))
			{
				if (!baseFiles.Contains(file.ToLower()))
				{
					try
					{
						System.AppDomain.CurrentDomain.ExecuteAssembly(file);
						baseFiles.Add(file.ToLower());
					}
					catch { }
				}
			}
			foreach (var directoryChild in Directory.GetDirectories(directory))
			{
				AddDll(baseFiles,directoryChild);
			}
		}
	}

	public static class ExtendXml
	{
		public static string GetValue(this XmlNode node, string name)
		{
			foreach (XmlAttribute child in node.Attributes)
			{
				if (child.Name.ToLower() == name.ToLower())
				{
					return child.Value;
				}
			}

			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.Name.ToLower() == name.ToLower())
				{
					return child.Value ?? child.ChildNodes[0].Value;
				}
			}

			return null;
		}
	}
}