using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Interfaces
{
    public interface ITwoFactorAuthenticationService
    {
        Task<bool> CheckTwoFactorAuthentication(DeviceInfoRequestModel model);
        Task<string> StoreDeviceInformation(DeviceInfoRequestModel model);
        Task<bool> SendAuthenticationCode(string Key);
        Task<bool> reSendAuthenticationCode(string Key);

        Task<bool> VerifyTwoFactorAuthenticationCode(DeviceAuthenticationModel model);
        Task<LoginResponse> GetUserDetail(string Key);
    }
}
