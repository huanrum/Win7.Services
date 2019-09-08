using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Win7.Services.Files
{
	/// <summary>
	/// 
	/// </summary>
	public static class FileMangage
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static Tuple<string, string> GetTuple(string name, string parent)
		{
			return new Tuple<string, string>(parent + "\\" + name, "../" + name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetLocalhostPath(string name,bool isFile = false)
		{
			var hostName = System.Net.Dns.GetHostName().ToLower();
            name = name.Replace("localhost", hostName).Replace("////", "//");

			if(name.StartsWith("../"))
			{
				var count = new Regex("../").Matches(name).Count - 1;
				var parent = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory);
				while (count-- > 0)
				{
					parent = parent.Parent;
				}
				name = parent.FullName + "\\" + name.Replace("../", "");
			}

			return name + (isFile?"":"//");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="directory"></param>
		/// <returns></returns>
		public static object Get(string directory)
		{
			var result = new List<object>();
			var directoryInfo = new DirectoryInfo(directory);
			foreach (var child in directoryInfo.GetDirectories())
			{
				result.Add(new { name = child.Name, children = Get(child.FullName)});
			}
			result.AddRange(directoryInfo.GetFiles().Select(e=>e.Name));

			return result;
		}

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        public static string GetRequestUriPath(string webUrl)
        {
            return System.Web.HttpContext.Current.Server.MapPath(webUrl);
        }
	}
}
