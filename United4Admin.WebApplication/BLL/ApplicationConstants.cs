namespace United4Admin.WebApplication.BLL
{
    public class ApplicationConstants
    {
        public const string REFERENCEBEGINSTRING = "E";
        public const string REFERENCEFORMAT = "D9";
        public const string DEFAULTCHILDNO = "-1";
        public const string REQUESTTYPE = "SP";
        public const string CONSENTSTATEMENTID = "10127"; // in product table chosen consent id 
        public const string SUPPORTERTYPE = "Individual";
        public const string ADDRESSSTRUCTURE = "Street";
        public const string PAYMENTMETHOD = "DIRECTDEBIT";
        public const string PAYMENTTYPE = "RECURRING";
        public const string PAYMENTFREQUENCY = "MONTHLY";
        public const string PREFERREDDDDATE = "1";
        public const string RECEIPTREQUIRED = "Electronic";
        public const string COMMITMENTCODE = "S";
        public const string AINAME1 = "Chosen Media Consent";
        public const string AINAME2 = "Chosen Field Event Code";
        public const string PHOTOFILENAMEPREFIX = "SponsorAccount_photo";
        public const int WORKFLOWSTATUSDRAFT = 1;
        public const string CURRENTUSER = "CurrentUser";
        public const int IMAGESTATUSAPPROVED = 1;
        public const int IMAGESTATUSDELTED = 2;
        public const int IMAGESTATUSNOTAPPROVED = 3;
    }
    public static class ConstantMessages
    {
        public static string Load = "{event} Loaded Successfully .";
        public static string Create = "{event} created Successfully";
        public static string Update = "{event} updated Successfully.";
        public static string Delete = "{event} deleted Successfully .";
        public static string Duplicate = "{event} details Already Exists.";
        public static string Error = "Error Occurred in While processing your requst.";
        public static string SignUpExists = "{event} already exists in Singup.";
        public static string LoadFailure = "Failed to retrieve the {event}-{0}.";
        public static string CreateFailure = "Failed to create {event} -{0}";
        public static string UpdateFailure = "Failed to update {event}-{0}";
        public static string DeleteFailure = "Failed to delete {event}-{0}";
        public static string ExistsFailure = "{event} not exists -{0}";
        public static string NoRecordsFound = "No records found in {event}";
        public static string SelfDelete = "You cannot delete yourself";
        public static string SignUpNotExists = "{event} not exists in SignUp";
        public static string UploadChildIds = "Child Ids Uploaded Successfully. ";
        public static string UploadChildIdsFailure = "Failed to Upload Child Ids. ";
        public static string ChildSelectedSponsored = "{childid} has been selected or sponsored.";
    }
}