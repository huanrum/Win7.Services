using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Specialized;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Win7.Services.Files
{
	/// <summary>
	/// 
	/// </summary>
	public class ExplorerCatalogController : ApiController
    {
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object GetFiles(string localhost,string name)
		{
            var dd = FileMangage.GetRequestUriPath(Request.Headers.Referrer.AbsolutePath);
            var temp = FileMangage.GetTuple(name, System.AppDomain.CurrentDomain.BaseDirectory);
			return new
			{
				name = name,
				path = temp.Item2.Replace("localhost", localhost),
				children = Directory.Exists(temp.Item1) ?FileMangage.Get(temp.Item1) : null
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<object> UploadFile()
		{
			try
			{
				var path = System.AppDomain.CurrentDomain.BaseDirectory + "UploadTemp//";
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				var provider = new MultipartFormDataStreamProvider(path);
				var result = await Request.Content.ReadAsMultipartAsync(provider);
				var formData = result.FormData;

				if (provider.FileData.Count == 1)
				{
					var buffer = File.ReadAllBytes(provider.FileData[0].LocalFileName);

					var fs = new FileStream(FileMangage.GetLocalhostPath(provider.FormData.Get("filePath")) + provider.FormData.Get("fileName"), FileMode.Append, FileAccess.Write);
					fs.Write(buffer, 0, buffer.Length);
					fs.Close();

					File.Delete(provider.FileData[0].LocalFileName);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public string SaveNotepad()
		{
			var fileName = "nodepad - " + DateTime.Now.Ticks.ToString()+ ".ehr.html";
			var path = Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\saveFiles\\";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			var bytes = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.TotalBytes);
			var message = Encoding.Default.GetString(bytes);
			File.AppendAllText(path + fileName, "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><title>" + fileName + "</title></head><body>" + message + "</body></html>");
			return fileName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fillName"></param>
		/// <returns></returns>
		public bool DeleteFile(FileParm fileParm)
		{
			try
			{
				var file = FileMangage.GetLocalhostPath(fileParm.Path);
				File.Delete(file + fileParm.File);
				return true;
			}
			catch
			{
				return false;
			}
			
		}
    }

	/// <summary>
	/// 
	/// </summary>
	public class FileParm
	{
		/// <summary>
		/// 
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string File { get; set; }
	}
}
