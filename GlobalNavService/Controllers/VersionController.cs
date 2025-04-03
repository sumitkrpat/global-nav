using System.IO;
using System.Reflection;
using System.Web.Mvc;
using GlobalNavService.Utils;

namespace GlobalNavService.Controllers
{
    public class VersionController : Controller
    {
        public JsonResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fi = new FileInfo(assembly.Location);

            var compilationDate = fi.LastWriteTime.ToShortDateString();
            var version = assembly.GetName().Version.ToString();
            
            return Json(new
            {
                Version = version,
                Environment = GlobalConst.FrameworkEnvironment,
                CompilationDate = compilationDate,
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
