using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using medprohiremvp.Models.Home;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CustomIdentityApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
         public IActionResult Register()
        {
            
         List<Countries> countries=  _commonService.GetCountries();
            ViewBag.Countries = countries;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.EmailAddress, UserName = model.UserName };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}