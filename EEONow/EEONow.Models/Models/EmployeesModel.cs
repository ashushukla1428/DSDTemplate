using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EmployeeSearchModel
    {
        public string region { get; set; }
        public int FileSubmissionFilterId { get; set; }
        public List<SelectListItem> ListFileSubmissionFilter { get; set; }

        public int EmployeeActiveColumnId { get; set; }

        public List<SelectListItem> ListEmployeeActiveColumn { get; set; }
    }
    public class EmployeesModel
    {
        [ScaffoldColumn(false)]
        public Int32 EmployeeId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public String OrganizationName { get; set; }
        [ScaffoldColumn(false)]
        public Int32 FileSubmissionId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Gender")]
        public String GenderName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Race")]
        public String RaceName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "EEO Code")]
        public String EEOCodeName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Agency Years Of Service")]
        public String AgencyYearsOfServiceName { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "Position Years Of Service")]
        public String PositionYearsOfServiceName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Age Range")]
        public String AgeRangeName { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "Salary Range")]
        public String SalaryRangeName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Work State")]
        public String StateName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "County")]
        public String CountyName { get; set; }

        [Required]
        [Display(Name = "First Name", Order = 1)]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name", Order = 2)]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name", Order = 3)]
        public string LastName { get; set; }
        [Display(Name = "Phone Number", Order = 4)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email Address", Order = 5)]
        public string Email { get; set; }
        [Display(Name = "Position Title", Order = 6)]
        [Required]
        public String PositionTitle { get; set; }
        [Display(Name = "Program Office Name", Order = 7)]
        [Required]
        public String ProgramOfficeName { get; set; }

        [UIHint("Date")]
        [Display(Name = "Vacancy Date", Order = 8)]
        [Required]
        public DateTime? VacancyDate { get; set; }

        [Display(Name = "Vacancy Date Range")]
        [Required]
        public decimal VacancyDateRangeId { get; set; }


        [Display(Name = "Salary", Order = 8)]
        [Required]
        public decimal Salary { get; set; }

        [Display(Name = "Salary Range")]
        [Required]
        public decimal SalaryRangeId { get; set; }


        [Display(Name = "Position Number", Order = 9)]
        public string PositionNumber { get; set; }
        [Display(Name = "Supervisor Position Number", Order = 10)]
        public string SupervisorPositionNumber { get; set; }

        [Display(Name = "National Origin (Ethnicity)", Order = 11)]
        public string NationalOrigin { get; set; }
        [Display(Name = "Age", Order = 12)]
        //[UIHint("Date")]
        public Int32 Age { get; set; }

        [Display(Name = "Age Range")]
        [Required]
        public decimal AgeRangeId { get; set; }


        [UIHint("Date")]
        [Display(Name = "Position Date Of Hire", Order = 13)]
        public DateTime? PositionDateOfHire { get; set; }
        [UIHint("Date")]
        [Display(Name = "Agency Date Of Hire", Order = 14)]
        public DateTime? AgencyDateOfHire { get; set; }
        [UIHint("GenderList")]
        [Required]
        [Display(Name = "Gender", Order = 15)]
        public Int32 GenderId { get; set; }
        [Display(Name = "Gender Index", Order = 15)]
        public Decimal GenderIndex { get; set; }
        [UIHint("OrganisationList")]
        [Required]
        [Display(Name = "Organization", Order = 16)]
        public Int32 OrganizationId { get; set; }
        [UIHint("EmployeeStateList")]
        [Display(Name = "Work State", Order = 17)]
        public Int32 StateId { get; set; }
        [UIHint("CountyList")]
        [Required]
        [Display(Name = "County", Order = 18)]
        public Int32 CountyId { get; set; }
        [UIHint("EEOCodeList")]
        [Required]
        [Display(Name = "EEO Code", Order = 19)]
        public Int32 EEOCodeId { get; set; }
        [UIHint("AgencyYearsOfServiceList")]
        [Required]
        [Display(Name = "Agency Years Of Service", Order = 20)]
        public Int32 AgencyYearsOfServiceId { get; set; }
        [UIHint("PositionYearsOfServiceList")]
        [Required]
        [Display(Name = "Position Years Of Service")]
        public Int32 PositionYearsOfServiceId { get; set; }
        [UIHint("RaceList")]
        [Required]
        [Display(Name = "Race", Order = 21)]
        public Int32 RaceId { get; set; }

        [Display(Name = "Race Index", Order = 15)]
        public Decimal RaceIndex { get; set; }

        [Display(Name = "Region", Order = 15)]
        public String Region { get; set; }
        [Display(Name = "Span of Control")]
        public Int32 SpanOfControl { get; set; }

        [UIHint("LastPerformanceRatingList")]
        [Display(Name = "Last Performance Rating", Order = 22)]
        public Int32 LastPerformanceRatingId { get; set; }
        [Display(Name = "Last Performance Rating Value", Order = 23)]
        public decimal LastPerformanceRatingValue { get; set; }
        [Display(Name = "Work Address", Order = 24)]
        public String WorkAddress { get; set; }
        [Display(Name = "Work City", Order = 25)]
        public String WorkCity { get; set; }
        [Display(Name = "Work Zip Code", Order = 26)]
        public String WorkZipCode { get; set; }
        [Display(Name = "OPSPosition", Order = 27)]
        public bool OPSPosition { get; set; }
        [Display(Name = "State Cumulative Months Of Service", Order = 28)]
        public int StateCumulativeMonthsOfService { get; set; }
        [Display(Name = "Filled", Order = 29)]
        public bool Filled { get; set; }
        [Display(Name = "Supervisor Flag", Order = 30)]
        public bool SupervisorFlag { get; set; }

        [Display(Name = "Employee Level")]
        public string EmployeeLevel { get; set; }
    }


    public class EmployeeModelForExport
    {
        public String EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Organization { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }
        public String Filled { get; set; }
        public String PositionTitle { get; set; }
        public String ProgramOfficeName { get; set; }
        public String VacancyDate { get; set; }
        public String VacancyDateRange { get; set; }
        public String Salary { get; set; }
        public String SalaryRange { get; set; }
        public String PositionNumber { get; set; }
        public String SupervisorPositionNumber { get; set; }
        public String NationalOrigin { get; set; }
        public String Age { get; set; }
        public String AgeRange { get; set; }
        public String PositionDateOfHire { get; set; }
        public String AgencyDateOfHire { get; set; }
        public String Gender { get; set; }
        public String GenderIndex { get; set; }
        public String State { get; set; }
        public String County { get; set; }
        public String EEOCode { get; set; }
        public String AgencyYearsOfService { get; set; }
        public String PositionYearsOfService { get; set; }
        public String Race { get; set; }
        public String RaceIndex { get; set; }
        public String Region { get; set; }
        public String SpanOfControl { get; set; }
        public String LastPerformanceRating { get; set; }
        public String LastPerformanceRatingValue { get; set; }
        public String WorkAddress { get; set; }
        public String WorkCity { get; set; }
        public String WorkZipCode { get; set; }
        public String OPSPosition { get; set; }
        public String SupervisorFlag { get; set; }
        public String StateCumulativeMonthsOfService { get; set; }
        public string EmployeeLevel { get; set; }
    }
}
