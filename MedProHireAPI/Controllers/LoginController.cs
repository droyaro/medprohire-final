using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedProHireAPI.Models;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MedProHireAPI.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
       
        private string user_ID;
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public LoginController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            
           

        }
  
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginViewModel model, [FromHeader]string ApiKey)
        {
            if (ModelState.IsValid)
            {
                Guid Api = Guid.Empty;
                if (!String.IsNullOrEmpty(ApiKey))
                {
                    Guid.TryParse(ApiKey, out Api);
                }
                var apiAnswer = _commonService.CheckFullApiKey(Api);
                if (apiAnswer)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        user = await _userManager.FindByEmailAsync(model.UserName);

                    }
                    if (user != null)

                    {

                        int timeoffset = model.TimeOffset;
                        user.TimeOffset = timeoffset;
                        await _userManager.UpdateAsync(user);
                        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            var userRoles = await _userManager.GetRolesAsync(user);
                            // if user is applicant, checking if other registration forms are filled
                            if (userRoles.Any(x => x == approle))
                            {

                                Applicants applicant = _commonService.FindApplicantByUserID(user.Id);
                                if (applicant == null)
                                {
                                    return Ok(new string[] { approle, "Not Registered" });
                                }
                                else
                                {
                                    await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
                                   // await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserId", user.Id.ToString()));
                                    return Ok(new string[] { approle, applicant.BoardingProcess.ToString() });

                                }
                            }
                            else
                            // if user is clinicalInstitution, checking if registration form is filled
                            if (userRoles.Any(x => x == clrole))
                            {
                                ClinicalInstitutions clinical = _commonService.FindClinicaByUserID(user.Id);
                                if (clinical == null)
                                {

                                    return Ok(new string[] { clrole, "Not Registered" });
                                }

                            }
                            else { return RedirectToAction("Register", "Home"); }
                            await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserId", user.Id.ToString()));
                            return Ok(clrole);
                        }
                        // if login failed
                        else
                        {
                            ModelState.AddModelError("Password", "Password is not valid");
                            return BadRequest(ModelState);
                        }

                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "API is not valid");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
