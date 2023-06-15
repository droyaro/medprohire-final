using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MedProHireAPI.Models;
using MedProHireAPI.Models.Applicant;
using MedProHireAPI.Models.ClinicalInstitution;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedProHireAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]/[Action]")]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly RootPath _rootPath;
        private string user_ID;
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public RegisterController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices, IOptions<RootPath> rootPath
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _rootPath = rootPath.Value;


        }
      
        [HttpPost]
        public async Task<IActionResult> ClincicalRegsiterFirstStep([FromHeader]string ApiKey, [FromBody]ClinicalRegisterFirstStepModel model)
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
                    try
                {
                    string phonenumber = model.Phone;
                    phonenumber= phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
                    PhoneVerify phoneVerify = _commonService.GetVerificationKey("+1"+phonenumber);

                    bool answer = phoneVerify != null ? phoneVerify.IsVerified ? true : false : false;
                    if (answer)
                    {
                        string Name = "";

                        Name = model.InstitutionName.Replace(";", ",") + ";" + model.ContactPerson.Replace(";", ",") + ";" + model.ContactTitle.Replace(";", ",");
                        ApplicationUser user = new ApplicationUser
                        {
                            Email = model.EmailAddress,
                            UserName = model.UserName,
                            Address = model.Address,
                            City_ID = model.City_ID,
                            PhoneNumber = "+1 "+phonenumber,
                            ZipCode = model.ZipCode,
                            isVerified = true,
                            Name = Name
                        };
                        // Adding role

                        string role = clrole;


                        if (model.Latitude == 0 || model.Longitude == 0)
                        {
                            float latitude = 0;
                            float longitude = 0;
                            string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                            if (!String.IsNullOrEmpty(model.Address))
                            {
                                latlong latlong = _commonService.GetLatLongByAddress(query);
                                latitude = latlong.Latitude;
                                longitude = latlong.Longitude;
                            }
                            user.Longitude = longitude;
                            user.Latitude = latitude;
                        }
                        else
                        {
                            user.Longitude = model.Longitude;
                            user.Latitude = model.Latitude;

                        }
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {


                            await _userManager.AddToRoleAsync(user, role);
                     
                            _commonService.RemoveVerificationKey("+1"+phonenumber);
                            // passing user_ID and role as session, for filling other registration forms    
                          
                            return Ok(user.Id);

                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Phone Number is not verified");
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Please try again");
                }
            }
                else
                {
                    ModelState.AddModelError("", "Api Key is not valid");
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> ApplicantRegsiterFirstStep([FromHeader]string ApiKey, [FromBody]ApplicantRegisterFirstStepModel model)
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
                    try
                    {
                        string phonenumber = model.Phone;
                        phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();

                        PhoneVerify phoneVerify = _commonService.GetVerificationKey("+1" + phonenumber);

                        bool answer = phoneVerify != null ? phoneVerify.IsVerified ? true : false : false;
                        if (answer)
                        {
                            string Name = "";

                            Name = model.FirstName.Replace(";", ",") + ";" + model.LastName.Replace(";", ",") + ";" + model.MiddleName?.Replace(";", ",");
                            ApplicationUser user = new ApplicationUser
                            {
                                Email = model.EmailAddress,
                                UserName = model.UserName,
                                Address = model.Address,
                                City_ID = model.City_ID,
                                PhoneNumber = "+1 " + phonenumber,
                                ZipCode = model.ZipCode,
                                isVerified = true,
                                Name = Name
                            };
                            // Adding role

                            string role = approle;


                            if (model.Latitude == 0 || model.Longitude == 0)
                            {
                                float latitude = 0;
                                float longitude = 0;
                                string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                                if (!String.IsNullOrEmpty(model.Address))
                                {
                                    latlong latlong = _commonService.GetLatLongByAddress(query);
                                    latitude = latlong.Latitude;
                                    longitude = latlong.Longitude;
                                }
                                user.Longitude = longitude;
                                user.Latitude = latitude;
                            }
                            else
                            {
                                user.Longitude = model.Longitude;
                                user.Latitude = model.Latitude;

                            }
                            var result = await _userManager.CreateAsync(user, model.Password);

                            if (result.Succeeded)
                            {


                                await _userManager.AddToRoleAsync(user, role);

                                _commonService.RemoveVerificationKey("+1"+phonenumber);
                                // passing user_ID and role as session, for filling other registration forms    

                                return Ok(user.Id);

                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            }

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Phone Number is not verified");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "API is not valid");
                }
            }
                return BadRequest(ModelState);
        }
      
        [HttpPost]
        public async Task<IActionResult> AppliantRegisterNextStep([FromHeader]string ApiKey, [FromBody]ApplicantRegistrationNextModel model)
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
                    string profilefilename = "";
                    string resumefilename = "";
                    List<ApplicantCertificates> applicantCertificates = new List<ApplicantCertificates>();
                    try
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(model.User_ID.ToString());

                        if (user != null && user.isVerified)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            Applicants app = _commonService.FindApplicantByUserID(user.Id);
                            if (roles.Contains(approle) && app == null)
                            {
                                string firstname = user.Name;
                                string lastname = user.Name;
                                string middlename = user.Name;
                                string[] splitName = user.Name.Split(";");
                                if (splitName.Count() == 3)
                                {
                                    firstname = splitName[0];
                                    lastname = splitName[1];
                                    middlename = splitName[2];
                                }
                                if (splitName.Count() == 2)
                                {
                                    firstname = splitName[0];
                                    lastname = splitName[1];
                                    middlename = "";
                                }
                                // uploading forms files 
                                if (model.certificates.Count > 0)
                                {
                                    string subpath = "Certificates" + $@"\{user_ID}";

                                    foreach (ApplicantCertificateModel certificate in model.certificates)
                                    {
                                        if (certificate.CeritifcationImg != null && certificate.CertificateType != null)
                                        {
                                            string filename = Savefile(certificate.CeritifcationImg, subpath, certificate.CertificateType.ToString() + "_" + model.certificates.IndexOf(certificate).ToString());
                                            if (filename != "")
                                            {

                                                applicantCertificates.Add(new ApplicantCertificates() { CertificateTypes = certificate.CertificateType, CeritificationImg = filename });
                                            }
                                            else
                                            {
                                                ModelState.AddModelError(string.Empty, "Can't upload Certificates");
                                            }
                                        }

                                    }
                                }
                                if (model.ProfileImage != null)
                                {
                                    string path = "ApplicantImg";
                                    profilefilename = Savefile(model.ProfileImage, path, user_ID);
                                    if (profilefilename == "")
                                    {
                                        ModelState.AddModelError(string.Empty, "Can't upload Profile Picture");
                                    }

                                }
                                if (model.Resume != null)
                                {
                                    string path = "Resume";
                                    resumefilename = Savefile(model.Resume, path, user_ID);
                                    if (resumefilename == "")
                                    {
                                        ModelState.AddModelError(string.Empty, "Can't upload Resume");
                                    }

                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Resume is required");
                                    return View(model);
                                }
                                Applicants applicant = new Applicants()
                                {
                                    User_ID = Guid.Parse(user_ID),
                                    Availability_ID = model.Availability_ID,
                                    FirstName = firstname,
                                    LastName = lastname,
                                    MiddleName = middlename,
                                    VisaStatus_ID = model.VisaStatus_ID,
                                    //WorkAuth = model.WorkAuth,
                                    //CEU = model.CEU,
                                    ProfileImage = profilefilename,
                                    IsEligible = model.IsEligible,
                                    Resume = resumefilename

                                };

                                List<ApplicantCertificates> appcertificates = applicantCertificates;
                                List<ApplicantSpecialities> appspecialities = new List<ApplicantSpecialities>();
                                List<ApplicantWorkHistories> appworkHistories = new List<ApplicantWorkHistories>();

                                if (model.workHistories.Count > 0)
                                {
                                    foreach (var workhistory in model.workHistories)
                                    {

                                        if (workhistory.StartDate != null && workhistory.PlaceName != null && workhistory.Speciality_ID != null && (workhistory.UntilNow ? workhistory.UntilNow : (workhistory.EndDate != null)))
                                        {
                                            appworkHistories.Add(new ApplicantWorkHistories()
                                            {
                                                EndDate = workhistory.EndDate.GetValueOrDefault(),
                                                StartDate = workhistory.StartDate.Value,
                                                PlaceName = workhistory.PlaceName,
                                                JobTitle = workhistory.JobTitle
                                            });
                                        }

                                    }
                                }

                                if (model.specialities.Count > 0)
                                {
                                    foreach (var speciality in model.specialities)
                                    {
                                        if (speciality != null)
                                        {
                                            appspecialities.Add(new ApplicantSpecialities()
                                            {
                                                Speciality_ID = speciality.Speciality_ID,
                                                License = speciality.License,
                                                LegabilityStates = String.Join("; ", speciality.LegabilityStates)
                                            });
                                        }
                                    }
                                }
                                bool addchanges = _commonService.SaveApplicant(applicant, appcertificates, appspecialities, appworkHistories.Count == 0 ? null : appworkHistories);
                                if (addchanges)
                                {
                                    await _signInManager.SignInAsync(user, false);
                                    return Ok();
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "User is not Clinical Institution or Already Exist");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Registration failed. PhoneNumber is not verified!");

                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "API is not valid");
                }
            }

                return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> ClinicaltRegisterNextStep([FromHeader]string ApiKey,[FromBody]ClinicalRegisterNextStepModel model)
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
                    string logo = "";
                    try
                    {
                        ApplicationUser user = await _userManager.FindByIdAsync(model.User_ID.ToString());

                    if (user != null && user.isVerified)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                       ClinicalInstitutions clinical= _commonService.FindClinicaByUserID(user.Id);
                        if (roles.Contains(clrole) && clinical==null)
                        {
                            string name = user.Name;
                            string contactperson = user.Name;
                            string contacttitle = user.Name;
                            string[] splitName = user.Name.Split(";");
                            if (splitName.Count() == 3)
                            {
                                name = splitName[0];
                                contactperson = splitName[1];
                                contacttitle = splitName[2];
                            }
                            if (model.Logo != null)
                            {
                                logo = Savefile(model.Logo, "ClinicalLogo", model.User_ID.ToString());
                                if (logo == "")
                                {
                                    ModelState.AddModelError(string.Empty, "Can't upload logo, please try again.");
                                }

                            }
                            ClinicalInstitutions clinicalInstitution = new ClinicalInstitutions()
                            {
                                User_ID = model.User_ID,
                                InstitutionType_ID = model.InstitutionType_ID,
                                InstitutionName = name,
                                ContactPerson = contactperson,
                                ContactTitle = contacttitle,
                                InstitutionTaxId = model.InstitutionTaxId,
                                InstitutionDescription = model.InstitutionDescription,
                                Logo = logo,
                            };
                            //if (_commonService.SaveClinicalIntitution(clinicalInstitution))
                            //{
                            //    await _signInManager.SignInAsync(user, false);
                                
                            //    return Ok();

                            //}
                            //else
                            //{
                            //    ModelState.AddModelError(string.Empty, "Registration failed. Please try again");

                            //}
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User is not Clinical Institution or Already Exist");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Registration failed. PhoneNumber is not verified!");

                    }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "API is not valid");
                }
            }

            return BadRequest(ModelState);
        }
        [HttpPost]
        public JsonResult SendPhoneVerification([FromHeader]string ApiKey, [FromBody]string PhoneNumber)
        {
            bool answer = false;
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                 answer = _commonService.SendPhoneVerificationCode(PhoneNumber);
            } 
            else
            {
                return Json(null);
            }
            return Json(answer);
        }

        [HttpPost]
        public JsonResult VerifyPhoneNumber([FromHeader]string ApiKey, [FromBody]string VerificationCode, [FromBody]string PhoneNumber)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                PhoneNumber = PhoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
                PhoneNumber = "+1" + PhoneNumber;
                PhoneVerify phoneVerify = _commonService.GetVerificationKey(PhoneNumber);

                if (phoneVerify != null)
                {
                    if (phoneVerify.VerificationCode == VerificationCode && VerificationCode != null && VerificationCode != "")
                    {
                        phoneVerify.IsVerified = true;

                        if (_commonService.UpdateVerifiactionKey(phoneVerify))
                        {
                            return Json(true);
                        }
                    }
                }
            }
            else Json(null);
            return Json(false);
        }
        private string Savefile( IFormFile file, string path, string filename)
        {
            string foldername = "Upload";
            if (!Directory.Exists(Path.Combine(_rootPath.UserRoot, foldername, path)))
            {
                Directory.CreateDirectory(Path.Combine(_rootPath.UserRoot, foldername, path));
            }
            string extention = file.FileName.Substring(file.FileName.LastIndexOf('.'), file.FileName.Length - file.FileName.LastIndexOf('.'));
            filename += extention;
            using (FileStream fs = new FileStream(Path.Combine(_rootPath.UserRoot, foldername, path, filename), FileMode.Create))
            {
                try
                {
                    file.CopyTo(fs);
                    fs.Flush();
                    return foldername + "\\" + path + "\\" + filename;
                }
                catch
                { return ""; }
            }
        }
    }
}