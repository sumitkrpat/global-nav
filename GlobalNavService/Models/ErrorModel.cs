namespace GlobalNavService.Models
{
    /// <summary>
    /// ErrorModel class
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string Message { get; set; }
    }
}