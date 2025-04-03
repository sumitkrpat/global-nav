using Acxiom.Web.Portal;
using Acxiom.Web.Profile;
using GlobalNavService.Utils;
using GlobalNavService.Utils.Extensions;
using GlobalNavService.Utils.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace GlobalNavService.Models
{
    /// <summary>
    /// FullAppModel
    /// </summary>
    public class FullAppModel : LinkedAppModel
    {
        #region Controllers

        /// <summary>
        /// Initializes a new instance of the <see cref="FullAppModel" /> class.
        /// </summary>
        public FullAppModel()
        {
            LevelZeroItems = new List<ZeroLinkData>();
            LevelOneItems = new List<OneLinkData>();
            LoggedUser = new UserModel();
            IsCorrectData = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the application marketplace URL.
        /// </summary>
        /// <value>
        /// The application marketplace URL.
        /// </value>
        public string ApplicationMarketplaceUrl { get; set; }

        /// <summary>
        /// Gets or sets the upgrade URL.
        /// </summary>
        /// <value>
        /// The upgrade URL.
        /// </value>
        public string FreemiumUpgradeUrl { get; set; }

        /// <summary>
        /// Gets or sets the support URL.
        /// </summary>
        /// <value>
        /// The support URL.
        /// </value>
        public string TechSupportUrl { get; set; }

        /// <summary>
        /// Gets or sets the level zero items.
        /// </summary>
        /// <value>
        /// The level zero items.
        /// </value>
        public List<ZeroLinkData> LevelZeroItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tenant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is tenant; otherwise, <c>false</c>.
        /// </value>
        public bool IsTenant { get; set; }

        /// <summary>
        /// Gets or sets the log out URL.
        /// </summary>
        /// <value>
        /// The log out URL.
        /// </value>
        public string LogOutUrl { get; set; }

        /// <summary>
        /// Gets or sets the logged user.
        /// </summary>
        /// <value>
        /// The logged user.
        /// </value>
        public UserModel LoggedUser { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="env">The env.</param>
		public void PrepareModel(string applicationName, string env, string ReturnUrl, Log log)
        {
            AppName = applicationName;
            EnvName = env;
            SetLoginUrl();

            SetGlobalVariables();
            SetUserInfo(log);

            foreach (var company in LoggedUser.Companies)
            {
                company.Url = GetCompanyLogInUrl(company.Name, ReturnUrl);
            }
        }

        /// <summary>
        /// Determines whether [is working company valid].
        /// </summary>
        /// <returns></returns>
        public bool IsWorkingCompanyValid(Log log)
		{
			UserProfile profile = AuthTokenParser.GetProfile(log);
			Guid? activeTenantId = profile.WorkingCompanyId;
            return LoggedUser.Companies.Any() && activeTenantId.HasValue &&
                   LoggedUser.Companies.Any(t => t.Id == activeTenantId.Value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the global variables.
        /// </summary>
        private void SetGlobalVariables()
        {
            StyleGuideRoot = string.Format("{0}/{1}", GlobalConst.StyleGuideRoot, SelectedLevelZeroItem.Id);
            ApplicationMarketplaceUrl = GlobalConst.ApplicationMarketplaceUrl.TrimUrl();

            if (string.IsNullOrWhiteSpace(TechSupportUrl))
            {
                TechSupportUrl = GlobalConst.TechSupportUrl.TrimUrl();
            }

            LogOutUrl = (!string.IsNullOrWhiteSpace(LoginAppUrl)
				? string.Format("{0}/{1}?app={2}&env={3}", LoginAppUrl, GlobalConst.LogOutActionName, AppName,
                    !string.IsNullOrWhiteSpace(EnvName) ? EnvName : GlobalConst.FrameworkEnvironment)
                : "#").TrimUrl();
        }

        /// <summary>
        /// Sets the user information.
        /// </summary>
        private void SetUserInfo(Log log)
        {
			UserProfile profile = AuthTokenParser.GetProfile(log);

            LoggedUser.Login = profile.UserName;
            LoggedUser.Name = string.Format("{0} {1}", profile.FirstName, profile.LastName);
            LoggedUser.WorkingCompanyId = profile.WorkingCompanyId;
            var workingCompany = LoggedUser.Companies.First(c => c.Id == profile.WorkingCompanyId);
            LoggedUser.WorkingCompanyName = !string.IsNullOrWhiteSpace(profile.WorkingCompanyDisplayName)
                ? profile.WorkingCompanyDisplayName
                : workingCompany.DisplayName;
        }

        /// <summary>
        /// Gets the company log in URL.
        /// </summary>
        /// <param name="companyName">Name of the company.</param>
        /// <returns></returns>
        private string GetCompanyLogInUrl(string companyName, string ReturnUrl=null)
        {
            return
                Regex.Replace(
                    string.Format("{0}{1}{2}cid={3}", LogInUrl, LogInUrl.Contains("?") ? "&" : "?",
                     string.IsNullOrWhiteSpace(ReturnUrl) ? "" : string.Format("ReturnUrl={0}&", ReturnUrl), companyName), "([^:]/)/+", "$1").TrimUrl();
        }

        #endregion
    }
}