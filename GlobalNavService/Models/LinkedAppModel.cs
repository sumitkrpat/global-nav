using System.Collections.Generic;
using Acxiom.Web.Internal.Portal;
using Acxiom.Web.Portal;

namespace GlobalNavService.Models
{
    /// <summary>
    /// LinkedAppModel
    /// </summary>
    public class LinkedAppModel : ApplicationModel
    {
        /// <summary>
        /// Gets or sets the level one items.
        /// </summary>
        /// <value>
        /// The level one items.
        /// </value>
        public List<OneLinkData> LevelOneItems { get; set; }

        /// <summary>
        /// Gets or sets the selected level one item.
        /// </summary>
        /// <value>
        /// The selected level one item.
        /// </value>
        public GlobalNav.Link SelectedLevelOneItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is correct data.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is correct data; otherwise, <c>false</c>.
        /// </value>
        public bool IsCorrectData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is help.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is help; otherwise, <c>false</c>.
        /// </value>
        public bool IsHelp { get; set; }

        /// <summary>
        /// Gets or sets the help items.
        /// </summary>
        /// <value>
        /// The help items.
        /// </value>
        public List<HelpItem> HelpItems { get; set; }
    }
}