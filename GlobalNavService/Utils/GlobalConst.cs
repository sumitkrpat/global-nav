using System;
using System.Collections;
using System.Configuration;
using System.Linq;

namespace GlobalNavService.Utils
{
    /// <summary>
    /// GlobalConst class
    /// </summary>
    public class GlobalConst
    {
        /// <summary>
        /// Gets the name of the framework read connection string.
        /// </summary>
        /// <value>
        /// The name of the framework read connection string.
        /// </value>
        public static string FrameworkReadConnectionStringName
        {
            get
            {
                var section = ConfigurationManager.GetSection("acxiom.web/framework") as Hashtable;
                var frameworkReadConnectionStringName = section != null
                    ? (string) section["frameworkReadConnectionStringName"]
                    : string.Empty;
                var conStringRead = !string.IsNullOrEmpty(frameworkReadConnectionStringName)
                    ? ConfigurationManager.ConnectionStrings[frameworkReadConnectionStringName]
                    : null;
                return conStringRead != null ? conStringRead.ConnectionString : string.Empty;
            }
        }

        /// <summary>
        /// Gets the framework environment.
        /// </summary>
        /// <value>
        /// The framework environment.
        /// </value>
        public static string FrameworkEnvironment
        {
            get
            {
                var section = ConfigurationManager.GetSection("acxiom.web/framework") as Hashtable;
                return section != null
                    ? section["environment"] as string
                    : string.Empty;
            }
        }

        /// <summary>
        /// Gets the style guide root.
        /// </summary>
        /// <value>
        /// The style guide root.
        /// </value>
        public static string StyleGuideRoot
        {
            get { return ConfigurationManager.AppSettings["StyleGuideRoot"] ?? string.Empty; }
        }

		public static string LoginApplicationName
		{
			get { return ConfigurationManager.AppSettings["LoginApplicationName"] ?? string.Empty; }
		}

		public static string AccountsApplicationName
		{
			get { return ConfigurationManager.AppSettings["AccountsApplicationName"] ?? string.Empty; }
		}

		public static string AcxiomWwwApplicationName
		{
			get { return ConfigurationManager.AppSettings["AcxiomWwwApplicationName"] ?? string.Empty; }
		}

		public static string LogOutActionName
		{
			get { return ConfigurationManager.AppSettings["LogOutActionName"] ?? string.Empty; }
		}

		public static string LogInActionName
		{
			get { return ConfigurationManager.AppSettings["LogInActionName"] ?? string.Empty; }
		}

        public static string[] LoginLinks
        {
            get
            {
                return (ConfigurationManager.AppSettings["LoginLinks"] ?? string.Empty)
                    .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLower()).ToArray();
            }
        }

        /// <summary>
        /// Gets the name of the app.
        /// </summary>
        /// <value>
        /// The name of the app.
        /// </value>
        public static string Component
        {
            get { return "GlobalNav"; }
        }

        /// <summary>
        /// Gets the application marketplace URL.
        /// </summary>
        /// <value>
        /// The application marketplace URL.
        /// </value>
        public static string ApplicationMarketplaceUrl
        {
            get { return ConfigurationManager.AppSettings["ApplicationMarketplaceUrl"] ?? string.Empty; }
        }

        /// <summary>
        /// Gets the tech support URL.
        /// </summary>
        /// <value>
        /// The tech support URL.
        /// </value>
        public static string TechSupportUrl
        {
            get { return ConfigurationManager.AppSettings["TechSupportUrl"] ?? string.Empty; }
        }

        /// <summary>
        /// Gets the ga account.
        /// </summary>
        /// <value>
        /// The ga account.
        /// </value>
        public static string GaAccount
        {
            get { return ConfigurationManager.AppSettings["GaAccount"] ?? string.Empty; }
        }

        /// <summary>
        /// Gets the name of the ga domain.
        /// </summary>
        /// <value>
        /// The name of the ga domain.
        /// </value>
        public static string GaDomainName
        {
            get { return ConfigurationManager.AppSettings["GaDomainName"] ?? string.Empty; }
        }

        /// <summary>
        /// Gets the default help URL.
        /// </summary>
        /// <value>
        /// The default help URL.
        /// </value>
        public static string DefaultHelpUrl
        {
            get { return ConfigurationManager.AppSettings["DefaultHelpUrl"] ?? string.Empty; }
        }

        /// <summary>
        /// Gets the acxiom URL.
        /// </summary>
        /// <value>The acxiom URL.</value>
        public static string AcxiomUrl
        {
            get { return ConfigurationManager.AppSettings["AcxiomUrl"] ?? string.Empty; }
        }

        /// <summary>
        /// Gets the name of the iframe mode.
        /// </summary>
        /// <value>
        /// The name of the iframe mode.
        /// </value>
        public static string IframeModeName
        {
            get { return "iframe"; }
        }

        /// <summary>
        /// Gets the name of the level0 mode.
        /// </summary>
        /// <value>
        /// The name of the level0 mode.
        /// </value>
        public static string LevelZeroModeName
        {
            get { return "level0"; }
        }

        public static string ScriptUrl
        {
            get { return ConfigurationManager.AppSettings["GlobalNavScriptUrl"] ?? string.Empty; }
        }

        public static string DefaultLocality
        {
            get { return ConfigurationManager.AppSettings["DefaultLocality"] ?? "en-US"; }
        }
    }
}