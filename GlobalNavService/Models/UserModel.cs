using System;
using System.Collections.Generic;

namespace GlobalNavService.Models
{
    /// <summary>
    /// UserModel class
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the companies.
        /// </summary>
        /// <value>
        /// The companies.
        /// </value>
        public List<CompanyModel> Companies { get; set; }

        /// <summary>
        /// Gets or sets the working company id.
        /// </summary>
        /// <value>
        /// The working company id.
        /// </value>
        public Guid? WorkingCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the working company.
        /// </summary>
        /// <value>
        /// The name of the working company.
        /// </value>
        public string WorkingCompanyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [working company is freemium].
        /// </summary>
        /// <value>
        /// <c>true</c> if [working company is freemium]; otherwise, <c>false</c>.
        /// </value>
        public bool WorkingCompanyIsFreemium { get; set; }
    }
}