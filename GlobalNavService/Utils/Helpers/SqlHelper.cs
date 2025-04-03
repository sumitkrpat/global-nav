using Acxiom.Web.Internal.Portal;
using Acxiom.Web.Portal;
using GlobalNavService.Models;
using GlobalNavService.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalNavService.Utils.Helpers
{
    /// <summary>
    /// SqlHelper
    /// </summary>
    public static class SqlHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets the header data.
        /// </summary>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="locality"></param>
        /// <returns></returns>
        public static ApplicationModel GetHeaderData(string linkName, string appName, string userName, string environment, string locality)
        {
            using (var sqlCon = new SqlConnection(GlobalConst.FrameworkReadConnectionStringName))
            using (var sqlCmd = new SqlCommand("", sqlCon))
            {
                sqlCon.Open();

				sqlCmd.CommandText = "DPNav_GetDataByLinkName";
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter("@LinkName", SqlDbType.NVarChar)).Value = linkName;
                sqlCmd.Parameters.Add(new SqlParameter("@AppName", SqlDbType.NVarChar)).Value = appName;
                sqlCmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = userName;
                sqlCmd.Parameters.Add(new SqlParameter("@Environment", SqlDbType.NVarChar)).Value = string.IsNullOrWhiteSpace(environment) ? GlobalConst.FrameworkEnvironment : environment;
                sqlCmd.Parameters.Add(new SqlParameter("@DefaultEnvironment", SqlDbType.NVarChar)).Value = GlobalConst.FrameworkEnvironment;
				sqlCmd.Parameters.Add(new SqlParameter("@AcxiomWwwAppName", SqlDbType.NVarChar)).Value = GlobalConst.AcxiomWwwApplicationName;
				sqlCmd.Parameters.Add(new SqlParameter("@AccountsAppName", SqlDbType.NVarChar)).Value = GlobalConst.AccountsApplicationName;
				sqlCmd.Parameters.Add(new SqlParameter("@LoginAppName", SqlDbType.NVarChar)).Value = GlobalConst.LoginApplicationName;
                sqlCmd.Parameters.Add(new SqlParameter("@LoginLinks", SqlDbType.NVarChar)).Value = string.Join(",", GlobalConst.LoginLinks);
                sqlCmd.Parameters.Add(new SqlParameter("@Locality", SqlDbType.NVarChar)).Value = locality;

                using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                {
                    return ReadResult(sqlDr);
                }
            }
        }


        /// <summary>
        /// Gets the header data.
        /// </summary>
        /// <param name="navOneId">The nav one identifier.</param>
        /// <param name="navZeroId">The nav zero identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        public static ApplicationModel GetHeaderData(int? navOneId, int navZeroId, string userName, string environment)
        {
            using (var sqlCon = new SqlConnection(GlobalConst.FrameworkReadConnectionStringName))
            using (var sqlCmd = new SqlCommand("", sqlCon))
            {
                sqlCon.Open();

				sqlCmd.CommandText = "DPNav_GetDataByLinkId";
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter("@LinkId", SqlDbType.NVarChar)).Value = navOneId;
                sqlCmd.Parameters.Add(new SqlParameter("@ParentId", SqlDbType.NVarChar)).Value = navZeroId;
                sqlCmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = userName;
				sqlCmd.Parameters.Add(new SqlParameter("@Environment", SqlDbType.NVarChar)).Value = string.IsNullOrWhiteSpace(environment) ? GlobalConst.FrameworkEnvironment : environment;
				sqlCmd.Parameters.Add(new SqlParameter("@DefaultEnvironment", SqlDbType.NVarChar)).Value = GlobalConst.FrameworkEnvironment;
				sqlCmd.Parameters.Add(new SqlParameter("@AcxiomWwwAppName", SqlDbType.NVarChar)).Value = GlobalConst.AcxiomWwwApplicationName;
				sqlCmd.Parameters.Add(new SqlParameter("@AccountsAppName", SqlDbType.NVarChar)).Value = GlobalConst.AccountsApplicationName;
				sqlCmd.Parameters.Add(new SqlParameter("@LoginAppName", SqlDbType.NVarChar)).Value = GlobalConst.LoginApplicationName;
                sqlCmd.Parameters.Add(new SqlParameter("@LoginLinks", SqlDbType.NVarChar)).Value = string.Join(",", GlobalConst.LoginLinks);

                using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                {
                    return ReadResult(sqlDr);
                }
            }
        }

        /// <summary>
        /// Gets the login data.
        /// </summary>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="locality">The locality</param>
        /// <returns></returns>
        public static LoginAppModel GetLoginData(string linkName, string environment, string locality)
        {
            using (var sqlCon = new SqlConnection(GlobalConst.FrameworkReadConnectionStringName))
            using (var sqlCmd = new SqlCommand("", sqlCon))
            {
                sqlCon.Open();

				sqlCmd.CommandText = "DPNav_GetLoginDataByLinkName";
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter("@LoginLinkName", SqlDbType.NVarChar)).Value = linkName;
                sqlCmd.Parameters.Add(new SqlParameter("@Environment", SqlDbType.NVarChar)).Value = string.IsNullOrWhiteSpace(environment) ? GlobalConst.FrameworkEnvironment : environment;
				sqlCmd.Parameters.Add(new SqlParameter("@DefaultEnvironment", SqlDbType.NVarChar)).Value = GlobalConst.FrameworkEnvironment;
				sqlCmd.Parameters.Add(new SqlParameter("@AcxiomWwwAppName", SqlDbType.NVarChar)).Value = GlobalConst.AcxiomWwwApplicationName;
				sqlCmd.Parameters.Add(new SqlParameter("@AccountsAppName", SqlDbType.NVarChar)).Value = GlobalConst.AccountsApplicationName;
				sqlCmd.Parameters.Add(new SqlParameter("@LoginAppName", SqlDbType.NVarChar)).Value = GlobalConst.LoginApplicationName;
                sqlCmd.Parameters.Add(new SqlParameter("@LoginLinks", SqlDbType.NVarChar)).Value = string.Join(",", GlobalConst.LoginLinks);
                sqlCmd.Parameters.Add(new SqlParameter("@Locality", SqlDbType.NVarChar)).Value = locality;

                using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                {
                    if (sqlDr.Read())
                    {
                        var isLogin = (bool) sqlDr["IsLogin"];

                        if (isLogin)
                        {
                            return ReadLoginResult(sqlDr);
                        }
                    }

                    return null;
                }
            }
        }

        #endregion

        #region Private Result Methods

        /// <summary>
        /// Reads the result.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static ApplicationModel ReadResult(SqlDataReader sqlDr)
        {
            if (sqlDr.Read())
            {
                var isDataCorrect = (bool)sqlDr["IsDataCorrect"];
                var isLogin = (bool)sqlDr["IsLogin"];

                if (isLogin)
                {
                    return ReadLoginResult(sqlDr);
                }

                if (isDataCorrect)
                {
                    return ReadHeaderResult(sqlDr);
                }
                return ReadErrorResult(sqlDr);
            }

            return null;
        }

        /// <summary>
        /// Reads the login result.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static LoginAppModel ReadLoginResult(SqlDataReader sqlDr)
        {
            var loginResult = new LoginAppModel();

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                loginResult.Error = !sqlDr.IsDBNull(sqlDr.GetOrdinal("ErrorCode"))
                    ? new ErrorModel { Code = (int)sqlDr["ErrorCode"], Message = sqlDr["ErrorMessage"] as string }
                    : null;
            }

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                loginResult.AcxiomCorporateUrl = !string.IsNullOrWhiteSpace(GlobalConst.AcxiomUrl)
                    ? GlobalConst.AcxiomUrl
					: sqlDr["AcxiomWwwURL"] as string;

				loginResult.AccountsUrl = sqlDr["AccountsURL"] as string;
				loginResult.LoginAppUrl = sqlDr["LoginURL"] as string;
            }

            if (sqlDr.NextResult())
            {
                loginResult.SelectedLevelZeroItem = ReadGlobalNavItem(sqlDr);
            }

            if (sqlDr.NextResult())
            {
                loginResult.SelectedLevelOneItem = ReadGlobalNavItem(sqlDr);
            }

            if (sqlDr.NextResult())
            {
                loginResult.LevelOneItems = ReadLevelOneItems(sqlDr);
            }

            if (sqlDr.NextResult())
            {
				loginResult.HelpItems = ReadHelpItems(sqlDr, loginResult.LoginAppUrl);
            }

            return loginResult;
        }

        /// <summary>
        /// Reads the error result.
        /// </summary>
        /// <param name="sqlDr">The SQL data reader.</param>
        /// <returns></returns>
        private static EmptyAppModel ReadErrorResult(SqlDataReader sqlDr)
        {
            var errorResult = new EmptyAppModel();

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                errorResult.Error = new ErrorModel
                {
                    Code = (int) sqlDr["ErrorCode"],
                    Message = sqlDr["ErrorMessage"] as string
                };
            }

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                errorResult.AcxiomCorporateUrl = !string.IsNullOrWhiteSpace(GlobalConst.AcxiomUrl)
                    ? GlobalConst.AcxiomUrl
					: sqlDr["AcxiomWwwURL"] as string;

				errorResult.AccountsUrl = sqlDr["AccountsURL"] as string;
				errorResult.LoginAppUrl = sqlDr["LoginURL"] as string;
            }

            if (sqlDr.NextResult())
            {
                errorResult.SelectedLevelZeroItem = ReadGlobalNavItem(sqlDr);
            }

            return errorResult;
        }

        /// <summary>
        /// Reads the header result.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static FullAppModel ReadHeaderResult(SqlDataReader sqlDr)
        {
            var result = new FullAppModel {LoggedUser = new UserModel()};
            var appUrl = string.Empty;

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                result.Error = !sqlDr.IsDBNull(sqlDr.GetOrdinal("ErrorCode"))
                    ? new ErrorModel {Code = (int) sqlDr["ErrorCode"], Message = sqlDr["ErrorMessage"] as string}
                    : null;
            }

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                result.AcxiomCorporateUrl = !string.IsNullOrWhiteSpace(GlobalConst.AcxiomUrl)
                    ? GlobalConst.AcxiomUrl
					: sqlDr["AcxiomWwwURL"] as string;

				result.AccountsUrl = sqlDr["AccountsURL"] as string;
				result.LoginAppUrl = sqlDr["LoginURL"] as string;
            }

            if (sqlDr.NextResult())
            {
                result.SelectedLevelZeroItem = ReadGlobalNavItem(sqlDr);
            }

            if (sqlDr.NextResult())
            {
                result.SelectedLevelOneItem = ReadGlobalNavItem(sqlDr);
            }

            if (sqlDr.NextResult())
            {
                result.LevelOneItems = ReadLevelOneItems(sqlDr);
            }

            if (sqlDr.NextResult())
            {
                result.LevelZeroItems = ReadLevelZeroItems(sqlDr);
            }

            if (sqlDr.NextResult() && sqlDr.Read())
            {
                appUrl = sqlDr["SelectedAppURL"] as string;
                result.TechSupportUrl = sqlDr["SupportUrl"] as string;
            }

            if (sqlDr.NextResult())
            {
                result.LoggedUser.Companies = ReadCompanies(sqlDr);
            }

            if (sqlDr.NextResult())
            {
                result.HelpItems = ReadHelpItems(sqlDr, appUrl);
            }

            return result;
        }

        #endregion
        
        #region Private Read Methods

        /// <summary>
        /// Reads the level one items.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static List<OneLinkData> ReadLevelOneItems(SqlDataReader sqlDr)
        {
            var levelOneItems = new List<OneLinkData>();
            while (sqlDr.Read())
            {
                levelOneItems.Add(new OneLinkData
                {
                    Id = (int) sqlDr["Id"],
                    Name = sqlDr["Name"] as string,
                    URL = sqlDr["URL"] as string,
                    DisplayName = sqlDr["DisplayName"] as string,
                    ApplicationName = sqlDr["ApplicationName"] as string
                });
            }
            return levelOneItems;
        }

        /// <summary>
        /// Reads the level zero items.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static List<ZeroLinkData> ReadLevelZeroItems(SqlDataReader sqlDr)
        {
            var levelZeroItems = new List<ZeroLinkData>();
            while (sqlDr.Read())
            {
                levelZeroItems.Add(new ZeroLinkData
                {
                    Id = (int) sqlDr["Id"],
                    Name = sqlDr["Name"] as string,
                    IconURL = sqlDr["IconURL"] as string,
                    URL = sqlDr["URL"] as string
                });
            }
            return levelZeroItems;
        }

        /// <summary>
        /// Reads the help items.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <param name="appUrl">The application URL.</param>
        /// <returns></returns>
        private static List<HelpItem> ReadHelpItems(SqlDataReader sqlDr, string appUrl)
        {
            var helpItems = new List<HelpItem>();
            while (sqlDr.Read())
            {
                var helpLocation = sqlDr["Location"] as string ?? string.Empty;

                helpItems.Add(new HelpItem
                {
                    id = int.Parse(sqlDr["HelpId"].ToString()),
                    name = sqlDr["SystemName"] as string,
                    helpLoc = (helpLocation.IsAbsoluteUrl()
                        ? helpLocation
                        : string.Format("{0}/{1}", appUrl, helpLocation.TrimStart(new[] {'~', '/'}))).TrimUrl()
                });
            }
            return helpItems;
        }

        /// <summary>
        /// Reads the companies.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static List<CompanyModel> ReadCompanies(SqlDataReader sqlDr)
        {
            var companies = new List<CompanyModel>();
            while (sqlDr.Read())
            {

                //Temporary HotFix
                //var ipList = (sqlDr["IPPattern"] as string ?? string.Empty).Split(new[] {','},
                //    StringSplitOptions.RemoveEmptyEntries).ToList();
                //var isIpAllowed = true;

                //if (ipList.Count > 0 && HttpContext.Current.Request.Headers["X-Forwarded-For"] != null)
                //{
                //    string requestIp = IPRestrictionHelper.GetIP4Address(HttpContext.Current.Request.Headers["X-Forwarded-For"]);
                    
                //    isIpAllowed = ipList.Any(ip => IPRestrictionHelper.IsSameIPAddress(ip, requestIp));
                //}
                
                //if (isIpAllowed)
                if (true)
                {
                    companies.Add(new CompanyModel
                    {
                        Id = (Guid) sqlDr["UserId"],
                        Name = sqlDr["CompanyName"] as string,
                        DisplayName = sqlDr["DisplayName"] as string,
                        IsFreemium = (bool) sqlDr["IsFreemium"]
                    });
                }
            }
            return companies.OrderBy(c=>c.DisplayName).ToList();
        }

        /// <summary>
        /// Reads the global nav item.
        /// </summary>
        /// <param name="sqlDr">The SQL dr.</param>
        /// <returns></returns>
        private static GlobalNav.Link ReadGlobalNavItem(SqlDataReader sqlDr)
        {
            if (sqlDr.Read())
            {
                return new GlobalNav.Link
                {
                    Id = (int) sqlDr["LinkId"],
                    Name = sqlDr["Name"] as string,
                    DisplayName = sqlDr["DisplayName"] as string,
                    IconURL = sqlDr["IconURL"] as string,
                    URL = sqlDr["URL"] as string,
                    Active = sqlDr["Active"] as bool?,
                    DisplayOrder = sqlDr["DisplayOrder"] as short?,
                    NavigationLevel = sqlDr["NavigationLevel"] as short?,
                    ApplicationId = sqlDr["ApplicationId"] as Guid?,
                    LessVariables = sqlDr["LessVariables"] as string,
                    ApplicationName =
                        sqlDr.HasColumn("LoweredApplicationName") ? sqlDr["LoweredApplicationName"] as string : null,
                    ColorSchemaId = sqlDr.HasColumn("ColorSchemaId") ? sqlDr["ColorSchemaId"] as int? : null
                };
            }

            throw new Exception("link is invalid");
        }

        #endregion
    }
}