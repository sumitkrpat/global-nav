using System;
using System.Net;
using Acxiom.Web.Portal;

namespace GlobalNavService.Models
{
    /// <summary>
    /// ResponseModel class
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets the HTML head.
        /// </summary>
        /// <value>
        /// The HTML head.
        /// </value>
        public string HtmlHead { get; set; }

        /// <summary>
        /// Gets or sets the style guide root.
        /// </summary>
        /// <value>
        /// The style guide root.
        /// </value>
        public string StyleGuideRoot { get; set; }

        /// <summary>
        /// Gets or sets the global nav header.
        /// </summary>
        /// <value>
        /// The global nav header.
        /// </value>
        public string GlobalNavHeader { get; set; }

        /// <summary>
        /// Gets or sets the less variables.
        /// </summary>
        /// <value>
        /// The less variables.
        /// </value>
        public string LessVariables { get; set; }

        /// <summary>
        /// Prepares the response.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public object PrepareResponse(ErrorModel error, Log log)
        {
            if (error != null)
            {
                try
                {
                    throw new Exception(string.Format("{0}: {1}", error.Code, error.Message));
                }
                catch (Exception wex)
                {
                    log.AddException(wex);
                    return new
                    {
                        htmlHead = HtmlHead,
                        styleGuideRoot = StyleGuideRoot,
                        globalNavHeader = GlobalNavHeader,
                        lessVariables = LessVariables,
                        errorCode = error.Code,
                        errorMessage = error.Message
                    };
                }
            }

            return new
                {
                    htmlHead = HtmlHead,
                    styleGuideRoot = StyleGuideRoot,
                    globalNavHeader = GlobalNavHeader,
                    lessVariables = LessVariables
                };
        }
    }
}