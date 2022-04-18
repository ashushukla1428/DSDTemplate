using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class CompanyUserModel
    {
        #region For user
        public long UserID { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(99)]
        public string UserEmail { get; set; }

        [StringLength(49)]
        public string FirstName { get; set; }   

        [StringLength(49)]
        public string LastName { get; set; }

        [Required]
        [StringLength(99)]
        [DataType(DataType.Password)]
        public string Password { get; set; }       
        
        [Required]
        [StringLength(99)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        #endregion

        #region For company
        public int CompanyID { get; set; }

        [Required]
        [StringLength(99)]
        public string CompanyName { get; set; }

        [StringLength(499)]
        public string Address1 { get; set; }

        [StringLength(499)]
        public string Address2 { get; set; }
       
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(49)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]       
        [StringLength(49)]
        public string EmailAddress { get; set; }

        [Url]
        [DataType(DataType.Url)]
        [StringLength(99)]
        public string Website { get; set; }

        public string LogoPath { get; set; }//Used for edit case, if company have any logo





        public HttpPostedFileBase Logo { get; set; }



        public string LogoStripPath { get; set; }//Used for edit case, if company have any logo

        public HttpPostedFileBase LogoStrip { get; set; }




        public bool IsActive { get; set; }

        public int RoleID { get; set; }
        #endregion


        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }

        // new columns
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(4)]
        public string UserLimit { get; set; }


        [Required]
        public string PackageStartDate { get; set; }
        public string PackageRenewalDate { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(4)]
        public string Payment { get; set; }
        public string PackagePeriod { get; set; }

        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }


        public string ZipCode { get; set; }

    }


    public class CompanyUserModel_Old
    {
        #region For user
        public long UserID { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(99)]
        public string UserEmail { get; set; }

        [StringLength(49)]
        public string FirstName { get; set; }

        [StringLength(49)]
        public string LastName { get; set; }

        [Required]
        [StringLength(99)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(99)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        #endregion

        #region For company
        public int CompanyID { get; set; }

        [Required]
        [StringLength(99)]
        public string CompanyName { get; set; }

        [StringLength(499)]
        public string Address1 { get; set; }

        [StringLength(499)]
        public string Address2 { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(49)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(49)]
        public string EmailAddress { get; set; }

        [Url]
        [DataType(DataType.Url)]
        [StringLength(99)]
        public string Website { get; set; }

        public string LogoPath { get; set; }//Used for edit case, if company have any logo

        public HttpPostedFileBase Logo { get; set; }


        public string LogoStripPath { get; set; }//Used for edit case, if company have any logo

        public HttpPostedFileBase LogoStrip { get; set; }



        public bool IsActive { get; set; }

        public int RoleID { get; set; }
        #endregion


        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }



        public string ZipCode { get; set; }
    }

    public class CompanyEmployeeModel
    {

        #region For user
        public long UserID { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(99)]
        public string UserEmail { get; set; }

        [StringLength(49)]
        public string FirstName { get; set; }

        [StringLength(49)]
        public string LastName { get; set; }

        [Required]
        [StringLength(99)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(99)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        #endregion

        #region For Employee
        public int Id { get; set; }

        [Required]
        [StringLength(99)]
        public string employeeFirstName { get; set; }

        [Required]
        [StringLength(99)]
        public string employeeLastName { get; set; }

        [StringLength(499)]
        public string employeeAddress1 { get; set; }

        [StringLength(499)]
        public string employeeAddress2 { get; set; }


        public string ZipCode { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(49)]
        public string employeePhoneNumber { get; set; }

        //[Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(49)]

        public string employeeEmailAddress { get; set; }

        //[Url]
        //[DataType(DataType.Url)]
        //[StringLength(99)]
        //public string Website { get; set; }

        public string ImagePath { get; set; }//Used for edit case, if company have any logo

        public HttpPostedFileBase Image { get; set; }
        // public bool IsActive { get; set; }
        public bool IsActive { get; set; }
        public int employeeRoleID { get; set; }




        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }


        public string Role { get; set; }


        //public string employeePhoneNumber { get; set; }

        #endregion
    }
}