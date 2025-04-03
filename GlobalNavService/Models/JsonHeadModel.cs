using GlobalNavService.Utils;

namespace GlobalNavService.Models
{
    /// <summary>
    /// JsonHeadModel
    /// </summary>
    public class JsonHeadModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonHeadModel" /> class.
        /// </summary>
        /// <param name="styleGuideRoot">The style guide root.</param>
        /// <param name="applicationModel">The application model.</param>
        public JsonHeadModel(string styleGuideRoot, ApplicationModel applicationModel)
        {
            StyleGuidRoot = styleGuideRoot;
            GaAccount = GlobalConst.GaAccount;
            GaDomainName = GlobalConst.GaDomainName;
            Application = applicationModel.AppName;
            Environment = applicationModel.EnvName;
            LoginUrl = applicationModel.LogInUrl;
            ApplicationModel = applicationModel;
        }

        /// <summary>
        /// Gets or sets the style GUID root.
        /// </summary>
        /// <value>
        /// The style GUID root.
        /// </value>
        public string StyleGuidRoot { get; set; }

        /// <summary>
        /// Gets or sets the ga account.
        /// </summary>
        /// <value>
        /// The ga account.
        /// </value>
        public string GaAccount { get; set; }

        /// <summary>
        /// Gets or sets the name of the ga domain.
        /// </summary>
        /// <value>
        /// The name of the ga domain.
        /// </value>
        public string GaDomainName { get; set; }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>The application.</value>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the login URL.
        /// </summary>
        /// <value>The login URL.</value>
        public string LoginUrl { get; set; }

        public ApplicationModel ApplicationModel { get; set; }
    }
}