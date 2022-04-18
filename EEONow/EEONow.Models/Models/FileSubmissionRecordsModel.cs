using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Models
{
    public class FileVersionClass
    {
        public int FileSubmissionId { get; set; }
        public string SubmissionDateTime { get; set; }
        public Int32 FileVersionNumber { get; set; }
    }
    public class FileSubmissionRecordsModel
    {
        public Int32 FileSubmissionRecordNumber { get; set; }
        public Int32 FileSubmissionId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Filled { get; set; }
        public string PositionNumber { get; set; }
        public string SupervisorPositionNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public string PicturePath { get; set; }
        public string SupervisorFlag { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string NationalOrigin { get; set; }
        public string Age { get; set; }
        public string PositionTitle { get; set; }
        public string ProgramOfficeName { get; set; }
        public string OPSPosition { get; set; }
        public string Salary { get; set; }
        public string EEOCode { get; set; }
        public string LastPerformanceRating { get; set; }
        public string PositionDateOfHire { get; set; }
        public string AgencyDateOfHire { get; set; }
        public string StateCumulativeMonthsOfService { get; set; }
        public string UserID { get; set; }
        public string WorkAddress { get; set; }
        public string WorkCity { get; set; }
        public string WorkStateName { get; set; }
        public string WorkZipCode { get; set; }
        public string WorkCounty { get; set; }
        //[Display(Name = "File Version Number")]
        //public int VersionNumber { get; set; }
        //[Display(Name = "Upload Date Time")]
        //public string UploadDateTime { get; set; }
    }

    public class ValidateEmployeeRecords
    {
        [ScaffoldColumn(false)]
        public Int32 FileSubmissionRecordNumber { get; set; }
        [ScaffoldColumn(false)]
        public Int32 FileSubmissionId { get; set; }
        [Required]
        [Display(Name = "First Name", Order = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name", Order = 2)]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name", Order = 3)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Position Number", Order = 4)]
        public string PositionNumber { get; set; }
        [Required]
        [Display(Name = "Supervisor Position Number", Order = 5)]
        public string SupervisorPositionNumber { get; set; }
        [Display(Name = "PhoneNumber", Order = 6)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email", Order = 7)]
        public string Email { get; set; }
        [Display(Name = "Picture Path", Order = 8)]
        public string PicturePath { get; set; }
        [Required]
        [Display(Name = "Gender", Order = 9)]
        [UIHint("GenderList")]
        public int GenderId { get; set; }
        [Required]
        [UIHint("RaceList")]
        [Display(Name = "Race", Order = 10)]
        public int RaceId { get; set; }
        [Required]
        [UIHint("Date")]
        [Display(Name = "Date Of Birth", Order = 11)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Display(Name = "Position Title", Order = 12)]
        public string PositionTitle { get; set; }
        [Required]
        [Display(Name = "Program Office Name", Order = 13)]
        public string ProgramOfficeName { get; set; }
        [Required]
        [Display(Name = "Salary", Order = 14)]
        public Decimal Salary { get; set; }
        [Required]
        [Display(Name = "EEO Code", Order = 15)]
        [UIHint("EEOCodeList")]
        public int EEOCodeId { get; set; }
        [Display(Name = "Last Performance Rating", Order = 16)]
        public int LastPerformanceRating { get; set; }
        [Display(Name = "State Date Of Hire", Order = 17)]
        [UIHint("Date")]
        public DateTime StateDateOfHire { get; set; }
        [Required]
        [Display(Name = "Agency Date Of Hire", Order = 18)]
        [UIHint("Date")]
        public DateTime AgencyDateOfHire { get; set; }
        [Required]
        [Display(Name = "Work Address", Order = 19)]
        public string WorkAddress { get; set; }
        [Required]
        [Display(Name = "Work City", Order = 20)]
        public string WorkCity { get; set; }
        [Required]
        [UIHint("EmployeeStateList")]
        [Display(Name = "Work State", Order = 21)]
        public int StateId { get; set; }
        [Required]
        [Display(Name = "Work ZipCode", Order = 22)]
        public string WorkZipCode { get; set; }
        [Required]
        [UIHint("CountyList")]
        [Display(Name = "Work County", Order = 23)]
        public int CountyId { get; set; }
        [Required]
        [Display(Name = "Vacant", Order = 24)]
        public bool Vacant { get; set; }
        [Required]
        [Display(Name = "Supervisor Flag", Order = 25)]
        public bool SupervisorFlag { get; set; }
    }

    public class EmployeeErrorlist
    {
        public String Position { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String ErrorMessage { get; set; }
    }
}
