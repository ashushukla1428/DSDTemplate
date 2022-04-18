using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using EEONow.Utilities;
using System.Configuration;
using EEONow.Context.EntityContext;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Mail;

namespace EEONow.Services
{
    public class TwoFactorAuthenticationService : ITwoFactorAuthenticationService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public TwoFactorAuthenticationService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();

        }
        public async Task<bool> CheckTwoFactorAuthentication(DeviceInfoRequestModel model)
        {
            try
            {
                bool result = false;
                var UserDetails = await _context.Users.Where(e => e.UserId == model.UserId).FirstOrDefaultAsync();
                if (UserDetails != null)
                {
                    var UserRole = UserDetails.UserRole;
                    if (UserRole != null)
                    {
                        var _Org = UserRole.Organization;
                        if (_Org != null)
                        {
                            if (_Org.EnableTwoFactorAuthentication)
                            {
                                var Info = GetdeviceInfo(model.UserAgent);
                                Info = Info.Length == 0 ? "Computer" : Info;
                                var IpAddress = GetUser_IP();
                                var MacAdresss = GetMACAddress();
                                var existLoginDeviceInfo = UserDetails.LoginDeviceInfoes
                                                              .Where(e => e.DeviceAccessInfo == Info
                                                              && e.IpAddress == IpAddress
                                                              && e.MacAdresss == MacAdresss
                                                              ).FirstOrDefault();
                                if (existLoginDeviceInfo != null)
                                {
                                    if (!existLoginDeviceInfo.IsVerified)
                                    {
                                        result = true;
                                    }
                                    else if (DateTime.Now.Date.Subtract(existLoginDeviceInfo.LoginDateTime.Date).Days > Convert.ToInt32(ConfigurationManager.AppSettings["DaysToReactivateTwoFactorAuthenticationValidation"]))
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CheckTwoFactorAuthentication", "TwoFactorAuthenticationService.cs");
                throw;
            }
        }
        private static String GetUser_IP()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]  ;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }
        private static String GetdeviceInfo(string userAgent)
        {
            Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string device_info = string.Empty;
            if (OS.IsMatch(userAgent))
            {
                device_info = OS.Match(userAgent).Groups[0].Value;
            }
            if (device.IsMatch(userAgent.Substring(0, 4)))
            {
                device_info += device.Match(userAgent).Groups[0].Value;
            }
            return device_info;
        }
        private string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
        public async Task<string> StoreDeviceInformation(DeviceInfoRequestModel model)
        {
            try
            {
                var user = await _context.Users.Where(x => x.UserId == model.UserId).FirstOrDefaultAsync();

                if (user != null)
                {


                    string Info = GetdeviceInfo(model.UserAgent);
                    Info = Info.Length == 0 ? "Computer" : Info;
                    var IpAddress = GetUser_IP();
                    var MacAdresss = GetMACAddress();
                    var existLoginDeviceInfo = user.LoginDeviceInfoes
                                                  .Where(e => e.DeviceAccessInfo == Info
                                                  && e.IpAddress == IpAddress
                                                  && e.MacAdresss == MacAdresss
                                                  ).FirstOrDefault();
                    string RandomKey = Guid.NewGuid().ToString();
                    if (existLoginDeviceInfo != null)
                    {
                        existLoginDeviceInfo.IsVerified = false;
                        existLoginDeviceInfo.UpdateDateTime = DateTime.Now;
                        existLoginDeviceInfo.RandomKey = RandomKey;
                    }
                    else
                    {
                        var InsertLoginDeviceInfo = new LoginDeviceInfo
                        {

                            Organization = await _context.Organizations.Where(x => x.OrganizationId == model.OrganizationId).FirstOrDefaultAsync(),
                            User = await _context.Users.Where(x => x.UserId == model.UserId).FirstOrDefaultAsync(),
                            DeviceAccessInfo = Info,
                            IsMobile = Info == "Computer" ? false : true,
                            RandomKey = RandomKey,
                            IpAddress = IpAddress,
                            IsVerified = false,
                            LoginDateTime = DateTime.Now,
                            MacAdresss = MacAdresss,
                            NoOfTimeLogin = 0,
                            CreateUserId = Convert.ToInt32(model.UserId),
                            UpdateUserId = Convert.ToInt32(model.UserId),
                            CreateDateTime = DateTime.Now,
                            UpdateDateTime = DateTime.Now
                        };
                        _context.LoginDeviceInfoes.Add(InsertLoginDeviceInfo);
                    }
                    await _context.SaveChangesAsync();
                    return RandomKey;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {

                AppUtility.LogMessage(ex, "StoreDeviceInformation", "TwoFactorAuthenticationService.cs");
                throw;
            }

        }
        public async Task<bool> SendAuthenticationCode(string Key)
        {
            bool result = await GenerateAuthenticationCode(Key);
            return result;
        }
        public async Task<bool> reSendAuthenticationCode(string Key)
        {
            bool result = await GenerateAuthenticationCode(Key);
            return result;
        }
        private async Task<bool> GenerateAuthenticationCode(string Key)
        {
            bool result = false;
            var GetLoginDeviceInfo = await _context.LoginDeviceInfoes.Where(e => e.RandomKey == Key).FirstOrDefaultAsync();
            if (GetLoginDeviceInfo != null)
            {
                String AuthCode = RandomString(6);
                int UserId = GetLoginDeviceInfo.User.UserId;
                string UserEmail = GetLoginDeviceInfo.User.Email;
                if (SendEmailToUser(UserEmail, AuthCode))
                {
                    foreach (var AuthenticateLogs in GetLoginDeviceInfo.EmailAuthenticateLogs)
                    {
                        AuthenticateLogs.Active = false;
                        AuthenticateLogs.UpdateDateTime = DateTime.Now;
                        AuthenticateLogs.UpdateUserId = UserId;
                    }


                    var insertEmailAuthenticateLog = new EmailAuthenticateLog
                    {
                        User = await _context.Users.Where(x => x.UserId == UserId).FirstOrDefaultAsync(),
                        LoginDeviceInfo = await _context.LoginDeviceInfoes.Where(e => e.RandomKey == Key).FirstOrDefaultAsync(),
                        AuthenticateCode = AuthCode,
                        Active = true,
                        EmailSentTime = DateTime.Now,
                        CreateUserId = UserId,
                        UpdateUserId = UserId,
                        CreateDateTime = DateTime.Now,
                        UpdateDateTime = DateTime.Now

                    };
                    _context.EmailAuthenticateLogs.Add(insertEmailAuthenticateLog);
                    await _context.SaveChangesAsync();
                    result = true;
                }
            }
            return result;
        }
        private bool SendEmailToUser(String ReceiverEmailAddress, String AuthCode)
        {
            bool result = false;
            try
            {
                String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/TwoFactorAuthenticationTemplate.html").ToString();
                string BodyHTML = String.Format(RawHtml, AuthCode);
                String Subject = ConfigurationManager.AppSettings["TwoFactorAuthenticationSubject"];
                MailMessage mail = new MailMessage();
                mail.To.Add(ReceiverEmailAddress);
                mail.Subject = Subject;
                mail.Body = BodyHTML;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "SendEmailToUser", "TwoFactorAuthenticationService.cs");
                result = true;
            }
            return result;
        }
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<bool> VerifyTwoFactorAuthenticationCode(DeviceAuthenticationModel model)
        {
            try
            {
                var userLoginInfo = await _context.LoginDeviceInfoes.Where(x => x.RandomKey == model.RandomKey).FirstOrDefaultAsync();
                 
                if (userLoginInfo == null)
                {
                    return false;
                }
                
                var GetUserCode = userLoginInfo.EmailAuthenticateLogs.Where(e => e.AuthenticateCode == model.Code && e.Active == true).FirstOrDefault();
                if (GetUserCode == null)
                {
                    return false;
                }
                if(model.RemoveDevicesInfo)
                {
                    var userId = userLoginInfo.User.UserId;
                    var userOtherLoginInfo = await _context.LoginDeviceInfoes.Where(x => x.User.UserId == userId && x.RandomKey != model.RandomKey).ToListAsync();
                    foreach (var OtherLoginInfo in userOtherLoginInfo)
                    {
                        OtherLoginInfo.IsVerified = false;
                    }
                }
                userLoginInfo.IsVerified = true;
                userLoginInfo.NoOfTimeLogin = userLoginInfo.NoOfTimeLogin + 1;
                userLoginInfo.LastLoginTime = userLoginInfo.LoginDateTime;
                userLoginInfo.LoginDateTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "VerifyTwoFactorAuthenticationCode", "TwoFactorAuthenticationService.cs");
                throw;
            }

        }
        public async Task<LoginResponse> GetUserDetail(string Key)
        {
            try
            {
                var userLoginInfo = await _context.LoginDeviceInfoes.Where(x => x.RandomKey == Key).FirstOrDefaultAsync();
                if (userLoginInfo == null)
                {
                    return new LoginResponse { Message = "Invalid user" };
                }
                var user = userLoginInfo.User;
                user.LastLoginDateTime = DateTime.Now;
                await _context.SaveChangesAsync();
                string _UserName = user.LastName + ", " + user.FirstName + " " + user.MiddleName;
                if (user.UserRole == null)
                {
                    return new LoginResponse { Message = "Success", Succeeded = true, Email = user.Email, UserId = user.UserId, Roles = "DefinedSoftwareAdministrator", OrgId = 0, UserRoleId = 0, IsAdd = true, IsEdit = true, IsFilter = true, UserName = _UserName };
                }
                else
                {
                    return new LoginResponse { Message = "Success", Succeeded = true, Email = user.Email, UserId = user.UserId, Roles = user.UserRole.Name, OrgId = user.UserRole.Organization.OrganizationId, IsFilter = user.UserRole.IsFilter, IsEdit = user.UserRole.IsEdit, IsAdd = user.UserRole.IsAdd, UserRoleId = user.UserRole.RoleId, UserName = _UserName };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetUserDetail", "TwoFactorAuthenticationService.cs");
                throw;
            }
        }
    }
}
