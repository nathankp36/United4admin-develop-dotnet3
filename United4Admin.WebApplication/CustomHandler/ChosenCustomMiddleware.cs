using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.CustomHandler
{
    public class ChosenCustomMiddleware
    {
     private readonly RequestDelegate _next;

        public ChosenCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                PermissionsVM pVM = GetPermissions(httpContext.User.Identity.Name);
                if (pVM != null)
                {
                    httpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("Admin", pVM.Administrator.ToString()));
                    httpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("CreateEditDeleteEvents", pVM.CreateEditDeleteEvents.ToString()));
                    httpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("EditDeleteSupporterData", pVM.EditDeleteSupporterData.ToString()));
                    httpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("Download", pVM.DownloadFilesandImages.ToString()));
                }
               
            }
            await _next(httpContext);
        }

        private PermissionsVM GetPermissions(string userEmail)
        {
            try
            {
                var permissionList = Task.Run(async () =>
                {
                    var result = await new PermissionFactory().LoadList();
                    return result;
                });
                PermissionsVM pVM = permissionList.Result.Where(x => x.WVEmail == userEmail.ToLower()).FirstOrDefault();
                return pVM;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
