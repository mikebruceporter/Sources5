using System.Collections.Generic;

namespace Sources5.Models
{
    public class UserInfoFormViewModel
    {
        public UserInfo userinfo { get; set; }
        public IEnumerable<Country> countries { get; set; }
    }
}
