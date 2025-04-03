using System.Collections.Specialized;
using GlobalNavService.Models;
using GlobalNavService.Utils;
using GlobalNavService.Utils.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GlobalNavService.Controllers
{
    /// <summary>
    /// Navigation controller
    /// </summary>
    public class NavigationController : BaseController
    {
        /// <summary>
        /// Gets the name of the header by.
        /// </summary>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="navZeroId">The nav zero id.</param>
        /// <param name="env">The env.</param>
        /// <param name="sgversion">The version of the StyleGuide.</param>
        /// <param name="locality">The locality of the GlobalNav</param>
        /// <param name="returnUrl">The URL which will be used for redirect</param>
        /// <param name="mode">The mode.</param>
        /// <param name="help">if set to <c>true</c> [help].</param>
        /// <param name="tenant">if set to <c>true</c> [tenant].</param>
        /// <returns></returns>
        public ActionResult GetHeaderByName(string linkName, string applicationName, int? navZeroId, string env,
            string sgversion, string locality, string returnUrl = null, string mode = "full", bool help = false, bool tenant = true)
        {
            Log.BaseFields.ApplicationName = applicationName;
            if (!string.IsNullOrWhiteSpace(env))
            {
                Log.BaseFields.Environment = env;
            }
            Log.AddCustomInfo(new NameValueCollection
            {
                {"linkName", linkName},
                {"navZeroId", navZeroId.ToString()},
                {"help", help.ToString()},
                {"sgversion", sgversion},
                {"showTenant", tenant.ToString()}
            });

            try
            {
                var headerLocality = new Locality(Locality.IsAuthLocality(locality) && (!IsAuthorized || !User.Identity.IsAuthenticated) ? null : locality, Log);

                if (applicationName.ToLower() == GlobalConst.LoginApplicationName && GlobalConst.LoginLinks.Contains(linkName.ToLower()))
                {
                    return LoginHeaderResult(linkName, env, sgversion, help, headerLocality);
                }

                ApplicationModel headerData = SqlHelper.GetHeaderData(linkName, applicationName, User.Identity.Name, env, headerLocality.LocalityName);

                headerData.Locality = headerLocality;
                headerData.StyleGuideVersion = sgversion;
                headerData.EnvName = !string.IsNullOrWhiteSpace(env) ? env : GlobalConst.FrameworkEnvironment;
                headerData.AppName = applicationName;
                headerData.IsIframeMode = String.Equals(mode, GlobalConst.IframeModeName,
                    StringComparison.CurrentCultureIgnoreCase);
                headerData.IsLevelZeroMode = String.Equals(mode, GlobalConst.LevelZeroModeName,
                    StringComparison.CurrentCultureIgnoreCase);
                return PrepareResult(headerData, applicationName, env, returnUrl, help, tenant);
            }
            catch (Exception ex)
            {
                Log.AddException(ex);
                throw;
            }
            finally
            {
                Log.Transaction();
            }
        }

        #region Private result methods

        /// <summary>
        /// Prepares the result.
        /// </summary>
        /// <param name="headerData">The header data.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="env">The env.</param>
        /// <param name="help">if set to <c>true</c> [help].</param>
        /// <param name="tenant">if set to <c>true</c> [tenant].</param>
        /// <returns></returns>
        private ActionResult PrepareResult(ApplicationModel headerData, string applicationName, string env, string ReturnUrl=null, bool help = false, bool tenant = true)
        {
            if (headerData is LoginAppModel)
            {
                return LoginHeaderResult(headerData as LoginAppModel);
            }

            if (!IsAuthorized || !User.Identity.IsAuthenticated)
            {
                return AuthorizationErrorResult(headerData);
            }

            if (headerData is FullAppModel)
            {
                var headerModel = headerData as FullAppModel;

                if (!headerModel.IsWorkingCompanyValid(Log))
                {
                    var emptyAppModel = new EmptyAppModel(headerData)
                    {
                        Error = new ErrorModel
                        {
                            Code = 403,
                            Message = "access denied for the specified user"
                        }
                    };
                    return EmptyHeaderResult(emptyAppModel);
                }

                headerModel.IsHelp = help;
                headerModel.IsTenant = tenant;
                headerModel.AppName = applicationName;
                headerModel.EnvName = env;
                headerModel.PrepareModel(string.IsNullOrWhiteSpace(applicationName) ? headerModel.SelectedLevelOneItem.ApplicationName : applicationName, 
					env, ReturnUrl, Log);

                return HeaderResult(headerModel);
            }

            if (headerData is EmptyAppModel)
            {
                var emptyAppModel = headerData as EmptyAppModel;

                return EmptyHeaderResult(emptyAppModel);
            }

            var model = new EmptyAppModel(headerData)
            {
                Error = new ErrorModel
                {
                    Code = 500,
                    Message = "internal server error"
                }
            };

            return EmptyHeaderResult(model);
        }

        /// <summary>
        /// Authorizations the error result.
        /// </summary>
        /// <param name="headerData">The header data.</param>
        /// <returns></returns>
        private ActionResult AuthorizationErrorResult(ApplicationModel headerData)
        {
            var emptyAppModel = new EmptyAppModel(headerData)
            {
                Error = new ErrorModel
                {
                    Code = 401,
                    Message = AuthorizationError ?? (!IsAuthorized ? "unauthorized" : "user token is invalid")
                }
            };
            return EmptyHeaderResult(emptyAppModel);
        }

        /// <summary>
        /// Logins the header result.
        /// </summary>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="sg"></param>
        /// <param name="help"></param>
        /// <param name="locality"></param>
        /// <returns></returns>
        private ActionResult LoginHeaderResult(string linkName, string environment, string sg, bool help, Locality locality)
        {
            LoginAppModel loginData = SqlHelper.GetLoginData(linkName, environment, locality.LocalityName);
            loginData.StyleGuideVersion = sg;
            loginData.IsHelp = help;
            loginData.Locality = locality;

            return LoginHeaderResult(loginData);
        }

        /// <summary>
        /// Logins the header result.
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <returns></returns>
        private ActionResult LoginHeaderResult(LoginAppModel loginModel)
        {
            loginModel.IsIframeMode = false;
            loginModel.IsLevelZeroMode = false;
            return FormatResult(loginModel, "LoginHeader");
        }

        /// <summary>
        /// Empties the header result.
        /// </summary>
        /// <param name="emptyModel">The empty model.</param>
        /// <returns></returns>
        private ActionResult EmptyHeaderResult(EmptyAppModel emptyModel)
        {
            return FormatResult(emptyModel, "EmptyHeader");
        }

        /// <summary>
        /// Headers the result.
        /// </summary>
        /// <param name="appModel">The application model.</param>
        /// <returns></returns>
        private ActionResult HeaderResult(FullAppModel appModel)
        {
            return FormatResult(appModel, "Index");
        }

        protected new ActionResult FormatResult(ApplicationModel appModel, string viewName)
        {
            StyleGuideElement styleGuide = StyleGuideHelper.GetStyleGuideByVersion(appModel.StyleGuideVersion);

            string styleRoot = string.Format("{0}/{1}", styleGuide.Url,
                appModel.SelectedLevelZeroItem.ColorSchemaId ?? appModel.SelectedLevelZeroItem.Id);

            appModel.StyleGuideRoot = styleRoot;
            appModel.StyleGuideVersion = styleGuide.Version;

            if (string.IsNullOrWhiteSpace(appModel.LogInUrl))
            {
                appModel.SetLoginUrl();
            }

            return Json(new ResponseModel
            {
                HtmlHead = RenderPartialViewToString("HeadSection", new JsonHeadModel(styleRoot, appModel)),
                StyleGuideRoot = styleRoot,
                GlobalNavHeader = RenderPartialViewToString("IndexJson"),
                LessVariables = appModel.SelectedLevelZeroItem.LessVariables
            }.PrepareResponse(appModel.Error, Log), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}