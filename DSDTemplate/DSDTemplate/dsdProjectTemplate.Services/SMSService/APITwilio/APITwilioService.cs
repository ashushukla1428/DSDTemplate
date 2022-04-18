using dsdProjectTemplate.Utility;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace dsdProjectTemplate.Services.SMSService.APITwilio
{
   
    public class APITwilioService
    { 
        public async Task<MessageResource> SendSMSByNumber(string toMobileNumber, string smsText)
        {
            try
            {
                TwilioClient.Init(System.Configuration.ConfigurationManager.AppSettings["accountSid"], System.Configuration.ConfigurationManager.AppSettings["authToken"]);
                return await MessageResource.CreateAsync(
                   body: smsText,
                   from: new Twilio.Types.PhoneNumber(System.Configuration.ConfigurationManager.AppSettings["twilioFromNumber"]),
                   //from: new Twilio.Types.PhoneNumber("+15005550006"), 
                   to: new Twilio.Types.PhoneNumber(toMobileNumber)
                  );

            }
            catch(Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->SendSMSByNumber", ex);
                throw;
            }
        }
    }
}
