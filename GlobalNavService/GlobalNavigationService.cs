/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using Newtonsoft.Json;
//using Acxiom.Automation.MyAcxiomApp.Configuration;
//using Acxiom.Automation.MyAcxiomApp.Entities;
using HtmlAgilityPack;

namespace Acxiom.Automation.MyAcxiomTest.Flows.AcxiomWebTest.API
{
    [TestFixture]
    class GlobalNavigationService
    {
        #region private members
        private IWebDriver driver;
        //private List<User> usersPositive;
        private User userAdmin;
        private User userStandard;
        private User userTest;
        #endregion private members

        public enum HeaderTypes
        {
            FullHeader,
            LoginHeader,
            EmptyHeader,
            UnknownHeader
        }

        public struct FullHeaderProperties
        {
            public bool hasHelp, hasSearchbox, hasTenantList;

            public FullHeaderProperties(bool help, bool search, bool tenantlist)
            {
                hasHelp = help;
                hasSearchbox = search;
                hasTenantList = tenantlist;
            }
        }

        public HeaderTypes GetHeaderTypeFromNavBar(string header)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(header);

            if (null != (doc.DocumentNode.SelectSingleNode("//div[@class='acxiom-div-logo-small']")) &&
                null != (doc.DocumentNode.SelectSingleNode("//span[@class='current-selection']")) &&
                null != (doc.DocumentNode.SelectSingleNode("//span[@class='acxiom-global-welcome']")) &&
                null != (doc.DocumentNode.SelectSingleNode("//a[@href='http://accounts.myacxiom.dev']")) &&
                null != (doc.DocumentNode.SelectSingleNode("//a[@href='http://accounts.myacxiom.dev/logout.aspx']")) &&
                doc.DocumentNode.SelectSingleNode("//div[@class='acxiom-topnav-tabs']").SelectNodes("//a").Count > 0
                )
            {
                /*if (null != (doc.DocumentNode.SelectSingleNode("//a[contains(text(),'Help')]")))
                {
                    return HeaderTypes.FullHeaderWithHelp;
                }#1#
                return HeaderTypes.FullHeader;
            }
            else if (null == (doc.DocumentNode.SelectSingleNode("//div[@class='acxiom-div-logo-small']")) &&
                doc.DocumentNode.SelectSingleNode("//div[@class='acxiom-topnav-tabs']").SelectNodes("//a").Count > 0 &&
                // null != (doc.DocumentNode.SelectSingleNode("//a[@href='http://accounts.myacxiom.dev/login.aspx']")) &&
                null != (doc.DocumentNode.SelectSingleNode("//a[contains(text(),'Help')]")) &&
                null != (doc.DocumentNode.SelectSingleNode("//a[contains(text(),'Password Reset']")) /*&&
                null != (doc.DocumentNode.SelectSingleNode("//a[contains(text(),'Registration']"))#1# &&
                null != (doc.DocumentNode.SelectSingleNode("//form"))
                )
            {
                return HeaderTypes.LoginHeader;
            }
            else if (doc.DocumentNode.SelectSingleNode("//a[@class='acxiom-globalnav-sign-in']").InnerText.Equals("Sign In") && doc.DocumentNode.SelectSingleNode("//div[@class='acxiom-topnav-tabs']").ChildNodes.Count < 2)
            {
                return HeaderTypes.EmptyHeader;
            }

            return HeaderTypes.UnknownHeader;
        }

        private FullHeaderProperties IdentyfyFullHeaderProperties(string header)
        {
            bool hasHelp = false, hasSearchbox = false, hasTenantList = false;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(header);

            if (null != (doc.DocumentNode.SelectSingleNode("//a[contains(text(),'Help')]")))
            {
                hasHelp = true;
            }
            if (null != (doc.DocumentNode.SelectSingleNode("//div[@class='s-tenant']")))
            {
                hasTenantList = true;
            }
            if (null != (doc.DocumentNode.SelectSingleNode("//div[@class='search']")))
            {
                hasSearchbox = true;
            }
            return new FullHeaderProperties(hasHelp, hasSearchbox, hasTenantList);
        }

        [SetUp]
        public void SetupTest()
        {
            // set the driver
            WebDriverFactory.SetDriver(Drivers.Chrome);
            driver = WebDriverFactory.Driver;
            WebDriverFactory.WaitElementTimeOut = 5;

            // load configuration elements
            userAdmin = Config.GetUserAdmin();
            userStandard = Config.GetUserStandard();
            userTest = Config.GetUserTest();

        }//Steup

        [TearDown]
        public void TeardownTest()
        {
            ValidationPointConfig.UpdateValidationPointSaasAutomated();

            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            //Assert.AreEqual(String.Empty, verificationErrors.ToString());
            Thread.Sleep(100);
        }//TearDown

        #region Auth-Token and Auhtorization headers, .DPPAUTH cookie
        [Test, Property("Id", 809), Description("Full correct authentication provided On FileCenter LogOn")]
        public void ShouldReturnSuccessOnFullCorrectAuthenticationProvidedOnFileCenterLogOn()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, !Is.StringContaining("errorCode"));
            Assert.That(resp, !Is.StringContaining("errorMessage"));
            // Final verification - not populated fields are NULLs
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.That(gnContent.errorCode, Is.Null);
            Assert.That(gnContent.errorMessage, Is.Null);
            Assert.That(gnContent.htmlHead, !Is.Null);
            Assert.AreEqual(HeaderTypes.FullHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));

            // TODO - add more assertions on returned values
        }

        [Test, Property("Id", 810), Description("Full correct authentication provided On FileCenter LogOn (with cookie provided - fixed bug")]
        public void ShouldReturnSuccessOnFullCorrectAuthenticationProvidedOnFileCenterLogOn_withCookieBug()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            // This is a bug - that cookie should not be needed!
            rest.Request.Headers.Add("Cookie", fc.dppAuthCookie.ToString());
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, !Is.StringContaining("errorCode"));
            Assert.That(resp, !Is.StringContaining("errorMessage"));
            // Final verification - not populated fields are NULLs
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.That(gnContent.errorCode, Is.Null);
            Assert.That(gnContent.errorMessage, Is.Null);
            Assert.That(gnContent.htmlHead, !Is.Null);
            // TODO - add more assertions on returned values
            Assert.AreEqual(HeaderTypes.FullHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
            FullHeaderProperties fhp = IdentyfyFullHeaderProperties(gnContent.globalNavHeader);
        }

        [Ignore("We need more control on that kind od tests - headers, textual respones etc...")]
        [Test, Property("Id", 811), Description("No authentication provided On FileCenter LogOn (Example of ServiceMapper in use)")]
        public void ShouldReturnErrorOnNoAuthenticationProvidedOnFileCenterLogOn_viaServiceMapper()
        {
            ServiceMapper<GlobalNavContentError> svc = new ServiceMapper<GlobalNavContentError>(ApiServiceConfig.URLGN,
                String.Empty, String.Empty, String.Empty);
            GlobalNavContentError gnContent = svc.SendRequest(Method.GET,
                String.Format("/{0}/files/FileCenter", StringAttribute.GetStringValue(ApiServiceVersion.V1_0)));
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 812), Description("No authentication provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnNoAuthenticationProvidedOnFileCenterLogOn()
        {
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                String.Empty, String.Empty, String.Empty);
            rest.UseBasicAuth = false;
            rest.UseToken = false;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_TOKEN), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_TOKEN), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 813), Description("No authentication provided On FileCenter LogOn - with empty Authorization")]
        public void ShouldReturnErrorOnNoAuthenticationProvidedOnFileCenterLogOn_withEmptyAuthorization()
        {
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                String.Empty, String.Empty, String.Empty);
            rest.UseBasicAuth = false;
            rest.UseToken = false;
            rest.Request.Headers.Add("Authorization", String.Empty); // Per purpose - a completely wrong header
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_TOKEN), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_TOKEN), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 814), Description("No authentication provided On FileCenter LogOn - with empty Auth-Token")]
        public void ShouldReturnErrorOnNoAuthenticationProvidedOnFileCenterLogOn_withEmptyAuthToken()
        {
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                String.Empty, String.Empty, String.Empty);
            rest.UseBasicAuth = false;
            rest.UseToken = false;
            rest.Request.Headers.Add("Auth-Token", String.Empty); // Per purpose - a completely wrong header
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_TOKEN), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_TOKEN), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 815), Description("Only a correct cookie provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyCorrectCookieProvided()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            // Adding custom cookies works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                String.Empty, String.Empty, String.Empty);
            rest.UseBasicAuth = false;
            rest.UseToken = false;
            rest.Request.Headers.Add("Cookie", fc.dppAuthCookie.ToString());
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_TOKEN), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_TOKEN), gnContent.errorMessage);
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 816), Description("Only a correct Auth-Token provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyCorrectAuthTokenProvided()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                String.Empty, String.Empty, fc.dppAuthCookie.Value);
            rest.UseBasicAuth = false;
            rest.UseToken = true;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification - not populated fields are NULLs
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        // TODO
        [Test, Manual, Description("Only a correct Auth-Token provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyCorrectAuthTokenProvided_FORACCOUNTCENTER_from_AC()
        {
            AccountCenter ac = new AccountCenter(userAdmin);
            ac.Login();
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "accounts/userInformation"),
                String.Empty, String.Empty, ac.dppAuthCookie.Value);
            rest.UseBasicAuth = false;
            rest.UseToken = true;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification - not populated fields are NULLs
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreSame(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        // TODO
        [Test, Manual, Description("Only a correct Auth-Token provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyCorrectAuthTokenProvided_FORACCOUNTCENTER_from_FC()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "accounts/userInformation"),
                String.Empty, String.Empty, fc.dppAuthCookie.Value);
            rest.UseBasicAuth = false;
            rest.UseToken = true;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification - not populated fields are NULLs
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
            // TODO - add more assertions on returned values
            Assert.AreSame(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 817), Description("Only a correct cookie and Auth-Token provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyCorrectCookieAndAuthTokenProvided()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            // Adding custom cookies works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                String.Empty, String.Empty, fc.dppAuthCookie.Value);
            rest.Request.Headers.Add("Cookie", fc.dppAuthCookie.ToString());
            rest.UseBasicAuth = false;
            rest.UseToken = true;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 818), Description("Only a correct Basic Authentication provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyBasicAuthenticationProvided()
        {
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, String.Empty);
            rest.UseBasicAuth = true;
            rest.UseToken = false;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_TOKEN), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_TOKEN), gnContent.errorMessage);
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }

        [Test, Property("Id", 819), Description("Only a correct cookie and Basic Authentication provided On FileCenter LogOn")]
        public void ShouldReturnErrorOnAuthenticationWithOnlyCorrectCookieAndBasicAuthenticationProvided()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, String.Empty);
            rest.Request.Headers.Add("Cookie", fc.dppAuthCookie.ToString());
            rest.UseBasicAuth = true;
            rest.UseToken = false;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_TOKEN), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_TOKEN), gnContent.errorMessage);
            Assert.AreEqual(HeaderTypes.EmptyHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }
        #endregion Auth-Token and Auhtorization headers, .DPPAUTH cookie

        #region API Keys valid period
        [Test, Property("Id", 820), Description("Wrong keys valid period, CurrentDate < EffectiveDate < TerminationDate")]
        public void ShouldReturnErrorOnWrongKeysValidPeriod_withCDltEDltTD()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myAppRef = apiAuth.First(key => key.app == Applications.FileCenter);

            // A copy of the exisiting entity from the DB
            ApiAuthEntityDB myAppBackup = new ApiAuthEntityDB(myAppRef.app, myAppRef.ApiKey);
            myAppBackup.download();

            ApiAuthEntityDB myApp = new ApiAuthEntityDB(myAppBackup.app, myAppBackup.ApiKey, myAppBackup.ApiSecret);
            // Dates in the future, CD < ED < TD
            DateTime dt = DateTime.Today;
            myApp.EffectiveDate = (dt.AddYears(1)).ToString("MM/dd/yyyy");
            myApp.TerminationDate = (dt.AddYears(2)).ToString("MM/dd/yyyy");
            myApp.upload();

            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            try
            {
                // Textual pre-verification
                Assert.That(resp, Is.StringContaining("errorCode"));
                Assert.That(resp, Is.StringContaining("errorMessage"));
                // Final verification
                GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
                // TODO - add more assertions on returned values
            }
            // There is a problem with the VS2010 debugger:
            // on a failed assertion that finally block won't be executed in debug mode!
            finally
            {
                // To restore values in DB
                myAppBackup.upload();
            }
        }

        [Test, Property("Id", 821), Description("Wrong keys valid period, EffectiveDate < TerminationDate < CurrentDate")]
        public void ShouldReturnErrorOnWrongKeysValidPeriod_withEDltTDltCD()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myAppRef = apiAuth.First(key => key.app == Applications.FileCenter);

            // A copy of the exisiting entity from the DB
            ApiAuthEntityDB myAppBackup = new ApiAuthEntityDB(myAppRef.app, myAppRef.ApiKey);
            myAppBackup.download();

            ApiAuthEntityDB myApp = new ApiAuthEntityDB(myAppBackup.app, myAppBackup.ApiKey, myAppBackup.ApiSecret);
            // Dates in the future, ED < TD < CD
            DateTime dt = DateTime.Today;
            myApp.EffectiveDate = (dt.AddYears(-2)).ToString("MM/dd/yyyy");
            myApp.TerminationDate = (dt.AddYears(-1)).ToString("MM/dd/yyyy");
            myApp.upload();

            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            try
            {
                // Textual pre-verification
                Assert.That(resp, Is.StringContaining("errorCode"));
                Assert.That(resp, Is.StringContaining("errorMessage"));
                // Final verification
                GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
                // TODO - add more assertions on returned values
            }
            // There is a problem with the VS2010 debugger:
            // on a failed assertion that finally block won't be executed in debug mode!
            finally
            {
                // To restore values in DB
                myAppBackup.upload();
            }
        }

        [Test, Property("Id", 822), Description("Wrong keys valid period, TerminationDate < EffectiveDate < CurrentDate")]
        public void ShouldReturnErrorOnWrongKeysValidPeriod_withTDltEDltCD()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myAppRef = apiAuth.First(key => key.app == Applications.FileCenter);

            // A copy of the exisiting entity from the DB
            ApiAuthEntityDB myAppBackup = new ApiAuthEntityDB(myAppRef.app, myAppRef.ApiKey);
            myAppBackup.download();

            ApiAuthEntityDB myApp = new ApiAuthEntityDB(myAppBackup.app, myAppBackup.ApiKey, myAppBackup.ApiSecret);
            // Dates in the future, TD < ED < CD
            DateTime dt = DateTime.Today;
            myApp.EffectiveDate = (dt.AddYears(-1)).ToString("MM/dd/yyyy");
            myApp.TerminationDate = (dt.AddYears(-2)).ToString("MM/dd/yyyy");
            myApp.upload();

            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            try
            {
                // Textual pre-verification
                Assert.That(resp, Is.StringContaining("errorCode"));
                Assert.That(resp, Is.StringContaining("errorMessage"));
                // Final verification
                GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
                // TODO - add more assertions on returned values
            }
            // There is a problem with the VS2010 debugger:
            // on a failed assertion that finally block won't be executed in debug mode!
            finally
            {
                // To restore values in DB
                myAppBackup.upload();
            }
        }

        [Test, Property("Id", 823), Description("Wrong keys valid period, CurrentDate < TerminationDate < EffectiveDate")]
        public void ShouldReturnErrorOnWrongKeysValidPeriod_withCDltTDltED()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myAppRef = apiAuth.First(key => key.app == Applications.FileCenter);

            // A copy of the exisiting entity from the DB
            ApiAuthEntityDB myAppBackup = new ApiAuthEntityDB(myAppRef.app, myAppRef.ApiKey);
            myAppBackup.download();

            ApiAuthEntityDB myApp = new ApiAuthEntityDB(myAppBackup.app, myAppBackup.ApiKey, myAppBackup.ApiSecret);
            // Dates in the future, CD < TD < ED
            DateTime dt = DateTime.Today;
            myApp.EffectiveDate = (dt.AddYears(2)).ToString("MM/dd/yyyy");
            myApp.TerminationDate = (dt.AddYears(1)).ToString("MM/dd/yyyy");
            myApp.upload();

            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            try
            {
                // Textual pre-verification
                Assert.That(resp, Is.StringContaining("errorCode"));
                Assert.That(resp, Is.StringContaining("errorMessage"));
                // Final verification
                GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.USER_UNAUTHORIZED), gnContent.errorCode);
                Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.USER_UNAUTHORIZED), gnContent.errorMessage);
                // TODO - add more assertions on returned values
            }
            // There is a problem with the VS2010 debugger:
            // on a failed assertion that finally block won't be executed in debug mode!
            finally
            {
                // To restore values in DB
                myAppBackup.upload();
            }
        }

        [Test, Property("Id", 824), Description("Correct keys valid period, EffectiveDate < CurrentDate < TerminationDate ")]
        public void ShouldReturnSuccessOnCorrectKeysValidPeriod_withEDltCDltTD()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myAppRef = apiAuth.First(key => key.app == Applications.FileCenter);

            // A copy of the exisiting entity from the DB
            ApiAuthEntityDB myAppBackup = new ApiAuthEntityDB(myAppRef.app, myAppRef.ApiKey);
            myAppBackup.download();

            ApiAuthEntityDB myApp = new ApiAuthEntityDB(myAppBackup.app, myAppBackup.ApiKey, myAppBackup.ApiSecret);
            // Dates in the future, ED < CD < TD
            DateTime dt = DateTime.Today;
            myApp.EffectiveDate = (dt.AddYears(-1)).ToString("MM/dd/yyyy");
            myApp.TerminationDate = (dt.AddYears(1)).ToString("MM/dd/yyyy");
            myApp.upload();

            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            try
            {
                // Textual pre-verification
                Assert.That(resp, !Is.StringContaining("errorCode"));
                Assert.That(resp, !Is.StringContaining("errorMessage"));
                // Final verification - not populated fields are NULLs
                GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
                Assert.That(gnContent.errorCode, Is.Null);
                Assert.That(gnContent.errorMessage, Is.Null);
                Assert.That(gnContent.htmlHead, !Is.Null);
                // TODO - add more assertions on returned values
            }
            // There is a problem with the VS2010 debugger:
            // on a failed assertion that finally block won't be executed in debug mode!
            finally
            {
                // To restore values in DB
                myAppBackup.upload();
            }
        }
        #endregion API Keys valid period

        #region Application names, links and links relationship
        /*
         * John Riewerts explained on 2013-07-24, that the only supported way 
         * to address resources form GlobalNav service should be this:
         * 
         *   GetGlobalNav(string LinkName, string ApplicationName);
         * 
         * where those two elements should be connected (that means, that
         * the link belongs to the application).
         #1#

        [Test, Property("Id", 825), Description("Wrong application name and correct link name")]
        public void ShouldReturnErrorOnNonExistingAppAndExistingLink()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "filesTHEREISNOSUCHAPP/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_APP), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_APP), gnContent.errorMessage);
            // TODO - add more assertions on returned values         
        }

        [Test, Property("Id", 826), Description("Wrong application name and wrong link name")]
        public void ShouldReturnErrorOnNonExistingAppAndNonExistingLink()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "filesTHEREISNOSUCHAPP/FileCenterTHEREISNOSUCHLINK"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_LINK_NAME), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_LINK_NAME), gnContent.errorMessage);
            // TODO - add more assertions on returned values  
        }

        [Test, Property("Id", 827), Description("Correct application name and wrong link name")]
        public void ShouldReturnErrorOnExistingAppAndAndNonExistingLink()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenterTHEREISNOSUCHLINK"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_LINK_NAME), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_LINK_NAME), gnContent.errorMessage);
            // TODO - add more assertions on returned values  
        }

        [Category("AskedForClarificationAboutTheCurrentErrorMessage_20130725")]
        [Test, Property("Id", 828), Description("Correct application name and missing link name")]
        public void ShouldReturnErrorOnExistingAppAndAndMissingLink()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("The remote server returned an error: (404) Not Found."));
            // TODO - add more assertions on returned values  

            /*
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_LINK_NAME), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_LINK_NAME), gnContent.errorMessage);
            // TODO - add more assertions on returned values               
            #1#
        }

        [Test, Property("Id", 829), Description("Environment not set up for an application")]
        public void ShouldReturnErrorOnNonExistingEnvironment()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter/?env=THEREISNOSUCHENV"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.ENV_NOT_SETUP), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.ENV_NOT_SETUP), gnContent.errorMessage);
            // TODO - add more assertions on returned values  
        }

        [Test, Property("Id", 830), Description("Invalid Parent/Child link relationship (built with an existing app and link and existing, but unrelated navZeroId)")]
        public void ShouldReturnErrorOnInvalidParentChildLinkRelationship_withExistingNavZeroId()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.

            // Explanation: Application 'File Center' (files) has link FileCenter but the NavZeroId is not 0 for that app:
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter/0"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_RELATIONSHIP), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_RELATIONSHIP), gnContent.errorMessage);
            // TODO - add more assertions on returned values  
        }

        [Test, Property("Id", 831), Description("Invalid Parent/Child link relationship (built with an existing app and link and nonexisting navZeroId)")]
        public void ShouldReturnErrorOnInvalidParentChildLinkRelationship_withNonExistingNavZeroId()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.

            // Explanation: Application 'File Center' (files) has link FileCenter but the NavZeroId=1000 does not exist:
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter/1000"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_RELATIONSHIP), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_RELATIONSHIP), gnContent.errorMessage);
            // TODO - add more assertions on returned values  
        }

        [Category("Bug_D04200")]
        [Category("Execution_blocked_by_not_working_FileCenter_in_NewGlobalNav")]
        [Test, Property("Id", 832), Description("Invalid Parent/Child link relationship (built with an existing app and existing, but unrelated link)")]
        public void ShouldReturnErrorOnInvalidParentChildLinkRelationship_withNonRelatedLink()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.

            // Explanation: Application 'File Center' (files) does not own the 'userInformation' link, but it exists in the system:
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/userInformation"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_RELATIONSHIP), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_RELATIONSHIP), gnContent.errorMessage);
            // TODO - add more assertions on returned values              
        }
        #endregion Application names, links and links relationship

        #region Mix of existing but not linked API keys, Auth-Tokens, applications and users
        [Test, Property("Id", 833), Description("Auth-Token from a correct user app and resource, but used with API key of another app")]
        public void ShouldReturnErrorOnAPIKeyFromAnotherApplication()
        {
            FileCenter fc = new FileCenter(userAdmin);
            fc.Login();
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.AccountCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.

            // Explanation: Auth-Token is from File Center, and API Key/Secret from Account Center
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/userInformation"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.INVALID_KEY_FOR_APP), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.INVALID_KEY_FOR_APP), gnContent.errorMessage);
            // TODO - add more assertions on returned values             
        }

        [Test, Property("Id", 834), Description("Auth-Token from a correct user and app, but used for another app (API Key/Secret and resource) for which that user doesn't have access")]
        public void ShouldReturnErrorOnAppThatUserDoesNotHaveAccess()
        {
            /*
             * Problem [2013-07-29]
             * Since we need an application which has the UserRoleNotRequired=FALSE,
             * and the FileCenter application has that role set to TRUE, we have to 
             * temporarily change that option to FALSE, run our test and the restore
             * that option back to TRUE. 
             * This is a workaround, until we have good application which does not
             * require that option set to TRUE by default.
             #1#

            // Temporary setting UserRoleNotRequired to false
            ApplicationDB fcdbBackup = new ApplicationDB(Config.GetAppFileCenter().Name, Config.GetAppFileCenter().ApplicationId);
            fcdbBackup.download();
            ApplicationDB fcdb = new ApplicationDB(fcdbBackup.Name, fcdbBackup.ApplicationId);
            fcdb.IsUserRoleNotRequired = false;
            fcdb.upload();

            AccountCenter ac = new AccountCenter(userTest);
            ac.Login();

            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.

            // Explanation: that user doesn't have access to the FileCenter app now (with the temporaray workaround)
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, ac.dppAuthCookie.Value);
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);

            // Restoring UserRoleNotRequired
            fcdbBackup.upload();

            // Textual pre-verification
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorCodes.ACCESS_DENIED), gnContent.errorCode);
            Assert.AreEqual(StringAttribute.GetStringValue(GlobalNavErrorMessages.ACCESS_DENIED), gnContent.errorMessage);
            // TODO - add more assertions on returned values             
        }
        #endregion Mixes of existing but not linked API keys, Auth-Tokens and applications


        #region pstepn_tests_for_proper globalnaverrormsg
        public void TestWrongAppId(int errorCode, string errorMsg, HeaderTypes expectedHeader)
        {
            // FileCenter fc = new FileCenter(userAdmin);
            // fc.Login();
            AccountCenter fc = AccountCenter.AccountCenterLogin(userAdmin);
            List<ApiAuthEntity> apiAuth = ApiServiceConfig.ApplicationsPositive();
            ApiAuthEntity myApp = apiAuth.First(key => key.app == Applications.FileCenter);
            ApiAuthEntity otherApp = apiAuth.First(key => key.app == Applications.AccountCenter);
            // Extracting textual content works on a Rest level (now), not on the ServiceMapper level
            // TODO - work with Tomasz on changing that in the future.
            Rest rest = new Rest(String.Format("{0}/{1}/{2}",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0), "files/FileCenter"),
                myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
            //rest.Request.Headers.Add("Cookie", fc.dppAuthCookie.ToString());
            string poo = String.Format("{0}/{1}/",
                ApiServiceConfig.URLGN, StringAttribute.GetStringValue(ApiServiceVersion.V1_0));
            switch (errorMsg)
            {
                case "poo":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FileCenter"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "invalid application":
                    rest = new Rest(String.Format("{0}{1}", poo, "awerb7"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "invalid link name or id":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FilCenter"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "environment not setup for application":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FileCenter?env=dev"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "invalid parent/child link relationship":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FileCenter"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "unathorized":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FileCenter"),
                        myApp.ApiKey+"d", myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "invalid token":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FileCenter"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString()+"t");
                    break;
                case "application key invalid for specified application":
                    rest = new Rest(String.Format("{0}{1}", poo, "files/FileCenter"),
                        myApp.ApiSecret, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
                case "access denied for the specified user":
                    rest = new Rest(String.Format("{0}{1}", poo, "awerb7"),
                        myApp.ApiKey, myApp.ApiSecret, fc.dppAuthCookie.ToString());
                    break;
            }

            rest.UseBasicAuth = true;
            rest.UseToken = true;
            string resp = rest.SendRequest(Method.GET, ContentType.JSON);
            //resp = resp.Replace(@"\r\n",Environment.NewLine);
            // Textual pre-verification
            if (resp.Contains("404")) return;
            Assert.That(resp, Is.StringContaining("errorCode"));
            Assert.That(resp, Is.StringContaining("errorMessage"));
            int ppp = resp.IndexOf("errorCode");
            string substr = resp.Substring(ppp);

            Assert.True(resp.Contains(String.Format("errorCode\":{0}",errorCode.ToString())));
            Assert.True(resp.Contains(String.Format("\"errorMessage\":\"{0}\"",errorMsg)));
            // Final verification
            GlobalNavContentError gnContent = JsonConvert.DeserializeObject<GlobalNavContentError>(resp);
            Assert.AreEqual(expectedHeader, GetHeaderTypeFromNavBar(gnContent.globalNavHeader));
        }
        #endregion

        //TODO
        [Test, Description("AOS-384")]
        public void TestOf_AOS_384()
        {
            /*
             * [marako]
             * Paul, John,
             * I have a question about this request: “Remove API Key check against the request application name from GlobalNav”
             * In JIRA: AOS-384, status Resolved, resolution: Fixed.
             * Could you please tell us how it should work now? and/or how to test this?
             * For now in DEV/CERT env I can send request with empty api key, api secret and auth-token and I’m not receiving any error. Is it ok?
             * 
             * [popala]
             * Hey Mateusz,
             * Sending empty values is ok, you will just receive a default header with “Login” and “Reset Password” links.  
             * The request you mention below allows any application with a valid API key to generate a header for all valid applications in the system, not just itself.
             * Another words all we check now is whether the API key/secret are valid and nothing else.  
             * InsightLab can request a header generated for AccountCenter and we will generated it as long as the API Key/Secret are valid.
             * 
             * [jriewe]
             * “The acceptance criteria here is that you should be able to use an API key from one app and make a call fro global nav of another app.
             * I.e. Data loader application's API key can be used to retrieve a global nav that is tied to insight lab.
             * 
             #1#
            Assert.That(false);
        }

    }
}
*/
