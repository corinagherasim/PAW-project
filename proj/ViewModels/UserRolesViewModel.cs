using proj.Models;

namespace proj.ViewModels
{
    public class UserRolesViewModel
    {
        public UserCustom User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
