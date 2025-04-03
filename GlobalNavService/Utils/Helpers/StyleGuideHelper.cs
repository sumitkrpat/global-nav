using System;
using System.Configuration;
using System.Linq;

namespace GlobalNavService.Utils.Helpers
{
    public static class StyleGuideHelper
    {
        public static StyleGuideElement GetStyleGuideByVersion(string verion)
        {
            verion = verion ?? string.Empty;

            var styleGuideSection = (StyleGuideSection)ConfigurationManager.GetSection("styleGuide");

            var styleGuide = styleGuideSection.Versions.FirstOrDefault(item => item.Version.ToLower() == verion.ToLower()) ??
                             styleGuideSection.Versions.FirstOrDefault(item => item.Version.ToLower() == styleGuideSection.DefaultVersion.ToLower());

            if (styleGuide == null)
            {
                throw new Exception("StyleGuide version is invalid");
            }

            return styleGuide;
        }
    }
}