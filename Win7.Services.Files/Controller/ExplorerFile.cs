using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Win7.Services.Files.Controller
{
    public class ExplorerFileController : ApiController
    {
        [HttpGet]
        public object GetMedia(string name)
        {

            try
            {
                foreach (var file in new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "midea//").GetFiles())
                {
                    if (file.Name.Replace(file.Extension, "") == name)
                    {
                        var FilePath = file.FullName;
                        var stream = new FileStream(FilePath, FileMode.Open);
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Content = new StreamContent(stream);
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = file.Name
                        };
                        return response;
                    }
                }

            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public object _GetMedia(string name)
        {
            var response = new HttpResponseMessage();
            foreach (var file in new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "midea//").GetFiles())
            {
                if (file.Name.Replace(file.Extension, "") == name)
                {
                    response.Content = new StringContent(FileMangage.GetLocalhostPath(Request.RequestUri.ToString().Split('?').FirstOrDefault() + "/../../midea/" + file.Name, true), Encoding.UTF8, "text/html");
                }
            }
            return response;
        }

    }
}
