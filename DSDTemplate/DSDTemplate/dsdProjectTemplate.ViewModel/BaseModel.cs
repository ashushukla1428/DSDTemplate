using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel
{
    public class BaseModel
    {
        [ScaffoldColumn(false)]
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
