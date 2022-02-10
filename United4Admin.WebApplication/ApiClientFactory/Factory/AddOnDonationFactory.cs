using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using United4Admin.WebApplication.Models;
using System.Linq;
using Microsoft.Azure.Storage.Blob;
using United4Admin.WebApplication.BLL;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class AddOnDonationFactory : IAddOnDonationFactory
    {

        protected ApiClient apiClient;
        private readonly IImageStoreFactory imageStoreFactory;
        private readonly IZipFileHelper _zipFileHelper;

        public AddOnDonationFactory(IImageStoreFactory imageStoreFactory, IZipFileHelper zipFileHelper)
        {
            this.apiClient = new ApiClient();
            this.imageStoreFactory = imageStoreFactory;
            _zipFileHelper = zipFileHelper;
        }

        public AddOnDonationFactory(ApiClient _apiClient, IImageStoreFactory imageStoreFactory, IZipFileHelper zipFileHelper)
        {
            this.apiClient = _apiClient;
            this.imageStoreFactory = imageStoreFactory;
            _zipFileHelper = zipFileHelper;
        }

        public async Task<List<AddOnDonationVM>> GetAddOnDonationData()
        {
            try
            {
                BlobStorageVM blobStorageVM = await imageStoreFactory.GetBobStorageDetails();
                CloudBlobContainer cloudBlobContainer = await _zipFileHelper.GetBlobImageContainer(blobStorageVM);

                List<AddOnDonationVM> addOnDonationVMList = new List<AddOnDonationVM>();
                addOnDonationVMList = await GetAddOnDonationList();

                var addonDonations = (from addon in addOnDonationVMList
                                      select new AddOnDonationVM
                                      {
                                          Forename = addon.Forename,
                                          Surname = addon.Surname,
                                          ChildID = addon.ChildID,
                                          OrderGuid = addon.OrderGuid,
                                          Exported = addon.Exported,
                                          DateOfRequest = addon.DateOfRequest
                                      }).ToList();

                //foreach (var item in addonDonations)
                //{
                //    CloudBlockBlob messageBlob = cloudBlobContainer.GetBlockBlobReference(item.ChildID + "-" + item.OrderGuid.ToString() + ".txt");
                //    item.Message = messageBlob.Exists() ? "Online" : "";
                //    CloudBlockBlob photoBlob = cloudBlobContainer.GetBlockBlobReference(item.ChildID + "-" + item.OrderGuid.ToString() + ".jpeg");
                //    item.PhotoFileName = photoBlob.Exists() ? item.ChildID + "-" + item.OrderGuid.ToString() : "";
                //}
                return addonDonations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //method to load add-on donation list data
        public async Task<List<AddOnDonationVM>> GetAddOnDonationList()
        {
            try
            {
                List<AddOnDonationVM> addOnDonationVM = new List<AddOnDonationVM>();
                var requestUrl = apiClient.CreateRequestUri(AddOnDonationApiUrls.AddOnDonationLoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                addOnDonationVM = JsonConvert.DeserializeObject<List<AddOnDonationVM>>(Convert.ToString(response.ResponseObject));
                return addOnDonationVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AddOnDonationVM> GetAddOnDonationBySalesOrderId(string salesOrderId)
        {
            try
            {
                AddOnDonationVM addOnDonationVM = new AddOnDonationVM();
                ContactViewModel contactVM = new ContactViewModel();
                var requestUrl = apiClient.CreateRequestUri(SupporterTransactionApiUrls.GetSupporterTransaction.Replace("{salesOrderId}", salesOrderId));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                contactVM = JsonConvert.DeserializeObject<ContactViewModel>(Convert.ToString(response.ResponseObject));

                if (contactVM != null)
                {
                    addOnDonationVM.Forename = contactVM.Contact.FirstName;
                    addOnDonationVM.Surname = contactVM.Contact.LastName;
                    addOnDonationVM.ChildID = contactVM.OrderProduct.WvChildId;
                    addOnDonationVM.OrderGuid = Guid.Parse(contactVM.OrderProduct.SalesOrderId);
                    addOnDonationVM.DateOfRequest = contactVM.Order.DateFulfilled ?? DateTime.Now;
                }

                BlobStorageVM blobStorageVM = await imageStoreFactory.GetBobStorageDetails();
                CloudBlobContainer cloudBlobContainer = await _zipFileHelper.GetBlobImageContainer(blobStorageVM);

                CloudBlockBlob messageBlob = cloudBlobContainer.GetBlockBlobReference(addOnDonationVM.ChildID + "-" + addOnDonationVM.OrderGuid.ToString() + ".txt");
                addOnDonationVM.Message = messageBlob.Exists() ? "Online" : "";

                CloudBlockBlob photoBlob = cloudBlobContainer.GetBlockBlobReference(addOnDonationVM.ChildID + "-" + addOnDonationVM.OrderGuid.ToString() + ".jpeg");
                addOnDonationVM.PhotoFileName = photoBlob.Exists() ? addOnDonationVM.ChildID + "-" + addOnDonationVM.OrderGuid.ToString() : "";
                //if (photoBlob.Exists())
                //{
                //    var result = await imageStoreFactory.GetSecureUrlImage(addOnDonationVM.ChildID + "-" + addOnDonationVM.OrderGuid.ToString() + ".jpeg");
                //    addOnDonationVM.PhotoFileName = result.ToString();
                //}

                return addOnDonationVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AddOnDonationVM>> GetFieldDataExport(SignUpExtractParameterVM signUpVM)
        {
            try
            {
                List<AddOnDonationVM> addOnDonationVMList = new List<AddOnDonationVM>();
                addOnDonationVMList = await GetAddOnDonationList();

                BlobStorageVM blobStorageVM = await imageStoreFactory.GetBobStorageDetails();
                CloudBlobContainer cloudBlobContainer = await _zipFileHelper.GetBlobImageContainer(blobStorageVM);

                List<AddOnDonationVM> addOnDonationInfoList = new List<AddOnDonationVM>();

                if (signUpVM.SignExtractType == "newtransaction")
                {
                    addOnDonationVMList = addOnDonationVMList.Where(x => x.Exported == false).ToList();
                }
                else if (signUpVM.SignExtractType == "date")
                {
                    addOnDonationVMList = addOnDonationVMList.Where(x => x.DateOfRequest >= signUpVM.SignStartDate.Value
                                                    && x.DateOfRequest <= signUpVM.SignEndDate.Value.AddDays(1)).ToList();
                }

                if (addOnDonationVMList.Count > 0)
                {
                    foreach (var item in addOnDonationVMList)
                    {
                        CloudBlockBlob messageBlob = cloudBlobContainer.GetBlockBlobReference(item.ChildID + "-" + item.OrderGuid.ToString() + ".txt");
                        if (messageBlob.Exists())
                        {
                            AddOnDonationVM addOnDonation = new AddOnDonationVM();
                            addOnDonation.Forename = item.Forename;
                            addOnDonation.Surname = item.Surname;
                            addOnDonation.ChildID = item.ChildID;
                            addOnDonation.Message = messageBlob.DownloadText();
                            addOnDonation.DateOfRequest = item.DateOfRequest;
                            addOnDonationInfoList.Add(addOnDonation);
                        }
                    }
                }
                else
                {
                    addOnDonationInfoList = new List<AddOnDonationVM>();
                }
                return addOnDonationInfoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ImageInfoVM>> GetImageNames(SignUpExtractParameterVM signUpVM)
        {
            try
            {
                List<AddOnDonationVM> addOnDonationVMList = new List<AddOnDonationVM>();
                addOnDonationVMList = await GetAddOnDonationList();

                BlobStorageVM blobStorageVM = await imageStoreFactory.GetBobStorageDetails();
                CloudBlobContainer cloudBlobContainer = await _zipFileHelper.GetBlobImageContainer(blobStorageVM);

                List<ImageInfoVM> imageInfoVMList = new List<ImageInfoVM>();

                if (signUpVM.SignExtractType == "newtransaction")
                {
                    addOnDonationVMList = addOnDonationVMList.Where(x => x.Exported == false).ToList();
                }
                else if (signUpVM.SignExtractType == "date")
                {
                    addOnDonationVMList = addOnDonationVMList.Where(x => x.DateOfRequest >= signUpVM.SignStartDate.Value
                                                    && x.DateOfRequest <= signUpVM.SignEndDate.Value.AddDays(1)).ToList();
                }

                if (addOnDonationVMList.Count > 0)
                {
                    foreach (var item in addOnDonationVMList)
                    {
                        CloudBlockBlob photoBlob = cloudBlobContainer.GetBlockBlobReference(item.ChildID + "-" + item.OrderGuid.ToString() + ".jpeg");
                        if (photoBlob.Exists())
                        {
                            ImageInfoVM imageInfo = new ImageInfoVM();
                            imageInfo.BlobGUID = item.ChildID + "-" + item.OrderGuid.ToString() + ".jpeg";
                            imageInfoVMList.Add(imageInfo);
                        }
                    }
                }
                else
                {
                    imageInfoVMList = new List<ImageInfoVM>();
                }

                return imageInfoVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposed = false;

        /// <summary>
        ///Dispose the object used
        /// </summary>
        /// <param name=""></param>
        /// <returns>no values</returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.apiClient.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}