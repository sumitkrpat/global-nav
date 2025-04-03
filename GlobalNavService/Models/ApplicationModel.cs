using System.Web.Script.Serialization;
using Acxiom.Web.Internal.Portal;
using GlobalNavService.Utils;
using GlobalNavService.Utils.Extensions;

namespace GlobalNavService.Models
{
    /// <summary>
    /// IAppModel
    /// </summary>
    public class ApplicationModel
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public ErrorModel Error { get; set; }

        /// <summary>
        /// Gets or sets the acxiom corporate URL.
        /// </summary>
        /// <value>
        /// The acxiom corporate URL.
        /// </value>
        public string AcxiomCorporateUrl { get; set; }

        /// <summary>
        /// Gets or sets the accounts URL.
        /// </summary>
        /// <value>
        /// The accounts URL.
        /// </value>
        public string AccountsUrl { get; set; }

		/// <summary>
		/// Gets the URL of the application handling all logging actions (logging in, out, etc.).
		/// </summary>
		public string LoginAppUrl { get; set; }

        /// <summary>
        /// Gets or sets the selected level zero item.
        /// </summary>
        /// <value>
        /// The selected level zero item.
        /// </value>
        public GlobalNav.Link SelectedLevelZeroItem { get; set; }

        /// <summary>
        /// Gets or sets the style guide root.
        /// </summary>
        /// <value>
        /// The style guide root.
        /// </value>
        public string StyleGuideRoot { get; set; }

        /// <summary>
        /// Gets or sets the log in URL.
        /// </summary>
        /// <value>
        /// The log in URL.
        /// </value>
        public string LogInUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is iframe mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is iframe mode]; otherwise, <c>false</c>.
        /// </value>
        public bool IsIframeMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is level0 mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is level0 mode]; otherwise, <c>false</c>.
        /// </value>
        public bool IsLevelZeroMode { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the name of the env.
        /// </summary>
        /// <value>The name of the env.</value>
        public string EnvName { get; set; }

        public string StyleGuideVersion { get; set; }

        [ScriptIgnore]
        public Locality Locality { get; set; }

        /// <summary>
        /// Sets the login URL.
        /// </summary>
        public void SetLoginUrl()
        {
            SetLoginUrl(AppName, EnvName);
        }

        /// <summary>
        /// Sets the login URL.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="env">The env.</param>
        public void SetLoginUrl(string appName, string env)
        {
			if (string.IsNullOrWhiteSpace(appName))
				appName = GlobalConst.AccountsApplicationName;

			LogInUrl = (!string.IsNullOrWhiteSpace(LoginAppUrl)
				? string.Format("{0}/{1}?app={2}&env={3}", LoginAppUrl, GlobalConst.LogInActionName, appName,
					!string.IsNullOrWhiteSpace(env) ? env : GlobalConst.FrameworkEnvironment)
				: "#").TrimUrl();
        }
    }
}