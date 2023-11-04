using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Services.AppUserService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerAdvanceVM;
using IK_Project.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IK_Project.UI.Areas.Admin.Controllers
{
    public class UserInformationController : AdminBaseController
    {
        readonly private IMapper _mapper;
        readonly private IAppUserService _appUserServices;
        public UserInformationController(IAppUserService appUserServices, IMapper mapper)
        {
            _appUserServices = appUserServices;
            _mapper = mapper;
        }
        public async Task<IActionResult> GetAllUserInformation()
        {

            var appUserInformations = await _appUserServices.AllAppUserInformation(x=>x.IsCall==false);
            var appUserInformationsVM= _mapper.Map<List<AppUserInformationListVM>>(appUserInformations.Data);
            return View(appUserInformationsVM);
        }
        public async Task<IActionResult> Aprove(Guid Id)
        {
            var result = await _appUserServices.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.IsCall = true;
            await _appUserServices.Edit(result.Data);
            return RedirectToAction("GetAllUserInformation", "UserInformation", new { area = "Admin" });
        }
       
    }
}
