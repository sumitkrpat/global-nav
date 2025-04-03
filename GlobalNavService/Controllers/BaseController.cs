using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Acxiom.Web.Portal;
using GlobalNavService.Models;
using GlobalNavService.Utils;

namespace GlobalNavService.Controllers
{
    /// <summary>
    /// BaseController class
    /// </summary>
    public class BaseController : Controller
    {

        /// <summary>
        /// Inits the log.
        /// </summary>
        public void InitLog()
        {
            _log = new Log(string.Empty)
            {
                BaseFields =
                {
                    Component = GlobalConst.Component
                }
            };
        }
        /// <summary>
        /// Called when authorization occurs.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        /// <exception cref="System.Security.Authentication.AuthenticationException">user token is invalid</exception>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var authToken = Request.Headers.GetValues("Auth-Token");
                if (authToken != null)
                {
                    var formsAuthenticationTicket = FormsAuthentication.Decrypt(authToken.First());

                    if (formsAuthenticationTicket != null && !formsAuthenticationTicket.Expired)
                    {
                        System.Web.HttpContext.Current.User =
                            new GenericPrincipal(new FormsIdentity(formsAuthenticationTicket), new string[0]);
                    }
                    else
                    {
                        throw new AuthenticationException("user token is invalid");
                    }
                }
                else
                {
                    throw new AuthenticationException("user token is invalid");
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                Log.AddException(ex);
                IsAuthorized = false;
                AuthorizationError = "user token is invalid";
            }
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var authenticationHeaderBase64Value = Request.Headers[HttpRequestHeader.Authorization.ToString()];
                if (!string.IsNullOrEmpty(authenticationHeaderBase64Value))
                {

                    var basicAuthenticationFormatString =
                        Encoding.UTF8.GetString(
                            Convert.FromBase64String(authenticationHeaderBase64Value.Remove(0, "Basic ".Length)));

                    var basicAuthenticationParams = basicAuthenticationFormatString.Split(new[] { ':' }, 2);

                    Guid gKey;
                    if (Guid.TryParse((basicAuthenticationParams.FirstOrDefault() ?? string.Empty).Trim(), out gKey))
                    {
                        APIKey = gKey;
                        APISecret = (basicAuthenticationParams.LastOrDefault() ?? string.Empty).Trim();

                        IsAuthorized = CheckAccess();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddException(ex);
                IsAuthorized = false;
                AuthorizationError = "unauthorized";
            }

            base.OnActionExecuting(filterContext);
        }

        #region Protected Properties

        /// <summary>
        /// The _log
        /// </summary>
        private Log _log;

        /// <summary>
        /// The log
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        protected Log Log
        {
            get
            {
                if (_log == null)
                {
                    InitLog();
                }

                return _log;
            }
        }

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        protected Guid APIKey { get; private set; }

        /// <summary>
        /// Gets the API secret.
        /// </summary>
        /// <value>
        /// The API secret.
        /// </value>
        protected string APISecret { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is authorized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is authorized; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAuthorized { get; private set; }

        /// <summary>
        /// Gets the authorization error.
        /// </summary>
        /// <value>
        /// The authorization error.
        /// </value>
        protected string AuthorizationError { get; private set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>
        /// <returns></returns>
        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Formats the result.
        /// </summary>
        /// <param name="appModel">The application model.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        protected ActionResult FormatResult(ApplicationModel appModel, string viewName)
        {
            string styleRoot = string.Format("{0}/{1}", GlobalConst.StyleGuideRoot,
                appModel.SelectedLevelZeroItem.ColorSchemaId ?? appModel.SelectedLevelZeroItem.Id);
            appModel.StyleGuideRoot = styleRoot;

            if (string.IsNullOrWhiteSpace(appModel.LogInUrl))
            {
                appModel.SetLoginUrl();
            }

            return Json(new ResponseModel
            {
                HtmlHead = RenderPartialViewToString("HeadSection", new HeadModel(styleRoot,appModel)),
                StyleGuideRoot = styleRoot,
                GlobalNavHeader = RenderPartialViewToString(appModel.IsIframeMode ? "IframeHeader" : viewName, appModel),
                LessVariables = appModel.SelectedLevelZeroItem.LessVariables
            }.PrepareResponse(appModel.Error, Log), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            using (var sqlCon = new SqlConnection(GlobalConst.FrameworkReadConnectionStringName))
            {
                sqlCon.Open();

                const string qStr = @"SELECT APIKey, APISecret, TerminationDate, EffectiveDate FROM DPSec_APILicenses WHERE APIKey = @APIKey AND APISecret=@APISecret; RETURN;";

                using (var sqlCmd = new SqlCommand(qStr, sqlCon))
                {
                    sqlCmd.CommandType = CommandType.Text;

                    sqlCmd.Parameters.Add(new SqlParameter("@APIKey", SqlDbType.UniqueIdentifier)).Value = APIKey;
                    sqlCmd.Parameters.Add(new SqlParameter("@APISecret", SqlDbType.NVarChar, 256)).Value = APISecret;

                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.Read())
                        {
                            var terminationDate = (DateTime)sqlDr["TerminationDate"];
                            var effectiveDate = (DateTime)sqlDr["EffectiveDate"];
                            if (terminationDate.Date >= DateTime.Now.Date && effectiveDate.Date <= DateTime.Now.Date)
                                return true;
                        }
                    }
                }
            }
            AuthorizationError = "unauthorized";
            return false;
        }

        #endregion
    }
}