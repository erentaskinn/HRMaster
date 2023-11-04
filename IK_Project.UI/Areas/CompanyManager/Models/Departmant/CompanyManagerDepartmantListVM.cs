using IK_Project.Application.Models.DTOs.CompanyDTOs;

namespace IK_Project.UI.Areas.CompanyManager.Models.Departmant
{
    public class CompanyManagerDepartmantListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
