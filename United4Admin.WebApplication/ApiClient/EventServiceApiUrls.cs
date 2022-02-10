using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class EventServiceApiUrls
    {
        public const string eventK8ApiServicePrefix = "k8seventandregistration/api/";
        public const string GetWorkflowStatusListK8 = eventK8ApiServicePrefix + "ChoosingEvent/GetWorkflowStatusList";
        public static class ChoosingEventK8ApiUrl
        {
            public const string LoadList = eventK8ApiServicePrefix + "ChoosingEvent/GetChoosingEventList";
            public const string Load = eventK8ApiServicePrefix + "ChoosingEvent/GetChoosingEventListById/{id}";
            public const string Create = eventK8ApiServicePrefix + "ChoosingEvent/CreateChoosingEvent";
            public const string Update = eventK8ApiServicePrefix + "ChoosingEvent/UpdateChoosingEvent";
            public const string Delete = eventK8ApiServicePrefix + "ChoosingEvent/DeleteChoosingEvent/{id}";
            public const string SignUpExists = eventK8ApiServicePrefix + "ChoosingEvent/CheckChoosingEventSignUpExists/{id}";
        }

        public static class SignUpEventK8ApiUrl
        {
            public const string LoadList = eventK8ApiServicePrefix + "SignupEvent/GetSignupEventList";
            public const string Load = eventK8ApiServicePrefix + "SignupEvent/GetSignupEventListById/{id}";
            public const string Create = eventK8ApiServicePrefix + "SignupEvent/CreateSignupEvent";
            public const string Update = eventK8ApiServicePrefix + "SignupEvent/UpdateSignupEvent";
            public const string Delete = eventK8ApiServicePrefix + "SignupEvent/DeleteSignupEvent/{id}";
            public const string SignUpExists = eventK8ApiServicePrefix + "SignupEvent/CheckChoosingEventSignUpExists/{id}";
        }

        public static class RevealEventK8ApiUrl
        {
            public const string LoadList = eventK8ApiServicePrefix + "RevealEvent/GetRevealEventList";
            public const string Load = eventK8ApiServicePrefix + "RevealEvent/GetRevealEventListById/{id}";
            public const string Create = eventK8ApiServicePrefix + "RevealEvent/CreateRevealEvent";
            public const string Update = eventK8ApiServicePrefix + "RevealEvent/UpdateRevealEvent";
            public const string Delete = eventK8ApiServicePrefix + "RevealEvent/DeleteRevealEvent/{id}";
            public const string SignUpExists = eventK8ApiServicePrefix + "RevealEvent/CheckRevealEventSignUpExists/{id}";
        }

        public static class PermissionK8ApiUrl
        {
            public const string LoadList = eventK8ApiServicePrefix + "Permission/GetPermissionList";
            public const string Load = eventK8ApiServicePrefix + "Permission/GetPermissionListById/{id}";
            public const string Create = eventK8ApiServicePrefix + "Permission/CreatePermission";
            public const string Update = eventK8ApiServicePrefix + "Permission/UpdatePermission";
            public const string Delete = eventK8ApiServicePrefix + "Permission/DeletePermission/{id}";
            public const string Adminstrators = eventK8ApiServicePrefix + "Permission/GetAdministrators";
        }
    }
}
