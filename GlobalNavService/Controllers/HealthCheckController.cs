using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalNavService.Utils;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace GlobalNavService.Controllers
{
    public class HealthCheckController : Controller
    {
        public ContentResult PingCheck()
        {
            return Content("OK");
        }

        public JsonResult HealthCheck()
        {
            return Json(
                new 
                {
                    DPPortalRead = DPPortalRead(),
                    ping = ping()
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DPPortalReadCheck()
        {
            return Json(DPPortalRead(), JsonRequestBehavior.AllowGet);
        }

        private Object ping()
        {
            var sw = new Stopwatch();
            sw.Start();

            var assembly = Assembly.GetExecutingAssembly();
            var fi = new FileInfo(assembly.Location);

            var compilationDate = fi.LastWriteTime.ToUniversalTime().ToString("o");
            var version = assembly.GetName().Version.ToString();

            String requestUrl = HttpContext.Request.Url.ToString();
            if(requestUrl.Contains("?"))
            {
                requestUrl = requestUrl.Substring(0, requestUrl.LastIndexOf('?'));
            }

            Uri uri = new Uri(requestUrl + "/ping");
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Headers.Add("Authorization", HttpContext.Request.Headers["Authorization"]);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseText = reader.ReadToEnd();

            if(responseText.ToLower().Equals("ok"))
            {
                return new
                {
                    healty = true,
                    version = version,
                    compilationDate = compilationDate,
                    executionTime = String.Format("{0}ms", sw.ElapsedMilliseconds)
                };
            }
            else
            {
                return new
                {
                    healty = false,
                    version = version,
                    compilationDate = compilationDate,
                    executionTime = String.Format("{0}ms", sw.ElapsedMilliseconds)
                };
            }
        }

        private Object DPPortalRead()
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                using (var sqlCon = new SqlConnection(GlobalConst.FrameworkReadConnectionStringName))
                using (var sqlCmd = new SqlCommand("SELECT UserId FROM DPSec_Users WHERE LoweredUserName = 'acxiom'", sqlCon))
                {
                    sqlCon.Open();
                    sqlCmd.CommandType = CommandType.Text;
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.Read())
                        {
                            Guid acxiomId = sqlDr.GetGuid(0);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                return new
                {
                    healthy = false,
                    error = err.Message,
                    executionTime = String.Format("{0}ms", sw.ElapsedMilliseconds)
                };
            }

            return new
            {
                healthy = true,
                executionTime = String.Format("{0}ms", sw.ElapsedMilliseconds)
            };
        }
    }
}
