using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MedProHireAPI.Models.ClinicalInstitution;
using MedProHireAPI.Models.Applicant;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedProHireAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Client/[Action]")]
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonServices _commonService;
        private readonly RootPath _rootPath;
        private readonly IHostingEnvironment _environment;
        private string user_ID;
        // role names
        private string approle = "Applicant";
        private string clrole = "ClinicalInstitution";


        public ClientController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICommonServices commonServices, IOptions<RootPath> rootPath, IHostingEnvironment environment
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _commonService = commonServices;
            _rootPath = rootPath.Value;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult ClientDashboard([FromHeader]string ApiKey, int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClientDashboardModel model = new ClientDashboardModel();

                ClinicalInstitutions clinicalInstitution = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinicalInstitution != null)
                {

                    List<ClientShifts> shifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID);

                    model.Yearshifts = new ShiftsCountModelForDashboard();
                    model.Yearshifts.Created = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Yearshifts.NotStarted = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 1).Count();
                    model.Yearshifts.Active = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 2).Count();
                    model.Yearshifts.Completed = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 4).Count();
                    model.Yearshifts.Incomplete = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 5).Count();
                    model.Yearshifts.Cancelled = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year && x.Category_ID == 4).Count();
                    model.Allshifts = new ShiftsCountModelForDashboard();
                    model.Allshifts.Created = shifts.Count();
                    model.Allshifts.NotStarted = shifts.Where(x => x.Category_ID == 1).Count();
                    model.Allshifts.Active = shifts.Where(x => x.Category_ID == 2).Count();
                    model.Allshifts.Completed = shifts.Where(x => x.Category_ID == 4).Count();
                    model.Allshifts.Incomplete = shifts.Where(x => x.Category_ID == 5).Count();
                    model.Allshifts.Cancelled = shifts.Where(x => x.Category_ID == 4).Count();
                    model.Q1shifts = new ShiftsCountModelForDashboard();
                    model.Q1shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q1shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts = new ShiftsCountModelForDashboard();
                    model.Q2shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q2shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts = new ShiftsCountModelForDashboard();
                    model.Q3shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q3shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts = new ShiftsCountModelForDashboard();
                    model.Q4shifts.Created = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.NotStarted = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 1 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Active = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Completed = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Incomplete = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 5 && x.DateOfShift.Year == DateTime.Now.Year).Count();
                    model.Q4shifts.Cancelled = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count();

                    List<int> locations_id = shifts.GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.AllLocation = new List<LocationCountModelForDashboard>();
                    foreach (int id in locations_id)
                    {
                        if (id == 0)
                        {

                            model.AllLocation.Add(new LocationCountModelForDashboard
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.AllLocation.Add(new LocationCountModelForDashboard
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4).Count(),
                                Location_ID = id

                            });
                        }

                    }
                    List<int> years_lid = shifts.Where(x => x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.YearLocation = new List<LocationCountModelForDashboard>();
                    foreach (int id in years_lid)
                    {
                        if (id == 0)
                        {

                            model.YearLocation.Add(new LocationCountModelForDashboard
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.YearLocation.Add(new LocationCountModelForDashboard
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }

                    }
                    List<int> Q1_lid = shifts.Where(x => x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q1Location = new List<LocationCountModelForDashboard>();
                    foreach (int id in Q1_lid)
                    {
                        if (id == 0)
                        {

                            model.Q1Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q1Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 1 && x.DateOfShift.Month <= 3 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }

                    }
                    List<int> Q2_lid = shifts.Where(x => x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q2Location = new List<LocationCountModelForDashboard>();
                    foreach (int id in Q2_lid)
                    {
                        if (id == 0)
                        {

                            model.Q2Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q2Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 4 && x.DateOfShift.Month <= 6 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                    }
                    List<int> Q3_lid = shifts.Where(x => x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q3Location = new List<LocationCountModelForDashboard>();
                    foreach (int id in Q3_lid)
                    {
                        if (id == 0)
                        {

                            model.Q3Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q3Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 7 && x.DateOfShift.Month <= 9 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                    }
                    List<int> Q4_lid = shifts.Where(x => x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).GroupBy(x => x.Branch_ID.GetValueOrDefault()).Select(x => x.Key).ToList();
                    model.Q4Location = new List<LocationCountModelForDashboard>();
                    foreach (int id in Q4_lid)
                    {
                        if (id == 0)
                        {

                            model.Q4Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = clinicalInstitution.InstitutionName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 2 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == 0 && x.Category_ID == 4 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }
                        else
                        {
                            model.Q4Location.Add(new LocationCountModelForDashboard
                            {
                                LocationName = _commonService.GetLocations(clinicalInstitution.Institution_ID).Where(x => x.Branch_ID == id).FirstOrDefault().BranchName,
                                CreatedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                ActiveShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 2 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                CompletedShiftsCount = shifts.Where(x => x.Branch_ID.GetValueOrDefault() == id && x.Category_ID == 4 && x.DateOfShift.Month >= 10 && x.DateOfShift.Month <= 12 && x.DateOfShift.Year == DateTime.Now.Year).Count(),
                                Location_ID = id

                            });
                        }


                    }
                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Institution ID is not valid");

                }

            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);

        }
        [HttpGet]
        public async Task<IActionResult> ClientProfile([FromHeader]string ApiKey, int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinical != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        if ((user.ChangesMakedTime - DateTime.Now).Days != 0 || ((user.ChangesMakedTime - DateTime.Now).Days == 0 && (user.ChangesMakedTime - DateTime.Now).Hours != 0))
                        {
                            user.ChangesCount = 0;
                            user.ChangesLocked = false;
                            await _userManager.UpdateAsync(user);
                        }

                        if (clinical != null)
                        {
                            ClientProfileModel model = new ClientProfileModel();
                            model.Institution_ID = clinical.Institution_ID;
                            model.User_ID = clinical.User_ID;
                            model.Profile = new ClientProfileDetailModel()
                            {
                                ContactPerson = clinical.ContactPerson,
                                ContactTitle = clinical.ContactTitle,
                                InstitutionName = clinical.InstitutionName,
                                InstitutionTaxId = clinical.InstitutionTaxId,
                                InstitutionType_ID = clinical.InstitutionType_ID,
                                City_ID = user.City_ID,
                                Address = user.Address,
                                ZipCode = user.ZipCode,
                                InstitutionDescription = clinical.InstitutionDescription,

                                Email = user.Email,
                                Disabled = user.ChangesLocked,
                                Longitude = user.Longitude,
                                Latitude = user.Latitude,


                            };
                            var city = _commonService.GetCitiesByCityid(user.City_ID);
                            model.Profile.State_ID = city != null ? city.state_id : 0;

                            model.LogoSrc = "/" + clinical.Logo.Replace('\\', '/');
                            model.PhoneNumber = user.PhoneNumber;
                            return Ok(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User does not exist");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Institution ID is not valid");
                }
            }

            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> SaveClientProfileChanges([FromHeader]string ApiKey, [FromBody]ClientProfileDetailModel model, [FromBody] int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if (ModelState.IsValid)
                {
                    ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                    if (clinical != null)
                    {

                        ApplicationUser user = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                        if (user != null)
                        {


                            if (user.City_ID != model.City_ID || user.Address != model.Address)
                            {
                                if (model.Latitude != user.Latitude || model.Longitude != model.Longitude)
                                {
                                    if (!String.IsNullOrEmpty(model.Address))
                                    {
                                        string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                                        latlong latlong = _commonService.GetLatLongByAddress(query);
                                        if (latlong.Latitude != 0 && latlong.Longitude != 0)
                                        {
                                            user.Latitude = latlong.Latitude;
                                            user.Longitude = latlong.Longitude;
                                        }
                                    }
                                }
                                else
                                {
                                    user.Longitude = model.Longitude;
                                    user.Latitude = model.Latitude;
                                    HttpContext.Session.Remove("counter");
                                }
                            }
                            clinical.ContactPerson = model.ContactPerson;
                            clinical.ContactTitle = model.ContactTitle;
                            clinical.InstitutionType_ID = model.InstitutionType_ID;
                            clinical.InstitutionDescription = model.InstitutionDescription;
                            clinical.InstitutionName = model.InstitutionName;
                            user.City_ID = model.City_ID;
                            user.Address = model.Address;
                            user.ZipCode = model.ZipCode;


                            if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                            {
                                if (user.ChangesCount >= 4)
                                {
                                    user.ChangesLocked = true;
                                }
                                user.ChangesMakedTime = DateTime.Now;
                                user.ChangesCount = user.ChangesCount + 1;


                            }
                            else
                            {
                                user.ChangesMakedTime = DateTime.Now;
                                user.ChangesCount = 1;
                                user.ChangesLocked = false;
                            }

                            _commonService.UpdateClinical(clinical);
                            var answer = await _userManager.UpdateAsync(user);
                            if (answer.Succeeded)
                            {
                                return Ok();
                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, string.Join("; ", answer.Errors.Select(x => x.Description).ToList()));
                            }


                        }
                        else
                        {
                            ModelState.AddModelError("", "ApplicationUser is null");

                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Institution ID is not valid");

                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProfileImage([FromHeader]string ApiKey, [FromBody]IFormFile Logo, [FromBody] int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinical != null)
                {

                    ApplicationUser user = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        if (Logo != null)
                        {
                            string path = "ClinicalLogo";
                            string profilefilename = SaveLogoFile(Logo, path, user.Id.ToString());
                            if (profilefilename != "")
                            {
                                clinical.Logo = profilefilename;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Can't save Logo file");

                            }
                            if ((DateTime.Now - user.ChangesMakedTime).Days == 0 && (DateTime.Now - user.ChangesMakedTime).Hours == 0)
                            {
                                if (user.ChangesCount >= 9)
                                {
                                    user.ChangesLocked = true;
                                }
                                user.ChangesMakedTime = DateTime.Now;
                                user.ChangesCount = user.ChangesCount + 1;


                            }
                            else
                            {
                                user.ChangesMakedTime = DateTime.Now;
                                user.ChangesCount = 1;
                                user.ChangesLocked = false;
                            }

                            _commonService.UpdateClinical(clinical);
                            var answer = await _userManager.UpdateAsync(user);
                            if (answer.Succeeded)
                            {
                                return Ok();
                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, string.Join("; ", answer.Errors.Select(x => x.Description).ToList()));
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Logo is null");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "ApplicationUser is null");

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Institution ID is not valid");

                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public IActionResult GetClientShiftList([FromHeader]string ApiKey, int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinical != null)
                {
                    List<ClientShifts> shifts = _commonService.GetClientShifts(clinical.Institution_ID);
                    List<ClientShiftModel> model = new List<ClientShiftModel>();

                    foreach (ClientShifts clientshift in shifts)
                    {
                        model.Add(new ClientShiftModel()
                        {
                            ClientShift_ID = clientshift.ClientShift_ID,
                            ClockInTime = clientshift.ClockInTime,
                            ClockOutTime = clientshift.ClockOutTime,
                            HourlyRate = clientshift.HourlyRate,
                            ContractorCount = clientshift.ContractorCount,
                            StartDate = clientshift.StartDate,
                            EndDate = clientshift.EndDate,
                            Responsibility = clientshift.Responsibility,
                            ShiftDescription = clientshift.ShiftDescription,
                            DateOfShift = clientshift.DateOfShift,
                            ShiftExpirationDate = clientshift.ShiftExpirationDate,
                            Institution_ID = clientshift.Institution_ID,
                            Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID),
                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                .Select(x => x.SpecialityName).ToList()),
                            Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                            BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinical.InstitutionName,
                            Category_ID = clientshift.Category_ID,
                            ShiftsDates = clientshift.ShiftsDates,
                            Category_Name = _commonService.GetShiftCategories().Where(x => x.Category_ID == clientshift.Category_ID).FirstOrDefault().CategoryName,

                            Occurrences = clientshift.Occurrences,
                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).FirstOrDefault().ShiftLabelName,
                            ShiftLabel_ID = clientshift.ShiftLabel_ID

                        });
                    }
                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Institution ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult CreateNewShift([FromHeader]string ApiKey, [FromBody] NewClientShiftModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if (ModelState.IsValid)
                {
                    ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(model.Institution_ID);
                    if (clinical != null)
                    {
                        if (model.Specialities.Count > 0)
                        {
                            List<ShiftSpecialities> shiftSpecialities = new List<ShiftSpecialities>();
                            foreach (var shiftspec in model.Specialities)
                            {
                                shiftSpecialities.Add(new ShiftSpecialities() { Speciality_ID = shiftspec });
                            }
                            if (model.ShiftsDates != "")
                            {
                                var dates = model.ShiftsDates.Split(',').ToList();
                                dates = dates.OrderBy(x => DateTime.Parse(x)).ToList();
                                ClientShifts clientShift = new ClientShifts()
                                {
                                    ClockInTime = model.ClockInTime,
                                    ClockOutTime = model.ClockOutTime,
                                    HourlyRate = model.HourlyRate,
                                    ContractorCount = model.ContractorCount,
                                    StartDate = DateTime.Parse(dates[0]),
                                    EndDate = DateTime.Parse(dates[dates.Count - 1]),
                                    Responsibility = model.Responsibility,
                                    ShiftDescription = model.ShiftDescription,
                                    DateOfShift = DateTime.Now.Date,
                                    ShiftExpirationDate = model.ShiftExpirationDate,
                                    Institution_ID = clinical.Institution_ID,
                                    institution = clinical,
                                    Occurrences = dates.Count,
                                    ShiftLabel_ID = model.ShiftLabel_ID,
                                    ShiftsDates = String.Join(',', dates),
                                    Category_ID = 1,


                                };
                                if (model.Branch_ID != 0)
                                {
                                    clientShift.Branch_ID = model.Branch_ID;
                                }
                                try
                                {
                                    bool answer = _commonService.AddClientShift(clientShift, shiftSpecialities);
                                    if (answer)
                                    {
                                        return Ok();
                                    }
                                    else
                                    {
                                        ModelState.AddModelError(string.Empty, "Can't Save Client Shift. Please try again!");
                                    }

                                }
                                catch
                                {
                                    ModelState.AddModelError(string.Empty, "Please try again!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Shift Dates can not be empty!");
                            }
                        }

                        else
                        {
                            ModelState.AddModelError(string.Empty, "Specialities can not be empty!");
                        }



                        return Ok(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Institution ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetShiftDetail([FromHeader]string ApiKey, int ClientShift_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                if (clientshift != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.GetClinicalInstitution_byID(clientshift.Institution_ID);
                    ClientShiftModel model = new ClientShiftModel()
                    {
                        ClientShift_ID = clientshift.ClientShift_ID,
                        ClockInTime = clientshift.ClockInTime,
                        ClockOutTime = clientshift.ClockOutTime,
                        HourlyRate = clientshift.HourlyRate,
                        ContractorCount = clientshift.ContractorCount,
                        StartDate = clientshift.StartDate.Date,
                        EndDate = clientshift.EndDate.Date,
                        Responsibility = clientshift.Responsibility,
                        ShiftDescription = clientshift.ShiftDescription,
                        DateOfShift = clientshift.DateOfShift.Date,
                        ShiftExpirationDate = clientshift.ShiftExpirationDate.Date,
                        Institution_ID = clientshift.Institution_ID,
                        Specialities = _commonService.GetShiftSpecialities(clientshift.ClientShift_ID),
                        SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().
                                                                                                Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                                .Select(x => x.SpecialityName).ToList()),
                        Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                        BranchName = clientshift.Branch_ID != null ? _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName : clinicalInstitution.InstitutionName,
                        ShiftLabel_ID = clientshift.ShiftLabel_ID,
                        ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).Select(X => X.ShiftLabelName).FirstOrDefault(),
                        Category_Name = _commonService.GetShiftCategories().Where(x => x.Category_ID == clientshift.Category_ID).FirstOrDefault().CategoryName,
                        Category_ID = clientshift.Category_ID,
                        Occurrences = clientshift.Occurrences,

                        ShiftsDates = clientshift.ShiftsDates,


                    };

                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Client Shift ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult EditShift([FromHeader]string ApiKey, [FromBody] NewClientShiftModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if (ModelState.IsValid)
                {
                    ClientShifts shift = _commonService.GetClientShiftByID(model.ClientShift_ID);
                    ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(model.Institution_ID);
                    if (shift != null && clinical != null)
                    {
                        if (model.Specialities.Count > 0)
                        {
                            List<ShiftSpecialities> shiftSpecialities = new List<ShiftSpecialities>();
                            foreach (var shiftspec in model.Specialities)
                            {
                                shiftSpecialities.Add(new ShiftSpecialities() { Speciality_ID = shiftspec });
                            }
                            if (model.ShiftsDates != "")
                            {
                                var dates = model.ShiftsDates.Split(',').ToList();
                                dates = dates.OrderBy(x => DateTime.Parse(x)).ToList();
                                ClientShifts clientShift = new ClientShifts()
                                {
                                    ClockInTime = model.ClockInTime,
                                    ClockOutTime = model.ClockOutTime,
                                    HourlyRate = model.HourlyRate,
                                    ContractorCount = model.ContractorCount,
                                    StartDate = DateTime.Parse(dates[0]),
                                    EndDate = DateTime.Parse(dates[dates.Count - 1]),
                                    Responsibility = model.Responsibility,
                                    ShiftDescription = model.ShiftDescription,
                                    DateOfShift = DateTime.Now.Date,
                                    ShiftExpirationDate = model.ShiftExpirationDate,
                                    ClientShift_ID = model.ClientShift_ID,
                                    Institution_ID = clinical.Institution_ID,

                                    Occurrences = dates.Count,
                                    ShiftLabel_ID = model.ShiftLabel_ID,
                                    ShiftsDates = String.Join(',', dates),
                                    Category_ID = 1,


                                };
                                if (model.Branch_ID != 0)
                                {
                                    clientShift.Branch_ID = model.Branch_ID;
                                }
                                try
                                {
                                    bool answer = _commonService.UpdateShift(clientShift, shiftSpecialities);
                                    if (answer)
                                    {
                                        return Ok();
                                    }
                                    else
                                    {
                                        ModelState.AddModelError(string.Empty, "Can't Update Client Shift. Please try again!");
                                    }

                                }
                                catch
                                {
                                    ModelState.AddModelError(string.Empty, "Please try again!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Shift Dates can not be empty!");
                            }
                        }

                        else
                        {
                            ModelState.AddModelError(string.Empty, "Specialities can not be empty!");
                        }



                        return Ok(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Clinical Institution or Shift is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult RemoveShift([FromHeader]string ApiKey, int ClientShift_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                if (clientshift != null)
                {
                    if (clientshift.Category_ID == 1)
                    {
                        var answer = _commonService.RemoveShift(ClientShift_ID);
                        if (answer)
                        {

                            return Ok();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't Remove Shift");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Client Shift Category is Not Started");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Client Shift ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetLocationList([FromHeader]string ApiKey, int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinical != null)
                {
                    List<LocationModel> locations = new List<LocationModel>();
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                    if (user != null)
                    {
                        locations.Add(new LocationModel()
                        {
                            Institution_ID = clinical.Institution_ID,
                            City_ID = user.City_ID,
                            ContactName = clinical.ContactPerson,
                            Name = clinical.InstitutionName,
                            Address = user.Address,
                            PhoneNumber = user.PhoneNumber,
                            ZipCode = user.ZipCode,
                            CityName = _commonService.GetCityName(user.City_ID),
                            Email = user.Email,
                            Branch_ID = 0,
                            Latitude = user.Latitude,
                            Longitude = user.Longitude,
                            State_ID = _commonService.GetCitiesByCityid(user.City_ID).state_id

                        });
                    }
                    List<ClinicalInstitutionBranches> institutionBranches = _commonService.GetLocations(Institution_ID);
                    if (institutionBranches != null)
                    {
                        foreach (ClinicalInstitutionBranches branch in institutionBranches)
                        {
                            locations.Add(new LocationModel()
                            {
                                Institution_ID = branch.Institution_ID,
                                City_ID = branch.CityId,
                                ContactName = branch.ContactName,
                                Branch_ID = branch.Branch_ID,
                                Name = branch.BranchName,
                                Address = branch.Address,
                                PhoneNumber = branch.PhoneNumber,
                                ZipCode = branch.ZipCode,
                                CityName = _commonService.GetCityName(branch.CityId),
                                Email = branch.Email,
                                Latitude = branch.Latitude,
                                Longitude = branch.Longitude,
                                State_ID = _commonService.GetCitiesByCityid(branch.CityId).state_id
                            });
                        }
                    }
                    return Ok(locations);
                }
                else
                {
                    ModelState.AddModelError("", "Institution ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetLocationDetail([FromHeader] string ApiKey, int Location_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {

                ClinicalInstitutionBranches location = _commonService.GetlocationbyId(Location_ID);
                if (location != null)
                {
                    LocationModel model = new LocationModel()
                    {
                        Institution_ID = location.Institution_ID,
                        City_ID = location.CityId,
                        ContactName = location.ContactName,
                        Branch_ID = location.Branch_ID,
                        Name = location.BranchName,
                        Address = location.Address,
                        PhoneNumber = location.PhoneNumber,
                        ZipCode = location.ZipCode,
                        CityName = _commonService.GetCityName(location.CityId),
                        Email = location.Email,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        State_ID = _commonService.GetCitiesByCityid(location.CityId).state_id
                    };
                    return Ok(model);

                }
                else
                {
                    ModelState.AddModelError("", "Location ID is not valid");
                }

            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult RemoveLocation([FromHeader] string ApiKey, int Location_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {

                ClinicalInstitutionBranches location = _commonService.GetlocationbyId(Location_ID);
                if (location != null)
                {
                    var answer = _commonService.RemoveLocation(Location_ID);
                    if (answer)
                    {
                        return Ok("Location removed successfully");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't remove Location");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Location ID is not valid");
                }

            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);

        }
        [HttpPost]
        public IActionResult NewLocation([FromHeader] string ApiKey, [FromBody] NewLocationModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if (ModelState.IsValid)
                {
                    ClinicalInstitutionBranches branch = new ClinicalInstitutionBranches()
                    {
                        Address = model.Address,
                        BranchName = model.Name,
                        CityId = model.City_ID,
                        ContactName = model.ContactName,
                        Email = model.Email,
                        Institution_ID = model.Institution_ID,
                        PhoneNumber = model.PhoneNumber,
                        ZipCode = model.ZipCode,
                    };
                    if (model.Latitude == 0 || model.Longitude == 0)
                    {
                        string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                        if (!String.IsNullOrEmpty(model.Address))
                        {
                            float latitude = 0;
                            float longitude = 0;
                            latlong latlong = _commonService.GetLatLongByAddress(query);
                            if (latlong.Latitude != 0 && latlong.Longitude != 0)
                            {
                                latitude = latlong.Latitude;
                                longitude = latlong.Longitude;
                            }
                            branch.Longitude = longitude;
                            branch.Latitude = latitude;
                        }
                    }
                    else
                    {
                        branch.Latitude = model.Latitude;
                        branch.Longitude = model.Longitude;
                    }
                    var answer = _commonService.AddLocation(branch);
                    if (answer)
                    {
                        return Ok("Location Added successfully");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Can't add Location");
                    }
                }


            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult EditLocation([FromHeader] string ApiKey, [FromBody] NewLocationModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                if (ModelState.IsValid)
                {
                    ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(model.Location_ID);
                    if (branch != null)
                    {
                        if (model.City_ID != branch.CityId || model.Address != branch.Address)
                        {
                            if (model.Latitude == 0 || model.Longitude == 0)
                            {
                                string query = model.Address + "," + _commonService.GetCityName(model.City_ID);
                                if (!String.IsNullOrEmpty(model.Address))
                                {
                                    float latitude = 0;
                                    float longitude = 0;
                                    latlong latlong = _commonService.GetLatLongByAddress(query);
                                    if (latlong.Latitude != 0 && latlong.Longitude != 0)
                                    {
                                        latitude = latlong.Latitude;
                                        longitude = latlong.Longitude;
                                    }
                                    branch.Longitude = longitude;
                                    branch.Latitude = latitude;
                                }
                            }
                            else
                            {
                                branch.Latitude = model.Latitude;
                                branch.Longitude = model.Longitude;
                            }
                        }
                        branch.Address = model.Address;
                        branch.BranchName = model.Name;
                        branch.CityId = model.City_ID;
                        branch.ContactName = model.ContactName;
                        branch.Email = model.Email;
                        branch.Institution_ID = model.Institution_ID;
                        branch.PhoneNumber = model.PhoneNumber;
                        branch.ZipCode = model.ZipCode;
                        var answer = _commonService.UpdateLocation(branch);
                        if (answer)
                        {
                            return Ok("Location updated successfully");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Can't update Location");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Branch ID is not valid");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult InPrcoccessShift([FromHeader] string ApiKey, int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinicalInstitution = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinicalInstitution != null)
                {
                    List<InProcessShiftModel> model = new List<InProcessShiftModel>();
                    List<ClientShifts> clientShifts = _commonService.GetClientShifts(clinicalInstitution.Institution_ID).Where(x => x.Category_ID == 2).ToList();
                    if (clientShifts.Count > 0)
                    {
                        foreach (var clientshift in clientShifts)
                        {
                            string Location = "";
                            if (clientshift.Branch_ID == null || clientshift.Branch_ID == 0)
                            {
                                Location = clinicalInstitution.InstitutionName;
                            }
                            else
                            {
                                Location = _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName;
                            }
                            List<ApplicantAppliedShifts> appliedShifts = _commonService.GetAppliedShiftsbyClientShift_ID(clientshift.ClientShift_ID).Where(x => x.Accepted == true).OrderBy(x => x.Applicant_ID).ToList();

                            if (appliedShifts != null)
                            {
                                List<int> Applicant_IDs = appliedShifts.Select(x => x.Applicant_ID).Distinct().ToList();

                                foreach (int Applicant_ID in Applicant_IDs)
                                {
                                    List<ApplicantAppliedShifts> applicantAppliedShifts = appliedShifts.Where(x => x.Applicant_ID == Applicant_ID).ToList();
                                    List<ApplicantClockInClockOutTime> clockInClockOutTimes = new List<ApplicantClockInClockOutTime>();
                                    List<ApplicantClockIn> clockin = new List<ApplicantClockIn>();
                                    foreach (ApplicantAppliedShifts shift in applicantAppliedShifts)
                                    {
                                        clockInClockOutTimes.AddRange(_commonService.GetAppliedShiftClockinClockouttimes(shift.AppliedShift_ID).OrderBy(x => x.WorkingDay).ToList());
                                    }
                                    foreach (ApplicantClockInClockOutTime clockintime in clockInClockOutTimes)
                                    {
                                        clockin.Add(new ApplicantClockIn()
                                        {
                                            Manually = clockintime.Manually,
                                            WorkEndTime = clockintime.WorkEndTime,
                                            WorkingDay = clockintime.WorkingDay,
                                            WorkStartTime = clockintime.WorkStartTime
                                        });
                                    }
                                    TimeSpan time = TimeSpan.Zero;
                                    List<ApplicantClockInClockOutTime> completeclockintimes = clockInClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                                    if (completeclockintimes.Count > 0)
                                    {
                                        foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                                        {
                                            time += times.ClockOutTime - times.ClockInTime;
                                        }
                                    }
                                    Applicants app = _commonService.GetApplicantbyId(Applicant_ID);
                                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                                    List<int> Speciality_IDs = _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList();
                                 
                                    model.Add(new InProcessShiftModel
                                    {

                                        Applicant = new ApplicantDetailModel()
                                        {
                                            FirstName = app.FirstName,
                                            LastName = app.LastName,
                                            MiddleName = app.MiddleName,
                                            Imgsrc = app.ProfileImage == null || app.ProfileImage == "" ? "/Upload/User.png" : "/" + app.ProfileImage.Replace('\\', '/'),
                                            PhoneNumber = user.PhoneNumber,
                                            SpecialitesString = Speciality_IDs != null ? String.Join("; ", _commonService.GetSpecialities().Where(x => Speciality_IDs.Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()) : "",
                                            Address = user.Address + " " + _commonService.GetCityName(user.City_ID),
                                            CityName = _commonService.GetCitiesByCityid(user.City_ID).city_name,
                                            IsAvailable = app.IsAvailable,
                                            Latitude = user.Latitude,
                                            Longitude = user.Longitude,
                                            State = _commonService.GetAllStates(231).Where(x => x.id == _commonService.GetCitiesByCityid(user.City_ID).state_id).FirstOrDefault().state_name,
                                            Email = user.Email,
                                            IsEligible = app.IsEligible,
                                            ZipCode = user.ZipCode,
                                            VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).FirstOrDefault().VisaStatus,
                                            Availability =  "",
                                        },
                                        Shift = new ClientShiftModel()
                                        {
                                            Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                            Institution_ID = clientshift.Institution_ID,
                                            BranchName = Location,
                                            Category_ID = clientshift.Category_ID,
                                            ClientShift_ID = clientshift.ClientShift_ID,
                                            ClockInTime = clientshift.ClockInTime,
                                            ClockOutTime = clientshift.ClockOutTime,
                                            ContractorCount = clientshift.ContractorCount,
                                            DateOfShift = clientshift.DateOfShift,
                                            EndDate = clientshift.EndDate,
                                            HourlyRate = clientshift.HourlyRate,
                                            Occurrences = clientshift.Occurrences,
                                            Responsibility = clientshift.Responsibility,
                                            ShiftDescription = clientshift.ShiftDescription,
                                            ShiftExpirationDate = clientshift.ShiftExpirationDate,
                                            ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).FirstOrDefault().ShiftLabelName,
                                            ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                            SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                                            StartDate = clientshift.StartDate,
                                            ShiftsDates = clientshift.ShiftsDates
                                        },
                                        WorkedHours = time.TotalHours.ToString("0.00"),
                                        Applicant_ID = app.Applicant_ID,
                                        ClientShift_ID = clientshift.ClientShift_ID,
                                        ClockinClockOutTimes = clockin,
                                        NumberofShift = clockInClockOutTimes.Count,
                                        CompletedNumberofShift = clockInClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count()

                                    });
                                }
                            }
                        }

                    }
                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Clinical Institution ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult InPrcoccessShiftDetail([FromHeader] string ApiKey, int ClientShift_ID, int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClientShifts clientshift = _commonService.GetClientShiftByID(ClientShift_ID);
                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);

                if (clientshift != null && app != null)
                {
                    ClinicalInstitutions clinicalInstitution = _commonService.GetClinicalInstitution_byID(clientshift.Institution_ID);
                    InProcessShiftModel model = new InProcessShiftModel();
                    string Location = "";
                    if (clientshift.Branch_ID == null || clientshift.Branch_ID == 0)
                    {
                        Location = clinicalInstitution.InstitutionName;
                    }
                    else
                    {
                        Location = _commonService.GetlocationbyId(clientshift.Branch_ID.GetValueOrDefault()).BranchName;
                    }
                    List<ApplicantAppliedShifts> appliedShifts = _commonService.GetAppliedShiftsbyClientShift_ID(clientshift.ClientShift_ID).Where(x => x.Accepted == true && x.Applicant_ID == Applicant_ID).OrderBy(x => x.Applicant_ID).ToList();

                    if (appliedShifts.Count > 0)
                    {

                        List<ApplicantAppliedShifts> applicantAppliedShifts = appliedShifts.Where(x => x.Applicant_ID == Applicant_ID).ToList();
                        List<ApplicantClockInClockOutTime> clockInClockOutTimes = new List<ApplicantClockInClockOutTime>();
                        List<ApplicantClockIn> clockin = new List<ApplicantClockIn>();
                        foreach (ApplicantAppliedShifts shift in applicantAppliedShifts)
                        {
                            clockInClockOutTimes.AddRange(_commonService.GetAppliedShiftClockinClockouttimes(shift.AppliedShift_ID).OrderBy(x => x.WorkingDay).ToList());
                        }
                        foreach (ApplicantClockInClockOutTime clockintime in clockInClockOutTimes)
                        {
                            clockin.Add(new ApplicantClockIn()
                            {
                                Manually = clockintime.Manually,
                                WorkEndTime = clockintime.WorkEndTime,
                                WorkingDay = clockintime.WorkingDay,
                                WorkStartTime = clockintime.WorkStartTime
                            });
                        }
                        TimeSpan time = TimeSpan.Zero;
                        List<ApplicantClockInClockOutTime> completeclockintimes = clockInClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).ToList();
                        if (completeclockintimes.Count > 0)
                        {
                            foreach (ApplicantClockInClockOutTime times in completeclockintimes)
                            {
                                time += times.ClockOutTime - times.ClockInTime;
                            }
                        }

                        ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                        List<int> Speciality_IDs = _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList();
                      
                        model = new InProcessShiftModel
                        {

                            Applicant = new ApplicantDetailModel()
                            {
                                FirstName = app.FirstName,
                                LastName = app.LastName,
                                MiddleName = app.MiddleName,
                                Imgsrc = app.ProfileImage == null || app.ProfileImage == "" ? "/Upload/User.png" : "/" + app.ProfileImage.Replace('\\', '/'),
                                PhoneNumber = user.PhoneNumber,
                                SpecialitesString = Speciality_IDs != null ? String.Join("; ", _commonService.GetSpecialities().Where(x => Speciality_IDs.Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()) : "",
                                Address = user.Address + " " + _commonService.GetCityName(user.City_ID),
                                CityName = _commonService.GetCitiesByCityid(user.City_ID).city_name,
                                IsAvailable = app.IsAvailable,
                                Latitude = user.Latitude,
                                Longitude = user.Longitude,
                                State = _commonService.GetAllStates(231).Where(x => x.id == _commonService.GetCitiesByCityid(user.City_ID).state_id).FirstOrDefault().state_name,
                                Email = user.Email,
                                IsEligible = app.IsEligible,
                                ZipCode = user.ZipCode,
                                VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).FirstOrDefault().VisaStatus,
                                Availability =  "",
                            },
                            Shift = new ClientShiftModel()
                            {
                                Branch_ID = clientshift.Branch_ID.GetValueOrDefault(),
                                Institution_ID = clientshift.Institution_ID,
                                BranchName = Location,
                                Category_ID = clientshift.Category_ID,
                                ClientShift_ID = clientshift.ClientShift_ID,
                                ClockInTime = clientshift.ClockInTime,
                                ClockOutTime = clientshift.ClockOutTime,
                                ContractorCount = clientshift.ContractorCount,
                                DateOfShift = clientshift.DateOfShift,
                                EndDate = clientshift.EndDate,
                                HourlyRate = clientshift.HourlyRate,
                                Occurrences = clientshift.Occurrences,
                                Responsibility = clientshift.Responsibility,
                                ShiftDescription = clientshift.ShiftDescription,
                                ShiftExpirationDate = clientshift.ShiftExpirationDate,
                                ShiftLabelName = _commonService.GetShiftLabels().Where(x => x.ShiftLabel_ID == clientshift.ShiftLabel_ID).FirstOrDefault().ShiftLabelName,
                                ShiftLabel_ID = clientshift.ShiftLabel_ID,
                                SpecialitiesName = String.Join("; ", _commonService.GetSpecialities().Where(x => _commonService.GetShiftSpecialities(clientshift.ClientShift_ID).Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()),
                                StartDate = clientshift.StartDate,
                                ShiftsDates = clientshift.ShiftsDates
                            },
                            WorkedHours = time.TotalHours.ToString("0.00"),
                            Applicant_ID = app.Applicant_ID,
                            ClientShift_ID = clientshift.ClientShift_ID,
                            ClockinClockOutTimes = clockin,
                            NumberofShift = clockInClockOutTimes.Count,
                            CompletedNumberofShift = clockInClockOutTimes.Where(x => x.ClockInTime != DateTime.MinValue && x.ClockOutTime != DateTime.MinValue).Count()

                        };
                        var workhistories = _commonService.GetApplicantWorkHistory(Applicant_ID);
                        if (workhistories != null)
                        {
                            model.Applicant.WorkHistories = new List<ApplicantWorkHistoryModel>();
                            foreach (ApplicantWorkHistories workHistory in workhistories)
                            {
                                model.Applicant.WorkHistories.Add(new ApplicantWorkHistoryModel
                                {
                                    EndDate = workHistory.EndDate,
                                    StartDate = workHistory.StartDate,
                                    PlaceName = workHistory.PlaceName,
                                    UntilNow = workHistory.EndDate == DateTime.MinValue ? true : false,
                                     JobTitle=workHistory.JobTitle,
                                    WorkHistory_ID = workHistory.WorkHistory_ID,
                                    SpecialityName = workHistory.JobTitle

                                });
                            }
                        }
                        var Specialities = _commonService.GetApplicantSpecialities(Applicant_ID);
                        if (Specialities != null)
                        {
                            model.Applicant.Specialities = new List<ApplicantSpecialtyModel>();
                            foreach (ApplicantSpecialities speciality in Specialities)
                            {
                                model.Applicant.Specialities.Add(new ApplicantSpecialtyModel
                                {
                                    LegabilityStates = _commonService.GetStates(231).Where(x => speciality.LegabilityStates.Split("; ").ToList().Contains(x.id.ToString())).Select(x => x.state_name).ToList(),
                                    License = speciality.License,
                                    Speciality_ID = speciality.Speciality_ID,

                                    SpecialityName = _commonService.GetSpecialities().Where(x => x.Speciality_ID == speciality.Speciality_ID).FirstOrDefault().SpecialityName


                                });
                            }
                        }
                        var certificates = _commonService.GetApplicantCertificates(Applicant_ID);
                        if (certificates != null)
                        {
                            model.Applicant.Certificates = new List<ApplicantCertificateModel>();

                            foreach (ApplicantCertificates certificate in certificates)
                            {
                                model.Applicant.Certificates.Add(new ApplicantCertificateModel()
                                {
                                    CeritifcationImgsrc = certificate.CeritificationImg,
                                    CertificateType = certificate.CertificateTypes,
                                    Certification_ID = certificate.Ceritification_ID,
                                    Applicant_ID = certificate.Applicant_ID


                                });
                            }

                        }

                        return Ok(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "There  is no Applied Shifts for ClientShift ID and Applicant ID");
                    }
                }


                else
                {
                    ModelState.AddModelError("", "Client Shift ID or Applicant ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult ApplicantDetail([FromHeader] string ApiKey, int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {

                Applicants app = _commonService.GetApplicantbyId(Applicant_ID);

                if (app != null)
                {

                    ApplicantDetailModel model = new ApplicantDetailModel();



                    ApplicationUser user = _userManager.Users.Where(x => x.Id == app.User_ID).FirstOrDefault();
                    List<int> Speciality_IDs = _commonService.GetApplicantSpecialities(app.Applicant_ID).Select(x => x.Speciality_ID).ToList();

              
                    model = new ApplicantDetailModel()
                    {
                        FirstName = app.FirstName,
                        LastName = app.LastName,
                        MiddleName = app.MiddleName,
                        Imgsrc = app.ProfileImage == null || app.ProfileImage == "" ? "/Upload/User.png" : "/" + app.ProfileImage.Replace('\\', '/'),
                        PhoneNumber = user.PhoneNumber,
                        SpecialitesString = Speciality_IDs != null ? String.Join("; ", _commonService.GetSpecialities().Where(x => Speciality_IDs.Contains(x.Speciality_ID)).Select(x => x.SpecialityName).ToList()) : "",
                        Address = user.Address + " " + _commonService.GetCityName(user.City_ID),
                        CityName = _commonService.GetCitiesByCityid(user.City_ID).city_name,
                        IsAvailable = app.IsAvailable,
                        Latitude = user.Latitude,
                        Longitude = user.Longitude,
                        State = _commonService.GetAllStates(231).Where(x => x.id == _commonService.GetCitiesByCityid(user.City_ID).state_id).FirstOrDefault().state_name,
                        Email = user.Email,
                        IsEligible = app.IsEligible,
                        ZipCode = user.ZipCode,
                        VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == app.VisaStatus_ID).FirstOrDefault().VisaStatus,
                        Availability = "",



                    };
                    var workhistories = _commonService.GetApplicantWorkHistory(Applicant_ID);
                    if (workhistories != null)
                    {
                        model.WorkHistories = new List<ApplicantWorkHistoryModel>();
                        foreach (ApplicantWorkHistories workHistory in workhistories)
                        {
                            model.WorkHistories.Add(new ApplicantWorkHistoryModel
                            {
                                EndDate = workHistory.EndDate,
                                StartDate = workHistory.StartDate,
                                PlaceName = workHistory.PlaceName,
                                UntilNow = workHistory.EndDate == DateTime.MinValue ? true : false,
                                JobTitle = workHistory.JobTitle,
                                WorkHistory_ID = workHistory.WorkHistory_ID,
                                SpecialityName = workHistory.JobTitle,

                            });
                        }
                    }
                    var Specialities = _commonService.GetApplicantSpecialities(Applicant_ID);
                    if (Specialities != null)
                    {
                        model.Specialities = new List<ApplicantSpecialtyModel>();
                        foreach (ApplicantSpecialities speciality in Specialities)
                        {
                            model.Specialities.Add(new ApplicantSpecialtyModel
                            {
                                LegabilityStates = _commonService.GetStates(231).Where(x => speciality.LegabilityStates.Split("; ").ToList().Contains(x.id.ToString())).Select(x => x.state_name).ToList(),
                                License = speciality.License,
                                Speciality_ID = speciality.Speciality_ID,

                                SpecialityName = _commonService.GetSpecialities().Where(x => x.Speciality_ID == speciality.Speciality_ID).FirstOrDefault().SpecialityName


                            });
                        }
                    }
                    var certificates = _commonService.GetApplicantCertificates(Applicant_ID);
                    if (certificates != null)
                    {
                        model.Certificates = new List<ApplicantCertificateModel>();

                        foreach (ApplicantCertificates certificate in certificates)
                        {
                            model.Certificates.Add(new ApplicantCertificateModel()
                            {
                                CeritifcationImgsrc = certificate.CeritificationImg,
                                CertificateType = certificate.CertificateTypes,
                                Certification_ID = certificate.Ceritification_ID,
                                Applicant_ID = certificate.Applicant_ID


                            });
                        }

                    }

                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Applicant ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public IActionResult Search([FromHeader] string ApiKey, int Institution_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinical != null)

                {
                    List<ApplicantSearchModel> model = new List<ApplicantSearchModel>();
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                    List<Applicants> applicants = _commonService.Search(Specialities: null, StartDate: DateTime.MinValue, EndDate:DateTime.MinValue, distance: 5000, latitude: user.Latitude, longitude: user.Longitude);
                    if (applicants.Count > 0)
                    {
                        foreach (Applicants applicant in applicants)
                        {
                            var clientshifts = _commonService.GetAvailableShiftsforApplicant(applicant.Applicant_ID, clinical.Institution_ID);
                            if (clientshifts != null)
                            {
                                List<string> certificates = _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateTypes).ToList();
                              
                                model.Add(new ApplicantSearchModel()
                                {
                                    IsAvailable = applicant.IsAvailable,
                                    Applicant_ID = applicant.Applicant_ID,
                                    FirstName = applicant.FirstName,
                                    LastName = applicant.LastName,

                                    VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).Select(x => x.VisaStatus).FirstOrDefault(),
                                  
                                    Certificates = String.Join(", ", certificates),
                                    SpecialitesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                                });
                            }

                        }
                    }
                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError("", "Clinical Institution ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult Search([FromHeader] string ApiKey, int Institution_ID, [FromBody]SearchModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                if (clinical != null)

                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == clinical.User_ID).FirstOrDefault();
                    float latitude = user.Latitude;
                    float longitude = user.Longitude;
                    ClinicalInstitutionBranches branch = _commonService.GetlocationbyId(model.Location_ID);
                    if (branch != null)
                    {
                        latitude = branch.Latitude;
                        longitude = branch.Longitude;
                    }
                    List<ApplicantSearchModel> applicantmodel = new List<ApplicantSearchModel>();

                    List<Applicants> applicants = _commonService.Search(Specialities: model.Specialities, StartDate:DateTime.MinValue, EndDate:DateTime.MinValue, distance: model.Distance, latitude: latitude, longitude: longitude);
                    if (applicants.Count > 0)
                    {
                        foreach (Applicants applicant in applicants)
                        {
                            var clientshifts = _commonService.GetAvailableShiftsforApplicant(applicant.Applicant_ID, clinical.Institution_ID);
                            if (clientshifts != null)
                            {
                                List<string> certificates = _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateTypes).ToList();
                              
                                applicantmodel.Add(new ApplicantSearchModel()
                                {
                                    IsAvailable = applicant.IsAvailable,
                                    Applicant_ID = applicant.Applicant_ID,
                                    FirstName = applicant.FirstName,
                                    LastName = applicant.LastName,

                                    VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).Select(x => x.VisaStatus).FirstOrDefault(),
                                   
                                    Certificates = String.Join(", ", certificates),
                                    SpecialitesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                                });
                            }

                        }
                    }
                    return Ok(new { applicantmodel, model });
                }
                else
                {
                    ModelState.AddModelError("", "Clinical Institution ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult InviteApplicant([FromHeader] string ApiKey, int Institution_ID, int Applicant_ID)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
                ClinicalInstitutions clinical = _commonService.GetClinicalInstitution_byID(Institution_ID);
                Applicants applicant = _commonService.GetApplicantbyId(Applicant_ID);
                ApplicantInviteModel model = new ApplicantInviteModel();
                if (clinical != null && applicant != null)
                {
                    ApplicationUser user = _userManager.Users.Where(x => x.Id == applicant.User_ID).FirstOrDefault();
                    List<string> certificates = _commonService.GetApplicantCertificates(applicant.Applicant_ID).Select(x => x.CertificateTypes).ToList();
                   
                    model.Applicant = new ApplicantSearchModel()
                    {
                        Applicant_ID = applicant.Applicant_ID,
                        ImgSrc = applicant.ProfileImage == null || applicant.ProfileImage == "" ? "/Upload/User.png" : "/" + applicant.ProfileImage.Replace('\\', '/'),
                        FirstName = applicant.FirstName,
                        LastName = applicant.LastName,
                        VisaStatus = _commonService.GetVisaStatuses().Where(x => x.VisaStatus_ID == applicant.VisaStatus_ID).Select(x => x.VisaStatus).FirstOrDefault(),
                      
                        Certificates = String.Join(", ", certificates),
                        SpecialitesString = String.Join(", ", _commonService.GetSpecialities().Where(s => _commonService.GetApplicantSpecialities(applicant.Applicant_ID).Select(x => x.Speciality_ID).ToList().Contains(s.Speciality_ID)).Select(s => s.SpecialityName).ToList()),
                    };
                    List<ApplicantAppliedShifts> appliedShifts = _commonService.GetApplicantAppliedShifts(applicant.User_ID);
                    List<int> AppliedClientShift_IDs = appliedShifts.Select(x => x.ClientShift_ID).ToList();
                    List<string> applieddays = new List<string>();
                    foreach (ApplicantAppliedShifts appliedShift in appliedShifts)
                    {
                        if (appliedShift.Status == 1 && appliedShift.Accepted == true)
                        {
                            applieddays.AddRange(appliedShift.AppliedDaysList.Split(","));
                        }
                    }
                    var shifts = _commonService.GetAvailableShiftsforApplicant(applicant.Applicant_ID, clinical.Institution_ID);
                    if (shifts != null)
                    {
                        List<ClientShifts> clientShifts = shifts;

                        List<ApplicantAvailables> applicantAvailables = _commonService.GetApplicantAvailables(applicant.Applicant_ID);
                        List<ClientShifts> RemovableShifts = new List<ClientShifts>();
                        foreach (ClientShifts clientShift in clientShifts)
                        {
                            if (AppliedClientShift_IDs.Contains(clientShift.ClientShift_ID))
                            {
                                RemovableShifts.Add(clientShift);
                            }
                        }
                        foreach (ClientShifts removeableshift in RemovableShifts)
                        {
                            clientShifts.Remove(removeableshift);
                        }
                        model.Shifts = new List<InviteShiftModel>();
                        foreach (ClientShifts shift in clientShifts)
                        {
                            if (shift != null)
                            {
                              

                                List<string> freedays = new List<string>();
                                freedays = applicantAvailables.Select(x=>x.AvailableDay.ToString()).ToList();

                                List<string> appliedshiftdays = new List<string>();
                                List<ApplicantAppliedShifts> appliedDays = _commonService.GetAppliedShiftsbyClientShift_ID(shift.ClientShift_ID).Where(x => x.Status == 1 && x.Accepted && x.Applicant_ID != applicant.Applicant_ID).ToList();
                                if (appliedDays != null)
                                {

                                    foreach (ApplicantAppliedShifts appliedshift in appliedDays)
                                    {
                                        appliedshiftdays.AddRange(appliedshift.AppliedDaysList.Split(','));
                                    }
                                    foreach (string day in appliedshiftdays)
                                    {
                                        if (appliedshiftdays.FindAll(x => x == day).Count() == shift.ContractorCount)
                                            freedays.Remove(day);
                                    }
                                }
                                var removabledays = new List<string>();
                                var shiftdays = shift.ShiftsDates.Split(",").ToList();
                                foreach (string day in freedays)
                                {
                                    if (applieddays.Contains(day))
                                        removabledays.Add(day);
                                    if (!shiftdays.Contains(day))
                                        removabledays.Add(day);
                                }


                                if (removabledays.Count > 0)
                                {
                                    freedays.Where(x => !removabledays.Contains(x)).ToList();
                                }


                                List<string> Specialites = _commonService.GetSpecialities().
                                                                                        Where(x => _commonService.GetShiftSpecialities(shift.ClientShift_ID).Contains(x.Speciality_ID))
                                                                                        .Select(x => x.SpecialityName).ToList();
                                if (freedays.Count > 0)
                                {
                                    model.Shifts.Add(new InviteShiftModel()
                                    {

                                        ClientShift_ID = shift.ClientShift_ID,
                                        ClockInTime = shift.ClockInTime,
                                        ClockOutTime = shift.ClockOutTime,
                                        StartDate = shift.StartDate,
                                        EndDate = shift.EndDate,
                                        Occurrences = shift.Occurrences,

                                        Institution_ID = shift.Institution_ID,
                                        SpecialitiesName = String.Join("; ", Specialites),
                                        Branch_ID = shift.Branch_ID.GetValueOrDefault(),
                                        BranchName = shift.Branch_ID != null ? _commonService.GetlocationbyId(shift.Branch_ID.GetValueOrDefault()).BranchName : clinical.InstitutionName,
                                        ActiveDates = String.Join(";", freedays)

                                    });
                                }
                            }
                        }

                        if(model.Shifts.Count>0)
                        {
                            return Ok(model);
                        }
                        else
                        {
                            ModelState.AddModelError("", "There is no Shift for Applicant");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "There is no Shift for Applicant");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Clinical Institution ID or Applicant ID is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
}

        [HttpPost]
        public IActionResult InviteApplicant ([FromHeader] string ApiKey, [FromBody]AppliedShiftsModel model)
        {
            Guid Api = Guid.Empty;
            if (!String.IsNullOrEmpty(ApiKey))
            {
                Guid.TryParse(ApiKey, out Api);
            }
            var apiAnswer = _commonService.CheckFullApiKey(Api);
            if (apiAnswer)
            {
            if(ModelState.IsValid)
                {
                    ClientShifts shift = _commonService.GetClientShiftByID(model.ClientShift_ID);
                    Applicants app = _commonService.GetApplicantbyId(model.Applicant_ID);
                    if(app!=null && shift!=null)
                    {
                        if (model.AppliedDays.Count > 0)
                        {
                            List<DateTime> dates = model.AppliedDays.Select(date => DateTime.Parse(date)).ToList();
                            List<DateTime> removabledates = new List<DateTime>();
                            removabledates= dates.Where(x => x < shift.StartDate || x > shift.EndDate).ToList();
                            if(removabledates.Count>0)
                            {
                                dates = dates.Where(x => !removabledates.Contains(x)).ToList();
                            }
                            if (dates.Count > 0)
                            {
                                ApplicantAppliedShifts appliedShift = new ApplicantAppliedShifts();
                                appliedShift.Remarks = model.Remarks;
                                appliedShift.AppliedDaysList = String.Join(",", dates.Select(x=>x.ToString("MM/dd/yyyy")).ToList());
                                appliedShift.ClientShift_ID = model.ClientShift_ID;
                                appliedShift.Applicant_ID = model.Applicant_ID;
                                appliedShift.Accepted = false;
                                appliedShift.Status = 0;
                                appliedShift.Invited = "Invited by Client";
                                appliedShift.AppliedAllDays = dates.Count == shift.Occurrences ? true : false;

                                var answer = _commonService.AddApplicantAppliedShift(appliedShift);
                                if (answer)
                                {
                                    Notifications notification = new Notifications()
                                    {
                                        NotificationTemplate_ID = 6,
                                        Status = 1,
                                        User_ID = app.User_ID,
                                        Special_ID = appliedShift.AppliedShift_ID

                                    };
                                    _commonService.AddNotification(notification);

                                }

                            }
                            else
                            {
                                ModelState.AddModelError("", "Applied Days is empty");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Applied Days is empty");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Client Shift ID  or Applicant ID is not valid");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Api Key is not valid");
            }
            return BadRequest(ModelState);
        }
        private string SaveLogoFile(IFormFile file, string path, string filename)
        {
            string foldername = "Upload";
            if (!Directory.Exists(Path.Combine(_environment.WebRootPath, foldername, path)))
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, foldername, path));
            }
            if (!Directory.Exists(Path.Combine(_environment.WebRootPath, foldername, path, filename)))
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, foldername, path, filename));
            }
            string extention = file.FileName.Substring(file.FileName.LastIndexOf('.'), file.FileName.Length - file.FileName.LastIndexOf('.'));

            var newfilename = Guid.NewGuid().ToString() + extention;
            using (FileStream fs = new FileStream(Path.Combine(_environment.WebRootPath, foldername, path, filename, newfilename), FileMode.Create))
            {
                try
                {
                    file.CopyTo(fs);
                    fs.Flush();
                    return foldername + "\\" + path + "\\" + filename + "\\" + newfilename;
                }
                catch
                { return ""; }
            }
        }

    }
}