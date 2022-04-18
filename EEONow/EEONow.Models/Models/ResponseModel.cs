using System;
using System.ComponentModel.DataAnnotations;

namespace EEONow.Models
{
    public class ResponseModel:BaseResponseModel
    {
        public int Id { get; set; }
    }

    public class BaseResponseModel
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
    public class verifyUserModel : BaseResponseModel
    {
        public bool Terms { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
    }
    public class DeviceInfoRequestModel
    {
        public long UserId { get; set; }
        public int OrganizationId { get; set; }
        public string UserAgent { get; set; }
        public string RemoteIpAddress { get; set; }
    }
    public class DeviceAuthenticationModel  
    {
        public String RandomKey { get; set; }
        [Required]
        public String Code { get; set; } 
        public bool RemoveDevicesInfo { get; set; }
    }

}
