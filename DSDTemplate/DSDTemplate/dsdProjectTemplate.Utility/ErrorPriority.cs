namespace dsdProjectTemplate.Utility
{
    public enum ErrorPriority
    {
        Low,   // Not used for now.
        Medium,// used for lookup screens like UserTypes, Gender, States, etc..
        High,  // used for screens like organizations, organization years, etc..
        Severe // used for screens like User Registration, Providers, Clients, Forms, etc..
    }
}
