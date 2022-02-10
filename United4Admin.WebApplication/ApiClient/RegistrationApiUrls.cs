using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.APIClient
{
    public static class RegistrationK8ApiUrls
    {
        public const string registrationApiServicePrefix = "k8seventandregistration/api/Registeration/";
        public const string LoadList = registrationApiServicePrefix + "GetSignupList";
        public const string Load = registrationApiServicePrefix + "GetSignupListById/{id}";
        public const string Create = registrationApiServicePrefix + "CreateSignup";
        public const string Update = registrationApiServicePrefix + "UpdateSignup";
        public const string Delete = registrationApiServicePrefix + "DeleteSignup/{id}";
        public const string GetTitles = registrationApiServicePrefix + "GetTitles";
        public const string GetStatuses = registrationApiServicePrefix + "GetStatuses";
        public const string GetFieldDataExport = registrationApiServicePrefix + "GetFieldDataExport";
        public const string GetEchoData = registrationApiServicePrefix + "GetEchoData";
        public const string GetPhotoApporval = registrationApiServicePrefix + "GetPhotoApporval";
        public const string CreateImage = registrationApiServicePrefix + "CreateImage";
        public const string UpdateImage = registrationApiServicePrefix + "UpdateImage";
        public const string DeleteImage = registrationApiServicePrefix + "DeleteImage";
        public const string GetImage = registrationApiServicePrefix + "GetImage/{id}";
        public const string GetImageList = registrationApiServicePrefix + "GetImageList";
        public const string GetImageNotUploadData = registrationApiServicePrefix + "GetImageNotUploadData";
    }
}