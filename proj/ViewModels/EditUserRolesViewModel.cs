using Microsoft.AspNetCore.Mvc.Rendering;

namespace proj.ViewModels
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<string> SelectedRoles { get; set; }

        public EditUserRolesViewModel()
        {
            Roles = new List<SelectListItem>();
            SelectedRoles = new List<string>();
        }
    }
}
