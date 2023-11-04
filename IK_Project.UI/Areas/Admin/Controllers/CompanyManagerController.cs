using AutoMapper;
using Humanizer;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyVM;
using IK_Project.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Web;

namespace IK_Project.UI.Areas.Admin.Controllers
{

    public class CompanyManagerController : AdminBaseController
    {
        private readonly ICompanyManagerService _companyManagerService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICompanyService _companyService;

        public CompanyManagerController(ICompanyManagerService companyManagerService, IMapper mapper, IWebHostEnvironment webHostEnvironment,  ICompanyService companyService)
        {
            _companyManagerService = companyManagerService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _companyService = companyService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllActiveCompanyManagers()
        {
            var listDTO = await _companyManagerService.GetDefaults(x => x.IsActive == true);
            var listVM = _mapper.Map<List<AdminCompanyManagerListVM>>(listDTO.Data);

            return View(listVM);
        }

        //public async Task<IActionResult> GetAllDeletedCompanyManagers()
        //{
        //    var listDTO = await _companyManagerService.GetDefaults(x => x.Status == Domain.Enums.Status.Deleted);
        //    var listVM = _mapper.Map<List<CompanyManagerListVM>>(listDTO);

        //    return View(listVM);
        //}

        public async Task<IActionResult> GetAllCompanyManagers()
        {
            var listDTO = await _companyManagerService.AllManagers();
            var listVM = _mapper.Map<List<AdminCompanyManagerListVM>>(listDTO.Data);

            return View(listVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AdminCompanyManagerCreateVM createVm = new AdminCompanyManagerCreateVM()
            {
                CompanyList = await GetCompanySelectListAsync(),
            };

            return View(createVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCompanyManagerCreateVM vm)
        {

            if (ModelState.IsValid)
            {
               
                try
                {

                    string temporaryPassword = GeneratePassword();
                    vm.Password = temporaryPassword;
                    vm.PasswordChangeRequired = true;
                    if (!string.IsNullOrEmpty(vm.CustomEmail))
                    {
                        vm.Email = vm.CustomEmail;
                    }
                    else
                    {
                        vm.Email = CreateMailAddress(vm.Name, vm.LastName/*, companyUpdateDTO.CompanyName*/);

                    }
                    var dto = _mapper.Map<CompanyManagerCreateDTO>(vm);
                    if (vm.LogoFile != null)
                    {
                        string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                        dto.ProfilePhoto = savedFilePath;

                    }
                    var result = await _companyManagerService.Create(dto);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("GetAllCompanyManagers", "CompanyManager");

                    }
                    else
                    {
                        vm.CompanyList = await GetCompanySelectListAsync();

                        TempData["error"] = "Error occured";
                    }
                }
                catch (Exception ex)
                {

                    vm.CompanyList = await GetCompanySelectListAsync();

                    TempData["error"] = ex.Message;
                }

            }
            vm.CompanyList = await GetCompanySelectListAsync();

            return View(vm);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _companyManagerService.Remove(id);
                return RedirectToAction("GetAllCompanyManagers");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("GetAllCompanyManagers");
            }
        }




        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AdminCompanyManagerEditVM companyManager = _mapper.Map<AdminCompanyManagerEditVM>((await _companyManagerService.GetById(id)).Data);
                companyManager.CompanyList = await GetCompanyInSelectListAsync(id);


                return View(companyManager);

            }


        }

        [HttpPost]
        public async Task<IActionResult> Details(AdminCompanyManagerEditVM vm)
        {
            if (ModelState.IsValid)
            {
            
            var editDto = _mapper.Map<CompanyManagerUpdateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                var result= await _companyManagerService.Edit(editDto);

            if (result.IsSuccess)
            {
                await _companyManagerService.Edit(editDto);
                return RedirectToAction("GetAllCompanyManagers");

            }
            else
            {
                TempData["error"] = "Error occured";
                return View(vm);
            }

            }
            vm.CompanyList = await GetCompanyInSelectListAsync(vm.Id);

            return View(vm);
        }
        public static string GeneratePassword()
        {
            var random = new Random();
            string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string digits = "0123456789";
            string specialCharacters = "!*+.";

            string password = new string(Enumerable.Repeat(
                uppercaseLetters + lowercaseLetters + digits + specialCharacters, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Şifreye en az bir rakam eklemek için:
            int position = random.Next(0, 8);
            char randomDigit = digits[random.Next(digits.Length)];
            password = password.Substring(0, position) + randomDigit + password.Substring(position + 1);

            return password;
        }
        public static string ConvertTurkishCharactersToEnglishLower(string input)
        {
            string output = input;
            output = output.Replace("ç", "c");
            output = output.Replace("ğ", "g");
            output = output.Replace("ı", "i");
            output = output.Replace("ö", "o");
            output = output.Replace("ş", "s");
            output = output.Replace("ü", "u");
            output = output.Replace("Ç", "c");
            output = output.Replace("Ğ", "g");
            output = output.Replace("I", "i");
            output = output.Replace("İ", "i");
            output = output.Replace("Ö", "o");
            output = output.Replace("Ş", "s");
            output = output.Replace("Ü", "u");
            return output;
        }
        public static string CreateMailAddress(string userName, string userLastName/*, string companyName*/)
        {
            string mailAdress = ConvertTurkishCharactersToEnglishLower((userName + "." + userLastName + "@" /*+ companyName*/ + "bilgeadamboost.com").ToLower());
            return mailAdress;
        }
        private async Task<SelectList> GetCompanySelectListAsync()
        {
            var companies = await _companyService.AllCompanies();

            return new SelectList(companies.Data.Where(x=>x.CompanyManagerId==null).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CompanyName,
            }), "Value", "Text");

        }
        private async Task<SelectList> GetCompanyInSelectListAsync(Guid companyManagerId)
        {
            var companies = await _companyService.AllCompanies();

            var selectListItems = companies.Data
                .Where(x => x.CompanyManagerId == companyManagerId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.CompanyName,
                });

            return new SelectList(selectListItems, "Value", "Text");
        }



    }
}
