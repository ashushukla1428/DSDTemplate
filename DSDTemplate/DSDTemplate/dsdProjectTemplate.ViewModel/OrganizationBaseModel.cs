

using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel
{
    /// <summary>
    /// Id, OrganizationId and IsActive 
    /// </summary>
    public class OrganizationBaseModel: BaseModel
    {
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }
    }
}
