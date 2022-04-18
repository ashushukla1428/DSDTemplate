namespace dsdProjectTemplate.ViewModel
{
    public class ResponseMessages
    {
        public static string NotAuthorized = "you are not authorized to do this action, please contact to your organization.";
        public static string CREATED_SUCCESSFULLY = "%s has been created successfully.";
        public static string UPDATEDED_SUCCESSFULLY = "%s has been updated successfully.";
        public static string DELETE_SUCCESSFULLY = "%s deleted successfully.";
        public static string NOT_FOUND = "%s not found";
        public static string Already_Exists = "%s is already exists.";
        public static string WRONG_USER = "You have entered an invalid username or password";
        public static string ACCOUNT_DEACTIVATED = "Account deactivated please contact with support team";
        public static string LOGIN_SUCCESSFULLY = "You are successfully logged in";
        public static string requiredFields="required fields are missing";
        public static string System_Error = "something went wrong. please try again or contact support.";
        public static string Failure_To_Update = "Failure to update or create new record";
        public static string ProvideMobile = "Please add your mobile number from my profile";
        public static string ProvideEmail = "Please add your email address from my profile";

        public static string SubjectCreatedSuccess(string data)
        {
            return CREATED_SUCCESSFULLY.Replace("%s", data);
        }
        public static string SubjectUpdatedSuccess(string data)
        {
            return UPDATEDED_SUCCESSFULLY.Replace("%s", data);
        }
        public static string SubjectDeletedSuccess(string data)
        {
            return DELETE_SUCCESSFULLY.Replace("%s", data);
        }
        public static string SubjectNotFound(string data)
        {
            return NOT_FOUND.Replace("%s", data);
        }
        public static string SubjectAlreadyExists(string data)
        {
            return Already_Exists.Replace("%s", data);
        }
    }
}
