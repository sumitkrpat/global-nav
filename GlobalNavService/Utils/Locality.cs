using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using GlobalNavService.Utils.Helpers;
using Acxiom.Web.Portal;

namespace GlobalNavService.Utils
{
    public class Locality
    {
        private static readonly List<string> SupportedLocalities;

        static Locality()
        {
            SupportedLocalities =
                Directory.GetFiles(HttpContext.Current.Server.MapPath(ResourcesPath), "*.json")
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();
        }

        private const string ResourceFile = "~/Resources/{0}.json";

        private const string ResourcesPath = "~/Resources/";

        private const string AuthorizationFlag = "auth";

        public Locality(string locality, Log log)
        {
            locality = locality ?? string.Empty;

            if (IsAuthLocality(locality))
            {
				var userProfile = AuthTokenParser.GetProfile(log);
                locality = userProfile.CultureType;
            }

            var language = locality.Contains("-") ? Regex.Match(locality, "^([^-]+)-.*$").Groups[1].Value : locality;

            var supportedLocality =
                SupportedLocalities.FirstOrDefault(l => l.StartsWith(locality, StringComparison.OrdinalIgnoreCase));

            var supportedLanguage =
                SupportedLocalities.FirstOrDefault(l => l.StartsWith(language, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(supportedLocality) && string.IsNullOrWhiteSpace(supportedLanguage))
            {
                LocalityName = GlobalConst.DefaultLocality;
            }
            else
            {
                LocalityName = string.IsNullOrWhiteSpace(supportedLocality) ? supportedLanguage : supportedLocality;
            }
        }

        public string LocalityName { get; private set; }

        public string LocalityData
        {
            get
            {
                return File.ReadAllText(HttpContext.Current.Server.MapPath(string.Format(ResourceFile, LocalityName)));
            }
        }

        public override string ToString()
        {
            return LocalityName;
        }

        public static bool IsAuthLocality(string locality)
        {
            return string.Equals(locality, AuthorizationFlag, StringComparison.OrdinalIgnoreCase);
        }
    }
}