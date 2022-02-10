﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.CustomHandler
{   
    public class DownloadClaimsHandler : AuthorizationHandler<DownloadRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DownloadRequirement requirement)
        {
            var authorizeFilter = context.Resource as AuthorizationFilterContext;
            string loggedUser = context.User.Identity.Name;
            //if (authorizeFilter == null)
            //{
            //    return Task.CompletedTask;
            //}
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            else if (context.User.HasClaim(claim => claim.Type == "Download" && claim.Value == "True"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
