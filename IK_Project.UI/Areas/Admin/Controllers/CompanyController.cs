using AutoMapper;
using Humanizer;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.MenuService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyVM;
using IK_Project.UI.FluentValidations.AdminAreaValidator.CompanyValidator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace IK_Project.UI.Areas.Admin.Controllers
{

    public class CompanyController : AdminBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        //private readonly IAppUserService _appUserService;
        IWebHostEnvironment _webHostEnvironment;
        private readonly IMenuService _menuService;

        public CompanyController(ICompanyService companyService, IMapper mapper, IWebHostEnvironment webHostEnvironment, IMenuService menuService)
        {
            _companyService = companyService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _menuService = menuService;

        }

        public IActionResult Index()
        {
            return View();
        }


        //Aktif ve Aktif olmayan Company Listeleme
        public async Task<IActionResult> GetAllActiveAndDeactiveCompanies()
        {
            var result = await _companyService.GetDefaults(x => x.IsActive == true || x.IsActive == false);
            if (result.Data == null)
            {
                return View();
            }
            var listVM = _mapper.Map<List<AdminCompanyListVM>>(result.Data);

            return View(listVM);
        }
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyService.AllCompanies();
            if (result.Data==null)
            {
                return View();
            }
            List<AdminCompanyListVM> companyList = _mapper.Map<List<AdminCompanyListVM>>(result.Data);
            return View(companyList);


        }

        //Company Ekleme
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AdminCompanyCreateVM createVm = new AdminCompanyCreateVM()
            {
                MenuList = await GetMenuSelectListAsync(),
            };

            return View(createVm);

        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCompanyCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var menu = await _menuService.GetById(vm.MenuId);
                vm.DealEndDate = vm.DealStartDate.AddMonths(menu.Data.Period);
                var dto = _mapper.Map<CompanyCreateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    dto.Logo = savedFilePath;
                }
                var result = await _companyService.Create(dto);
                if (result.IsSuccess)
                {
                    return RedirectToAction("GetAllCompanies");

                }


            }
            vm.MenuList = await GetMenuSelectListAsync();

            return View(vm);
        }
        

        //Toggle Active DeActive
        //[HttpGet]
        //public async Task<IActionResult> ActiveDeActive(int id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        CompanyUpdateVM company = _mapper.Map<CompanyUpdateVM>(await _companyService.GetById(id));

        //        return View(ActiveDeActive(company));
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> ActiveDeActive(CompanyUpdateVM vm)
        //{

        //    try
        //    {
        //        var editDto = _mapper.Map<CompanyUpdateDTO>(vm);
        //        await _companyService.Edit(editDto);
        //        return RedirectToAction("GetAllActiveAndDeactiveCompanies");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = ex.Message;
        //        return RedirectToAction("GetAllActiveAndDeactiveCompanies");
        //    }
        //}

        //Delete
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _companyService.Remove(id);
                return RedirectToAction("GetAllCompanies");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("GetAllCompanies");
            }
        }

        //  Category/Details/

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var companyDto = await _companyService.GetById(id);
                AdminCompanyUpdateVM company = _mapper.Map<AdminCompanyUpdateVM>(companyDto.Data);
                company.MenuList = await GetMenuSelectListAsync();

                return View(company);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Details(AdminCompanyUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.MenuList = await GetMenuSelectListAsync();

                return View(vm);

            }
            
            var menu = await _menuService.GetById(vm.MenuId);
            vm.DealEndDate = vm.DealStartDate.AddMonths(menu.Data.Period);
            var editDto = _mapper.Map<CompanyUpdateDTO>(vm);
            if (vm.LogoFile != null)
            {
              
                string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");

                editDto.Logo = savedFilePath;
            }
            await _companyService.Edit(editDto);
            return RedirectToAction("GetAllCompanies");
        }

        private async Task<SelectList> GetMenuSelectListAsync()
        {
            var menus = await _menuService.AllMenus();
            if (menus.Data != null)
            {
                return new SelectList(menus.Data.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }), "Value", "Text");
            }
            else
            {
                // menus.Data null ise uygun bir SelectList oluşturun veya hata işleme stratejisi uygulayın.
                return new SelectList(Enumerable.Empty<SelectListItem>()); // Varsayılan boş SelectList örneği
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateCompanyStatus(Guid companyId, bool newStatus)
        {

            var companyupdatedto = await _companyService.GetById(companyId);

            //companyupdatedto.Data.IsActive = newStatus;

            await _companyService.EditToggle(companyupdatedto.Data, newStatus);
            return View();
        }


    }
}
