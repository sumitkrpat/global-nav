using System.Web;
using System.Web.Security;
using Acxiom.Web.Profile;
using Newtonsoft.Json.Linq;
using Acxiom.Web.Portal;
using System;
using System.ServiceModel;
using Acxiom.Web.Utils;

namespace GlobalNavService.Utils.Helpers
{
    public class AuthTokenParser
    {
        public static UserProfile GetProfile(Log log)
        {
			try
			{
				var fid = HttpContext.Current.User.Identity as FormsIdentity;

				if (fid != null && fid.Ticket != null && !string.IsNullOrWhiteSpace(fid.Ticket.UserData))
				{
					UserProfile profileDto = JObject.Parse(fid.Ticket.UserData)["User"].ToObject<UserProfile>();
					if (profileDto != null) 
						return profileDto;
				}
			}
			catch (Exception ex)
			{
				log.AddException(ex);
			}

			string userName = HttpContext.Current != null
				? HttpContext.Current.User.Identity.Name
				: ServiceSecurityContext.Current.PrimaryIdentity.Name;

			Acxiom.Web.Api.Proxy.DTO.User user = Proxy.ApiProxy.GetUser(userName, null).Entity;
			return new UserProfile(user);
        }
    }
}