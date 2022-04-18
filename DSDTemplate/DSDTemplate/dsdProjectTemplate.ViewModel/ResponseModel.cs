using System;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel
{
    public class BaseResponse
    {
        public bool Status { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
    public class ResponseModel : BaseResponse
    {
        public long Id { get; set; }
    }
    public class VerifyUserModel : BaseResponse
    {
        public bool Terms { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
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
