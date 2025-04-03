using System.Collections.Generic;

namespace GlobalNavService.Models
{
    /// <summary>
    /// LoginAppModel
    /// </summary>
    public class LoginAppModel : LinkedAppModel
    {
        public LoginAppModel()
        {
            LevelOneItems = new List<OneLinkData>();
        }
    }
}