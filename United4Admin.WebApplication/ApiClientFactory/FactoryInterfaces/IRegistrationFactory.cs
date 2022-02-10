using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using Microsoft.AspNetCore.Http;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IRegistrationFactory : IApiClientFactory<SignUpVM>
    {
        Task<List<TitleVM>> GetTitles();
        Task<List<StatusVM>> GetStatuses();
        Task<List<ImageInfoVM>> GetFieldDataExport(SignUpExtractParameterVM signUpVM);
        Task<List<SignUpVM>> GetEchoData(SignUpExtractParameterVM signUpVM);
        Task<List<SignUpVM>> GetPreRevealExtractData(CRMExtractParameterModelVM cRMExtractParameterModel);
        Task<List<SignUpVM>> GetPhotoApproval();
        Task<ApiResponse> CreateImage(ImageInfoVM imageInfo);
        Task<ApiResponse> UpdateImage(ImageInfoVM imageInfo);
        Task<ApiResponse> DeleteImage(int SignUpId);
        Task<ImageInfoVM> GetImage(int SignUpId);
        Task<List<ImageInfoVM>> GetImageNames(SignUpExtractParameterVM signUpVM);

        // Event Microservice methods
        Task<List<ChoosingPartyVM>> GetChoosingPartyList();
        Task<List<RevealEventVM>> GetRevealEventList();
        Task<List<SignUpEventVM>> GetSignupEventList();
        Task<List<SignUpVM>> GetImageNotUploadData();
        Task<ApiResponse> UploadImage(IFormFile imageToUpload, string filenameFromId);
    }
}
