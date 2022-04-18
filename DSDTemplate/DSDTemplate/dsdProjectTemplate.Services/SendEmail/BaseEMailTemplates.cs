using System;
using System.IO;

namespace dsdProjectTemplate.Services.SendEmail
{
    public static class BaseEMailTemplates
    {
        public static string GetHeader()
        {
            return File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/base/header.html").ToString();
        }
        public static string GetFooter()
        {
            return File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/base/footer.html").ToString();
        }
    }
}
