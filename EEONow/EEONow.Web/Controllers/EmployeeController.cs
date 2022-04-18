using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Reflection;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("Employee")]
    [Authorize()]
    public class EmployeeController : Controller
    {
        IEmployeeService _EmployeeService;
        IOrganizationsService _OrganizationsService;
        ICountiesService _CountiesService;
        IEEOJobCategoryService _EEOJobCategoryService;
        IRaceService _RaceService;
        IAgencyYearsOfService _AgencyYearsOfService;
        IStatesService _StateService;
        IAccountService _AccountService;
        ILastPerformanceRating _LastPerformanceRating;
        IAgeRange _AgeRange;
        ISalaryRange _SalaryRange;
        IVacancyRange _VacancyRange;
        IPositionYearsOfService _PositionYearsOfService;

        public EmployeeController()
        {

            _EmployeeService = new EmployeeService();
            _OrganizationsService = new OrganizationService();
            _CountiesService = new CountyService();
            _EEOJobCategoryService = new EEOJobCategoryService();
            _RaceService = new RaceService();
            _AgencyYearsOfService = new AgencyYearsOfServiceService();
            _StateService = new StateService();
            _AccountService = new AccountService();
            _LastPerformanceRating = new LastPerformanceRatingService();
            _AgeRange = new AgeRangeService();
            _SalaryRange = new SalaryRangeService();
            _VacancyRange = new VacancyRangeService();
            _PositionYearsOfService = new PositionYearsOfServiceService();
        }

       [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            try
            {
                LoginResponse _Loginmodel = Utilities.AppUtility.DecryptCookie();
                int? organizationID = _Loginmodel.OrgId;

                if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
                {
                    return RedirectToAction("EmployeeViaOrganization");
                }

                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();

                ViewBag.GenderList = new List<SelectListItem>() {
                    new SelectListItem(){ Text="Male", Value="1"},
                    new SelectListItem(){ Text="Female", Value="2"}
                };

                ViewBag.RaceList = await _RaceService.BindRaceDropDown(organizationID);
                ViewBag.EEOCodeList = await _EEOJobCategoryService.BindEEOJobCategoryDropDown(organizationID);
                ViewBag.AgencyYearsOfServiceList = await _AgencyYearsOfService.BindAgencyYearsOfServiceDropDown(organizationID);
                ViewBag.StateList = await _StateService.BindStateDropDown();
                ViewBag.CountyList = await _CountiesService.BindCountyDropDown();
                ViewBag.LastPerformanceRatingList = await _LastPerformanceRating.BindLastPerformanceRatingDropDown(organizationID);

                ViewBag.AgeRangeList = await _AgeRange.BindAgeRangeDropDown(organizationID);
                ViewBag.SalaryRangeList = await _SalaryRange.BindSalaryRangeDropDown(organizationID);
                ViewBag.VacancyDateRangeList = await _VacancyRange.BindVacancyRangeDropDown(organizationID);
                ViewBag.PositionYearsOfServiceList = await _PositionYearsOfService.BindPositionYearsOfServiceDropDown(organizationID);
                ViewBag.EmployeeLevelList = await _EmployeeService.BindEmployeeLevelDropDown(organizationID);

                var model = _EmployeeService.BindEmployeeFilterModel();
                model.ListEmployeeActiveColumn = await _EmployeeService.BindEmployeeActiveColumn();

                return View(model);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindEmployeeModel([DataSourceRequest] DataSourceRequest request, EmployeeSearchModel _SearchModel)
        {
            try
            {
                if (request.PageSize > 200 && request.PageSize > 1350)
                {
                    request.PageSize = 1350;
                }
                var model = await _EmployeeService.GetEmployeeViaOrganizationsModel(_SearchModel.FileSubmissionFilterId, _SearchModel.region);//, 1);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateEmployee([DataSourceRequest] DataSourceRequest request, EmployeesModel __EmployeeModel)
        {
            try
            {
                if (__EmployeeModel != null && ModelState.IsValid)
                {
                    var model = await _EmployeeService.UpdateEmployee(__EmployeeModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { __EmployeeModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Upload)
        {
            return null;
        }
        public async Task<ActionResult> EmployeeViaOrganization(int ? orgID)
        {
            try
            {
                LoginResponse _Loginmodel = Utilities.AppUtility.DecryptCookie();
               
                if (_Loginmodel.Roles != "DefinedSoftwareAdministrator")
                {
                    return RedirectToAction("Index");
                }

                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();

                ViewBag.GenderList = new List<SelectListItem>() {
                    new SelectListItem(){ Text="Male", Value="1"},
                    new SelectListItem(){ Text="Female", Value="2"}
                };
                ViewBag.RaceList = await _RaceService.BindRaceDropDown(orgID);
                ViewBag.EEOCodeList = await _EEOJobCategoryService.BindEEOJobCategoryDropDown(orgID);
                ViewBag.AgencyYearsOfServiceList = await _AgencyYearsOfService.BindAgencyYearsOfServiceDropDown(orgID);
                ViewBag.StateList = await _StateService.BindStateDropDown();
                ViewBag.CountyList = await _CountiesService.BindCountyDropDown();
                ViewBag.LastPerformanceRatingList = await _LastPerformanceRating.BindLastPerformanceRatingDropDown(orgID);
                ViewBag.VacancyDateRangeList = await _VacancyRange.BindVacancyRangeDropDown(orgID);
                ViewBag.AgeRangeList = await _AgeRange.BindAgeRangeDropDown(orgID);
                ViewBag.SalaryRangeList = await _SalaryRange.BindSalaryRangeDropDown(orgID);
                ViewBag.PositionYearsOfServiceList = await _PositionYearsOfService.BindPositionYearsOfServiceDropDown(orgID);
                ViewBag.EmployeeLevelList = await _EmployeeService.BindEmployeeLevelDropDown(orgID);
                if (orgID==null)
                {
                    orgID = 0;
                }
                ViewBag.orgID = orgID.ToString();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindEmployeeViaOrganizationsModel([DataSourceRequest] DataSourceRequest request, EmployeeSearchModel _SearchModel)
        {
            try
            {
                if (request.PageSize > 200 && request.PageSize > 1350)
                {
                    request.PageSize = 1350;
                }
                var model = await _EmployeeService.GetEmployeeViaOrganizationsModel(_SearchModel.FileSubmissionFilterId, _SearchModel.region);//, 0);
                var result = model.ToDataSourceResult(request);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        public async Task<ActionResult> GetOrganizationsList()
        {
            try
            {
                var model = await _OrganizationsService.BindOrganizationDropDown();
                return Json(model.Select(p => new { OrganizationId = p.Value, OrganizationName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetFileViaOrganisation(int? organization)
        {
            try
            {
                var model = _EmployeeService.GetFileSubmissionViaOrganisation(organization);
                return Json(model.Select(p => new { FileSubmissionId = p.Value, FileName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public ActionResult GetGridFilters([DataSourceRequest] DataSourceRequest request)
        {
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ExportToExcel(int FileSubmissionFilterId, string region, string SelectedColumns)
        {
            try
            {

                if (FileSubmissionFilterId > 0)
                {
                    var model = await _EmployeeService.GetExportEmployeeViaOrganizationsModel(FileSubmissionFilterId, region);
                    if (model.Count() > 0 && model != null)
                    {
                        var _ResultData = model.ToList();
                        DataTable dt = ToDataTable(_ResultData);
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        Dictionary<string, string> dictCurrentColoumn = new Dictionary<string, string>();
                        dict.Add("EmployeeId", "EmployeeId");
                        dict.Add("firstname", "FirstName");
                        dict.Add("middlename", "MiddleName");
                        dict.Add("lastname", "LastName");
                        dict.Add("phonenumber", "PhoneNumber");
                        dict.Add("email", "Email");
                        dict.Add("positiontitle", "PositionTitle");
                        dict.Add("programofficename", "ProgramOfficeName");
                        dict.Add("salary", "Salary");
                        dict.Add("salaryrangeid", "SalaryRange");
                        dict.Add("positionnumber", "PositionNumber");
                        dict.Add("supervisorpositionnumber", "SupervisorPositionNumber");
                        dict.Add("nationalorigin", "NationalOrigin");
                        dict.Add("age", "Age");
                        dict.Add("agerangeid", "AgeRange");
                        dict.Add("positiondateofhire", "PositionDateOfHire");
                        dict.Add("agencydateofhire", "AgencyDateOfHire");
                        dict.Add("genderid", "Gender");
                        dict.Add("organizationid", "Organization");
                        dict.Add("stateid", "State");
                        dict.Add("countyid", "County");
                        dict.Add("eeocodeid", "EEOCode");
                        dict.Add("agencyyearsofserviceid", "AgencyYearsOfService");
                        dict.Add("positionyearsofserviceid", "PositionYearsOfService");
                        dict.Add("raceid", "Race");
                        dict.Add("lastperformanceratingid", "LastPerformanceRating");
                        dict.Add("lastperformanceratingvalue", "LastPerformanceRatingValue");
                        dict.Add("workaddress", "WorkAddress");
                        dict.Add("workcity", "WorkCity");
                        dict.Add("workzipcode", "WorkZipCode");
                        dict.Add("opsposition", "OPSPosition");
                        dict.Add("statecumulativemonthsofservice", "StateCumulativeMonthsOfService");
                        dict.Add("filled", "Filled");
                        dict.Add("supervisorflag", "SupervisorFlag");
                        dict.Add("employeelevel", "EmployeeLevel");
                        dict.Add("vacancydate", "VacancyDate");
                        dict.Add("vacancydaterangeid", "VacancyDateRange");
                        dict.Add("genderindex", "GenderIndex");
                        dict.Add("raceindex", "RaceIndex");
                        dict.Add("region", "Region");
                        dict.Add("spanofcontrol", "SpanOfControl");


                        dictCurrentColoumn.AddRange(dict);
                        var ListSelectedColumns = SelectedColumns.Split(',');
                        foreach (var item in ListSelectedColumns)
                        {
                            dict.Remove(item);
                        }

                        var dtFilter = dt.Copy();
                        foreach (var column in dict)
                        {
                            dictCurrentColoumn.Remove(column.Key);
                            dtFilter.Columns.Remove(column.Value.ToString());
                        }
                        dtFilter.AcceptChanges();

                        IEnumerable<DataRow> sequence = dtFilter.AsEnumerable();

                        #region To Generate CSV file code section                  
                        StringBuilder builder = new StringBuilder();
                        List<string> columnNames = new List<string>();
                        List<string> rows = new List<string>();

                        //columnNames.Add("Employee Id,First Name,Middle Name,Last Name,Phone Number," +
                        //                "Email,Position Title,Program Office,Salary," +
                        //                "Position Number,Supervisor Position Number,National Origin," +
                        //                "Age,Position Date Of Hire,Agency Date Of Hire,Gender,Organization,State," +
                        //                "County,EEOCode,Agency Years Of Service,Race,Last Performance Rating,Work Address," +
                        //                "Work City,Work Zip Code,OPS Position,State Cumulative Months Of Service,Filled,Supervisor Flag");



                        String ColHeader = "";
                        //foreach (var item in dictCurrentColoumn)
                        //{
                        //    ColHeader = ColHeader + item.Value + ",";
                        //}

                        foreach (DataColumn column in dtFilter.Columns)
                        {
                            ColHeader = ColHeader + column.ColumnName + ",";
                        }

                        ColHeader = ColHeader.Substring(0, ColHeader.Length - 1);
                        columnNames.Add(ColHeader);

                        builder.Append(string.Join(",", columnNames.ToArray())).Append("\n");
                        rows.AddRange(sequence.Select(e => string.Join(",", e.ItemArray)));
                        //rows.AddRange(_ResultData.Select(e => string.Join(",",
                        //                                                       "\"" + Convert.ToString(e.EmployeeId) + "\"",
                        //                                                       "\"" + Convert.ToString(e.FirstName) + "\"",
                        //                                                       "\"" + Convert.ToString(e.MiddleName) + "\"",
                        //                                                       "\"" + Convert.ToString(e.LastName) + "\"",
                        //                                                       "\"" + Convert.ToString(e.PhoneNumber) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Email) + "\"",
                        //                                                       "\"" + Convert.ToString(e.PositionTitle) + "\"",
                        //                                                       "\"" + Convert.ToString(e.ProgramOfficeName) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Salary) + "\"",
                        //                                                       "\"" + Convert.ToString(e.PositionNumber) + "\"",
                        //                                                       "\"" + Convert.ToString(e.SupervisorPositionNumber) + "\"",
                        //                                                       "\"" + Convert.ToString(e.NationalOrigin) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Age) + "\"",
                        //                                                       "\"" + Convert.ToString(e.PositionDateOfHire) + "\"",
                        //                                                       "\"" + Convert.ToString(e.AgencyDateOfHire) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Gender) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Organization) + "\"",
                        //                                                       "\"" + Convert.ToString(e.State) + "\"",
                        //                                                       "\"" + Convert.ToString(e.County) + "\"",
                        //                                                       "\"" + Convert.ToString(e.EEOCode) + "\"",
                        //                                                       "\"" + Convert.ToString(e.AgencyYearsOfService) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Race) + "\"",
                        //                                                       "\"" + Convert.ToString(e.LastPerformanceRating) + "\"",
                        //                                                       "\"" + Convert.ToString(e.WorkAddress.Replace("\"", "")) + "\"",
                        //                                                       "\"" + Convert.ToString(e.WorkCity.Replace("\"", "")) + "\"",
                        //                                                       "\"" + Convert.ToString(e.WorkZipCode) + "\"",
                        //                                                       "\"" + Convert.ToString(e.OPSPosition) + "\"",
                        //                                                       "\"" + Convert.ToString(e.StateCumulativeMonthsOfService) + "\"",
                        //                                                       "\"" + Convert.ToString(e.Filled) + "\"",
                        //                                                       "\"" + Convert.ToString(e.SupervisorFlag) + "\"")));
                        builder.Append(string.Join("\n", rows.ToArray()));
                        #endregion
                        string attachment = "attachment; filename=EmployeeList.csv";
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", attachment);
                        Response.ContentType = "text/csv";
                        Response.AddHeader("Pragma", "public");
                        Response.Write(builder.ToString());

                    }

                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        [HttpPost]
        public ActionResult Employee_Excel_Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }

}