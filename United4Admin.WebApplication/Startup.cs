using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.CustomHandler;
using United4Admin.WebApplication.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace United4Admin.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppConfigValues.ApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value;
            //AppConfigValues.DataCacheApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("DataCacheApiBaseUrl").Value;
            AppConfigValues.ApiToken = Configuration.GetSection("ApiConfig").GetSection("ApiToken").Value;
            AppConfigValues.ApiVersion = Configuration.GetSection("ApiConfig").GetSection("ApiVersion").Value;
            AppConfigValues.LogStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("LogStorageContainer").Value;
            AppConfigValues.StorageAccountKey = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountKey").Value;
            AppConfigValues.StorageAccountName = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountName").Value;
            AppConfigValues.CRMType = Configuration.GetSection("CRMExtractData").GetSection("CRMType").Value;
            AppConfigValues.XSLTStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("XSLTStorageContainer").Value;
            //AppConfigValues.BaseApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("BaseApiBaseUrl").Value;
            //AppConfigValues.BaseAdyenApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("BaseAdyenApiBaseUrl").Value;
            AppConfigValues.HostedCountry = Configuration.GetSection("CRMExtractData").GetSection("HostedCountry").Value;
            AppConfigValues.CRMQuarantineDisplay = Configuration.GetSection("CRMExtractData").GetSection("crmQuarantineDisplay").Value;
            AppConfigValues.CRMQuarantinePath = Configuration.GetSection("CRMExtractData").GetSection("crmQuarantinePath").Value;
            AppConfigValues.CRMQuarantineTitle = Configuration.GetSection("CRMExtractData").GetSection("crmQuarantineTitle").Value;
            AppConfigValues.CRMQuarantineNavTitle = Configuration.GetSection("CRMExtractData").GetSection("crmQuarantineNavTitle").Value;
            AppConfigValues.CRMQuarantineNoRecordDescription = Configuration.GetSection("CRMExtractData").GetSection("crmQuarantineNoRecordDescription").Value;
            AppConfigValues.CRMQuarantineErrorDownload = Configuration.GetSection("CRMExtractData").GetSection("crmQuarantineErrorDownload").Value;


            AppConfigValues.crmNavigationTitle = Configuration.GetSection("CRMMapping").GetSection("crmNavigationTitle").Value;
            AppConfigValues.crmDisplayNavigation = Configuration.GetSection("CRMMapping").GetSection("crmDisplayNavigation").Value;
            AppConfigValues.crmMappingPath = Configuration.GetSection("CRMMapping").GetSection("crmMappingPath").Value;
        }

        public IConfiguration Configuration { get; }
        public static string userEmail;
        public static PermissionsVM permissionsVM;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Azure AD Authentication
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
               .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            //Adding Not Authorised link into Authentication
            services.Configure<CookieAuthenticationOptions>(AzureADDefaults.CookieScheme, options => options.AccessDeniedPath = "/Home/NotAuthorised");

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            //Chosen Authorization policy
            services.AddAuthorization(config =>
            {

                config.AddPolicy("CreateEditDeleteEvents", policy =>
                {
                    policy.AddRequirements(new CreateEditDeleteEventsClaimsRequirement());
                });
                config.AddPolicy("Admin", policy =>
                {
                    policy.AddRequirements(new ChosenAdminClaimsRequirement());
                });
                config.AddPolicy("EditDeleteSupporterData", policy =>
                {
                    policy.AddRequirements(new EditDeleteSupporterDataClaimsRequirement());
                });
                config.AddPolicy("Download", policy =>
                {
                    policy.AddRequirements(new DownloadRequirement());
                });

            });

            services.AddRazorPages();
            services.AddMvc();

            // Use same instance within a scope and create new instance for different http request and out of scope.
            services.AddScoped<IChoosingEventFactory, ChoosingEventFactory>();
            services.AddScoped<IRevealEventFactory, RevealEventFactory>();
            services.AddScoped<ISignupEventFactory, SignupEventFactory>();
            services.AddScoped<IRegistrationFactory, RegistrationFactory>();
            services.AddScoped<IPermissionFactory, PermissionFactory>();
            services.AddScoped<IZipFileHelper, ZipFileHelper>();
            services.AddScoped<IImageStoreFactory, ImageStoreFactory>();
            services.AddScoped<IChildDataCacheFactory, ChildDataCacheFactory>();
            services.AddScoped<IChildDataCacheForWebFactory, ChildDataCacheForWebFactory>();
            services.AddScoped<ICRMExtractFactory, CRMExtractFactory>();
            services.AddScoped<ICRMQuarantineFactory, CRMQuarantineFactory>();
            // services.AddScoped<IAdyenPaymentsFactory, AdyenPaymentsFactory>();
            services.AddScoped<IFeatureSwitchFactory, FeatureSwitchFactory>();

            //Chosen Authorization handler
            // Use same instance within a scope and create new instance for different http request and out of scope.
            services.AddScoped<IAuthorizationHandler, ChosenAdminClaimsHandler>();
            services.AddScoped<IAuthorizationHandler, CreateEditDeleteEventsClaimsHandler>();
            services.AddScoped<IAuthorizationHandler, EditDeleteSupporterDataClaimsHandler>();
            services.AddScoped<IAuthorizationHandler, DownloadClaimsHandler>();
            services.AddScoped<ISalutationFactory, SalutationFactory>();
            services.AddScoped<IProductVariantFactory, ProductVariantFactory>();
            services.AddScoped<IPaymentMethodFactory, PaymentMethodFactory>();

            services.AddScoped<IFallbackValuesFactory, FallbackValuesFactory>();
            services.AddScoped<IMotivationFactory, MotivationFactory>();
            services.AddScoped<IPledgeDesignationFactory, PledgeDesignationFactory>();
            services.AddScoped<IAddOnDonationFactory, AddOnDonationFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {


            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appSettings.json")
                               .Build();

            Serilog.Formatting.Json.JsonFormatter jsonFormatter = new Serilog.Formatting.Json.JsonFormatter();
            string connectionString = GetAzureConnectionString(AppConfigValues.StorageAccountName, AppConfigValues.StorageAccountKey);

            Log.Logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(configuration)
                       .WriteTo.AzureBlobStorage(jsonFormatter, connectionString, Serilog.Events.LogEventLevel.Information, AppConfigValues.LogStorageContainer, "{yyyy}/{MM}/United4AdminWeb/log-{dd}.json", false, TimeSpan.FromDays(1))
                       .CreateLogger();
            // logging
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "MM/dd/yyyy hh:mm:ss tt"
            };
            culture.DateTimeFormat = dateformat;

            var supportedCultures = new[]
            {
                culture
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<ChosenCustomMiddleware>();

            app.Use(async (context, next) =>
            {
                if (context.User != null && context.User.Identity.IsAuthenticated)
                {
                    userEmail = context.User.Identity.Name;
                }
                await next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }

        private static string GetAzureConnectionString(string accountName, string accountKey)
        {
            string azureConnection = "DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accountKey + ";EndpointSuffix=core.windows.net";
            return azureConnection;
        }
    }
}
