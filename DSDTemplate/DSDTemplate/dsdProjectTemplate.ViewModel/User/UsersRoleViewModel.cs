using System;
using System.ComponentModel.DataAnnotations;


namespace dsdProjectTemplate.ViewModel.User
{
    public class UsersRoleViewModel : OrganizationBaseModel
    {
        [Required(ErrorMessage = "Role name is required field")]
        [MaxLength(50)]
        public string RoleName { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
    }
}
