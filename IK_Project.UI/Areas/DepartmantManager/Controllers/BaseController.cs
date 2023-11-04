using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IK_Project.UI.Areas.DepartmantManager.Controllers
{
    [Area("DepartmantManager")]
    [Authorize(Roles = "DepartmantManager")]
    public class BaseController : Controller
    {
        
    }
}
