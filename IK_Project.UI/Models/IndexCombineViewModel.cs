using IK_Project.UI.Areas.Admin.Models.ViewModels.MenuVMs;

namespace IK_Project.UI.Models
{
    public class IndexCombineViewModel
    {

        public AppUserInformationVM? UserInformation { get; set; }
        public List<AdminMenuListVM>? MenuList { get; set; }
    }
}
