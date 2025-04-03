namespace GlobalNavService.Models
{
    /// <summary>
    /// EmptyAppModel
    /// </summary>
    public class EmptyAppModel : ApplicationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyAppModel"/> class.
        /// </summary>
        public EmptyAppModel(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyAppModel"/> class.
        /// </summary>
        /// <param name="baseModel">The base model.</param>
        public EmptyAppModel(ApplicationModel baseModel)
        {
            Error = baseModel.Error;
            AcxiomCorporateUrl = baseModel.AcxiomCorporateUrl;
			AccountsUrl = baseModel.AccountsUrl;
			LoginAppUrl = baseModel.LoginAppUrl;
            SelectedLevelZeroItem = baseModel.SelectedLevelZeroItem;
            StyleGuideRoot = baseModel.StyleGuideRoot;
            StyleGuideVersion = baseModel.StyleGuideVersion;
            LogInUrl = baseModel.LogInUrl;
            IsIframeMode = baseModel.IsIframeMode;
            IsLevelZeroMode = baseModel.IsLevelZeroMode;
            AppName = baseModel.AppName;
            EnvName = baseModel.EnvName;
            Locality = baseModel.Locality;
        }
    }
}