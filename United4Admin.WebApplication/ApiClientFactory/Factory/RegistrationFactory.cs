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

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class RegistrationFactory : IRegistrationFactory
    {

        protected ApiClient apiClient;
        private readonly IFeatureSwitchFactory _featureSwitchFactory;
        private bool kubernetesImage;
        private bool kubernetesEvent;

        public RegistrationFactory()
        {
            this.apiClient = new ApiClient();
            this._featureSwitchFactory = new FeatureSwitchFactory();
        }

        public RegistrationFactory(ApiClient _apiClient,IFeatureSwitchFactory featureSwitchFactory)
        {
            this.apiClient = _apiClient;
            this._featureSwitchFactory = featureSwitchFactory;
        }
        public async Task<bool> FeatureSwitchImage()
        {
            try
            {
                bool result = await _featureSwitchFactory.CheckFeatureSwitch("name=ImageStoreMSInKubernetes");
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SignUpVM>> LoadList()
        {
            try
            {
                List<SignUpVM> SignUpVMList = await GetSignupData();
                return SignUpVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SignUpVM> Load(int id)
        {
            try
            {
                SignUpVM SignUpVM = await GetSignupDataById(id);
                return SignUpVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ApiResponse> Create(SignUpVM SignUpVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.Create);
                var response = await apiClient.PostAsync<SignUpVM>(requestUrl, SignUpVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> UploadImage(IFormFile imageToUpload, string filenameFromId)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            List<KeyValuePair<string, string>> errors = new List<KeyValuePair<string, string>>();
            errors = ValidatePhoto(imageToUpload);
            if (errors.Count > 0)
            {
                ApiResponse responseReturn = new ApiResponse();
                imageToUpload = null;
                responseReturn.Success = false;
                responseReturn.Message = "Your photo is over 5MB and too large to upload (or) Please upload a jpeg type image";
                responseReturn.ResponseObject = null;
                return responseReturn;
            }
            else
            {
                HttpContent content = new StringContent("fileToUpload");
                form.Add(content, "fileToUpload");
                content = new StreamContent(imageToUpload.OpenReadStream());

                string imageName = filenameFromId + Path.GetExtension(imageToUpload.FileName);
                var filename = content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "name",
                    FileName = imageName
                };
                content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg");
                form.Add(content);
            }
            try
            {
                kubernetesImage = await FeatureSwitchImage();
                var requestUrl = apiClient.CreateRequestUploadUri(ImageStoreApiUrls.UploadImage.Replace("{nameOfFile}", filenameFromId));
                if (kubernetesImage)
                {
                    requestUrl = apiClient.CreateRequestUploadUri(ImageStoreApiUrls.UploadImageK8s.Replace("{nameOfFile}", filenameFromId));
                }
                var response = await apiClient.PostFileAsync(requestUrl, form);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(SignUpVM SignUpVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.Update);
                var response = await apiClient.PutSync<SignUpVM>(requestUrl, SignUpVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.Delete.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<SignUpVM>(requestUrl);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IList<WorkflowStatusVM>> GetWorkflowStatuses()
        {
            try
            {
                List<WorkflowStatusVM> workFlowVMList = new List<WorkflowStatusVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.GetWorkflowStatusListK8);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                workFlowVMList = JsonConvert.DeserializeObject<List<WorkflowStatusVM>>(Convert.ToString(response.ResponseObject));
                return workFlowVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TitleVM>> GetTitles()
        {
            try
            {
                List<TitleVM> titleVMList = new List<TitleVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetTitles);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                titleVMList = JsonConvert.DeserializeObject<List<TitleVM>>(Convert.ToString(response.ResponseObject));
                return titleVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<StatusVM>> GetStatuses()
        {
            try
            {
                List<StatusVM> statusVMList = new List<StatusVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetStatuses);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                statusVMList = JsonConvert.DeserializeObject<List<StatusVM>>(Convert.ToString(response.ResponseObject));
                return statusVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ChoosingPartyVM>> GetChoosingPartyList()
        {
            try
            {
                List<ChoosingPartyVM> choosingPartyVMs = new List<ChoosingPartyVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                choosingPartyVMs = JsonConvert.DeserializeObject<List<ChoosingPartyVM>>(Convert.ToString(response.ResponseObject));
                return choosingPartyVMs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RevealEventVM>> GetRevealEventList()
        {
            try
            {
                List<RevealEventVM> revealEventVMs = new List<RevealEventVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                revealEventVMs = JsonConvert.DeserializeObject<List<RevealEventVM>>(Convert.ToString(response.ResponseObject));
                return revealEventVMs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SignUpEventVM>> GetSignupEventList()
        {
            try
            {
                List<SignUpEventVM> signUpEventVMs = new List<SignUpEventVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                signUpEventVMs = JsonConvert.DeserializeObject<List<SignUpEventVM>>(Convert.ToString(response.ResponseObject));
                return signUpEventVMs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ImageInfoVM>> GetFieldDataExport(SignUpExtractParameterVM signUpVM)
        {
            try
            {
                List<ImageInfoVM> imageVMList = new List<ImageInfoVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetFieldDataExport);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                imageVMList = JsonConvert.DeserializeObject<List<ImageInfoVM>>(Convert.ToString(response.ResponseObject));

                List<ContactViewModel> contactVMList = new List<ContactViewModel>();

                if (signUpVM.SignExtractType == "newtransaction")
                {
                    contactVMList = await GetChosenNewTransactionList();
                    contactVMList = contactVMList.ToList();
                }
                else if (signUpVM.SignExtractType == "choosing")
                {
                    contactVMList = await GetChosenTransactionList();
                    imageVMList = imageVMList.Where(x => x.ChosenSignUp.ChoosingPartyId == signUpVM.SignChoosingPartyId).ToList();
                }
                else if (signUpVM.SignExtractType == "date")
                {
                    contactVMList = await GetChosenTransactionList();
                    contactVMList = contactVMList.Where(x => x.Order.DateFulfilled >= signUpVM.SignStartDate.Value
                    && x.Order.DateFulfilled <= signUpVM.SignEndDate.Value.AddDays(1)).ToList();
                }

                var chosenSupporters = (from img in imageVMList
                                        join chosenOrder in contactVMList on img.ChosenSignUp.DDLSalesOrderId.ToUpper() equals chosenOrder.Order.SalesOrderId.ToUpper()
                                        select new ImageInfoVM
                                        {
                                            ImageInfoId = img.ImageInfoId,
                                            BlobGUID = img.BlobGUID,
                                            ImageStatusId = img.ImageStatusId,
                                            ImageURL = img.ImageURL,
                                            UploadDateTime = img.UploadDateTime,
                                            ChosenSignUpId = img.ChosenSignUp.chosenSignUpId,
                                            ChosenSignUp = img.ChosenSignUp,
                                            FirstName = chosenOrder.Contact.FirstName,
                                            LastName = chosenOrder.Contact.LastName,
                                            Country = chosenOrder.WVCountry.WVCountryName
                                        }).ToList();

                return chosenSupporters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SignUpVM>> GetEchoData(SignUpExtractParameterVM signUpVM)
        {
            try
            {
                List<SignUpVM> signupVMList = new List<SignUpVM>();

                if (signUpVM.SignExtractType == "newtransaction")
                {
                    signupVMList = await GetSignupDataNewTrans();
                    signupVMList = signupVMList.ToList();
                }
                else if (signUpVM.SignExtractType == "choosing")
                {
                    signupVMList = await GetSignupData();
                    signupVMList = signupVMList.Where(x => x.ChoosingPartyId == signUpVM.SignChoosingPartyId).ToList();
                }
                else if (signUpVM.SignExtractType == "date")
                {
                    signupVMList = await GetSignupData();
                    signupVMList = signupVMList.Where(x => x.DateFulfilled >= signUpVM.SignStartDate.Value
                    && x.DateFulfilled <= signUpVM.SignEndDate.Value.AddDays(1)).ToList();
                }
                return signupVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SignUpVM>> GetPreRevealExtractData(CRMExtractParameterModelVM cRMExtractParameterModel)
        {
            try
            {
                List<SignUpVM> signupVMList = await GetSignupData();
                signupVMList = signupVMList.Where(x => x.DateFulfilled >= cRMExtractParameterModel.StartDate
                 && x.DateFulfilled <= cRMExtractParameterModel.EndDate.Value.AddDays(1)
                 && x.ChildID == null).ToList();
                return signupVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SignUpVM>> GetSignupData()
        {
            try
            {                
                List<SignUpVM> signupVMList = new List<SignUpVM>();
                signupVMList = await GetSignupList();

                List<ContactViewModel> contactVMList = new List<ContactViewModel>();
                contactVMList = await GetChosenTransactionList();

                var chosenSupporters = (from sign in signupVMList
                                        join chosenOrder in contactVMList on sign.DDLSalesOrderId.ToUpper() equals chosenOrder.Order.SalesOrderId.ToUpper()
                                        select new SignUpVM
                                        {
                                            Id = sign.chosenSignUpId,
                                            chosenSignUpId = sign.chosenSignUpId,
                                            ContactId = chosenOrder != null ? chosenOrder.Contact.ContactId : string.Empty,
                                            DDLSalesOrderId = Convert.ToString(sign.DDLSalesOrderId),
                                            Title = chosenOrder != null ? chosenOrder.Contact.Salutation : string.Empty,
                                            FirstName = chosenOrder != null ? chosenOrder.Contact.FirstName : string.Empty,
                                            LastName = chosenOrder != null ? chosenOrder.Contact.LastName : string.Empty,
                                            PaymentTransactionID = sign.PaymentTransactionID,
                                            taxId = chosenOrder != null ? chosenOrder.OrderProduct.WvTaxId : string.Empty,
                                            TaxConsent = chosenOrder != null ? chosenOrder.OrderProduct.WvTaxConsentOptIn.Value : false,
                                            SortCode = chosenOrder != null ? chosenOrder.PaymentSchedule.WvSortCode : string.Empty,
                                            AccountCode = chosenOrder != null ? chosenOrder.PaymentSchedule.WvAccountNumber : string.Empty,
                                            CardHolder = chosenOrder != null ? chosenOrder.PaymentSchedule.WvAccountHolder : string.Empty,
                                            TriggerCode = sign.SignUpEvent.CampaignCode,
                                            BuildingNumberName = string.Empty,
                                            StreetName = chosenOrder != null ? chosenOrder.Contact.Address1Line1 : string.Empty,
                                            AddressLine2 = chosenOrder != null ? chosenOrder.Contact.Address1Line2 : string.Empty,
                                            TownCity = chosenOrder != null ? chosenOrder.Contact.Address1City : string.Empty,
                                            AddressLine3 = chosenOrder != null ? chosenOrder.Contact.Address1City : string.Empty,
                                            AddressLine4 = string.Empty,
                                            Postcode = chosenOrder != null ? chosenOrder.Contact.Address1PostalCode : string.Empty,
                                            PhoneNumber = chosenOrder != null ? chosenOrder.Contact.MobilePhone : string.Empty,
                                            Country = chosenOrder != null ? chosenOrder.WVCountry != null ? chosenOrder.WVCountry.WVCountryName : string.Empty : string.Empty,
                                            PhoneNum1 = chosenOrder != null ? chosenOrder.Contact.MobilePhone : string.Empty,
                                            EmailAddress = chosenOrder != null ? chosenOrder.Contact.EmailAddress1 : string.Empty,
                                            ChosenStatusId = sign.ChosenStatusId,
                                            SignUpDate = chosenOrder != null ? chosenOrder.Order.DateFulfilled ?? DateTime.Now : DateTime.Now,
                                            SignUpEventId = sign.SignUpEventId,
                                            ChoosingPartyId = sign.ChoosingPartyId,
                                            RevealEventId = sign.RevealEventId,
                                            Location = sign.SignUpEvent.EventName + "|" + sign.SignUpEvent.Location,
                                            AiValue1 = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 1)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            AiValue2 = sign.ChoosingParty != null ? Convert.ToString(sign.ChoosingParty.HorizonId) : string.Empty,
                                            DirectMailOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                               && (x.WvPreferenceTypeId == 4)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            EmailOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 3)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            PhoneOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 5)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            SMSOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 6)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            DateOfRequest = chosenOrder != null ? chosenOrder.Order.DateFulfilled ?? DateTime.Now : DateTime.Now,
                                            Forename = chosenOrder != null ? chosenOrder.Contact.FirstName : string.Empty,
                                            Surname = chosenOrder != null ? chosenOrder.Contact.LastName : string.Empty,
                                            supporterID = Convert.ToString(sign.supporterID),
                                            Organisation_GroupName = string.Empty,
                                            Organisation_GroupType = string.Empty,
                                            gender = chosenOrder != null ? chosenOrder.Contact.GenderCode == null ? string.Empty : (chosenOrder.Contact.GenderCode == 1 ? "Male" : "Female") : string.Empty,
                                            YearOfBirth = chosenOrder != null ? chosenOrder.Contact.BirthDate != null ? chosenOrder.Contact.BirthDate.Value.ToString("dd/MM/yyyy") : string.Empty : string.Empty,
                                            PhoneNum2 = sign.PhoneNum2,
                                            PaymentReference = chosenOrder != null ? chosenOrder.Transaction.WvPaymentProviderTransactionId ?? string.Empty : string.Empty,
                                            PreferredContinent = string.Empty,
                                            PreferredCountry = string.Empty,
                                            PreferredGender = string.Empty,
                                            PreferredAge = string.Empty,
                                            Fundraised = sign.Fundraised,
                                            Comments = sign.SignUpEvent.NotesComments,
                                            ResponseEntity = string.Empty,
                                            ScheduledPayID = string.Empty,
                                            FaithAndFamily = string.Empty,
                                            DirectMailOptOut = string.Empty,
                                            EmailOptOut = string.Empty,
                                            SMSOptOut = string.Empty,
                                            RequestType = sign.RequestType,
                                            ExternalPaymentToken = string.Empty,
                                            IBAN = chosenOrder != null ? chosenOrder.PaymentSchedule.WvIban : string.Empty,
                                            ProductID = chosenOrder != null ? Convert.ToString(chosenOrder.OrderProduct.WvProductId) : string.Empty,
                                            Donationamount = chosenOrder != null ? chosenOrder.Transaction.Amount : 0,
                                            DonationvariantID = chosenOrder != null ? chosenOrder.OrderProduct.ProductTypeCodedisplay : string.Empty,
                                            Donationfrequency = chosenOrder != null ? chosenOrder.PaymentSchedule.Frequency == 1 ? "M" : "O" : string.Empty,
                                            AdrStatusID = string.Empty,
                                            AdrTypeID = string.Empty,
                                            Title1Descr = string.Empty,
                                            Title2Descr = string.Empty,
                                            MaritalStatusDescr = string.Empty,
                                            AddDate = string.Empty,
                                            FamilyName2 = string.Empty,
                                            OrgName1 = string.Empty,
                                            OrgName2 = string.Empty,
                                            OrgDepartmentName = string.Empty,
                                            OrgRoleDescr = string.Empty,
                                            PartnershipName = string.Empty,
                                            StatesProvCode = string.Empty,
                                            RegionDescr = string.Empty,
                                            NoMail = string.Empty,
                                            NoMailID = string.Empty,
                                            SpokenLanguageCode = string.Empty,
                                            PrintedLanguageCode = string.Empty,
                                            ReceiptingID = string.Empty,
                                            MotivationID = string.Empty,
                                            ReferenceText = string.Empty,
                                            PhoneTypeID1 = string.Empty,
                                            PhoneTypeID2 = string.Empty,
                                            ChosenTempSupporterID = Convert.ToString(sign.chosenSignUpId),
                                            ChosenUKEventName = sign.SignUpEvent.EventName,
                                            ChosenFieldEventId = sign.ChoosingParty != null ? Convert.ToString(sign.ChoosingParty.HorizonId) : string.Empty,
                                            ChosenFieldEventDate = sign.ChoosingParty != null ? Convert.ToString(sign.ChoosingParty.PartyDate) : string.Empty,
                                            ChosenRevealType = sign.RevealEvent != null ? sign.RevealEvent.TypeOfReveal : string.Empty,
                                            ChosenRevealDate = sign.RevealEvent != null ? Convert.ToString(sign.RevealEvent.EventDate) : string.Empty,
                                            AccountHoldersName = chosenOrder != null ? chosenOrder.PaymentSchedule.WvAccountHolder : string.Empty,
                                            ChoosingParty = sign.ChoosingParty,
                                            RevealEvent = sign.RevealEvent,
                                            SignUpEvent = sign.SignUpEvent,
                                            Status = sign.Status,
                                            ImageInfo = sign.ImageInfo,
                                            DateFulfilled = chosenOrder != null ? chosenOrder.Order.DateFulfilled.GetValueOrDefault() : DateTime.Now,
                                            ChildID = chosenOrder != null ? chosenOrder.OrderProduct.WvChildId : string.Empty,
                                            CommitmentAmount = chosenOrder != null ? chosenOrder.Transaction.Amount > 26 ? Convert.ToString((int)(chosenOrder.Transaction.Amount * 100)) : string.Empty : string.Empty,
                                            TransactionAmount = string.Empty,
                                            Dataprocessingconsent = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 1)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false : false,
                                            Marketingcommsconsent = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 2)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false : false,
                                            Exported = chosenOrder != null ? chosenOrder.Order.WvExtractStatus.GetValueOrDefault() : false
                                        }).ToList();
                return chosenSupporters;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<SignUpVM>> GetSignupDataNewTrans()
        {
            try
            {
                List<SignUpVM> signupVMList = new List<SignUpVM>();
                signupVMList = await GetSignupList();

                List<ContactViewModel> contactVMList = new List<ContactViewModel>();
                contactVMList = await GetChosenNewTransactionList();

                var chosenSupporters = (from sign in signupVMList
                                        join chosenOrder in contactVMList on sign.DDLSalesOrderId.ToUpper() equals chosenOrder.Order.SalesOrderId.ToUpper()
                                        select new SignUpVM
                                        {
                                            Id = sign.chosenSignUpId,
                                            chosenSignUpId = sign.chosenSignUpId,
                                            ContactId = chosenOrder != null ? chosenOrder.Contact.ContactId : string.Empty,
                                            DDLSalesOrderId = Convert.ToString(sign.DDLSalesOrderId),
                                            Title = chosenOrder != null ? chosenOrder.Contact.Salutation : string.Empty,
                                            FirstName = chosenOrder != null ? chosenOrder.Contact.FirstName : string.Empty,
                                            LastName = chosenOrder != null ? chosenOrder.Contact.LastName : string.Empty,
                                            PaymentTransactionID = sign.PaymentTransactionID,
                                            taxId = chosenOrder != null ? chosenOrder.OrderProduct.WvTaxId : string.Empty,
                                            TaxConsent = chosenOrder != null ? chosenOrder.OrderProduct.WvTaxConsentOptIn.Value : false,
                                            SortCode = chosenOrder != null ? chosenOrder.PaymentSchedule.WvSortCode : string.Empty,
                                            AccountCode = chosenOrder != null ? chosenOrder.PaymentSchedule.WvAccountNumber : string.Empty,
                                            CardHolder = chosenOrder != null ? chosenOrder.PaymentSchedule.WvAccountHolder : string.Empty,
                                            TriggerCode = sign.SignUpEvent.CampaignCode,
                                            BuildingNumberName = string.Empty,
                                            StreetName = chosenOrder != null ? chosenOrder.Contact.Address1Line1 : string.Empty,
                                            AddressLine2 = chosenOrder != null ? chosenOrder.Contact.Address1Line2 : string.Empty,
                                            TownCity = chosenOrder != null ? chosenOrder.Contact.Address1City : string.Empty,
                                            AddressLine3 = chosenOrder != null ? chosenOrder.Contact.Address1City : string.Empty,
                                            AddressLine4 = string.Empty,
                                            Postcode = chosenOrder != null ? chosenOrder.Contact.Address1PostalCode : string.Empty,
                                            PhoneNumber = chosenOrder != null ? chosenOrder.Contact.MobilePhone : string.Empty,
                                            Country = chosenOrder != null ? chosenOrder.WVCountry != null ? chosenOrder.WVCountry.WVCountryName : string.Empty : string.Empty,
                                            PhoneNum1 = chosenOrder != null ? chosenOrder.Contact.MobilePhone : string.Empty,
                                            EmailAddress = chosenOrder != null ? chosenOrder.Contact.EmailAddress1 : string.Empty,
                                            ChosenStatusId = sign.ChosenStatusId,
                                            SignUpDate = chosenOrder != null ? chosenOrder.Order.DateFulfilled ?? DateTime.Now : DateTime.Now,
                                            SignUpEventId = sign.SignUpEventId,
                                            ChoosingPartyId = sign.ChoosingPartyId,
                                            RevealEventId = sign.RevealEventId,
                                            Location = sign.SignUpEvent.EventName + "|" + sign.SignUpEvent.Location,
                                            AiValue1 = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 1)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            AiValue2 = sign.ChoosingParty != null ? Convert.ToString(sign.ChoosingParty.HorizonId) : string.Empty,
                                            DirectMailOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                               && (x.WvPreferenceTypeId == 4)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            EmailOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 3)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            PhoneOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 5)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            SMSOptIn = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 6)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? "1" : "0" : string.Empty,
                                            DateOfRequest = chosenOrder != null ? chosenOrder.Order.DateFulfilled ?? DateTime.Now : DateTime.Now,
                                            Forename = chosenOrder != null ? chosenOrder.Contact.FirstName : string.Empty,
                                            Surname = chosenOrder != null ? chosenOrder.Contact.LastName : string.Empty,
                                            supporterID = Convert.ToString(sign.supporterID),
                                            Organisation_GroupName = string.Empty,
                                            Organisation_GroupType = string.Empty,
                                            gender = chosenOrder != null ? chosenOrder.Contact.GenderCode == null ? string.Empty : (chosenOrder.Contact.GenderCode == 1 ? "Male" : "Female") : string.Empty,
                                            YearOfBirth = chosenOrder != null ? chosenOrder.Contact.BirthDate != null ? chosenOrder.Contact.BirthDate.Value.ToString("dd/MM/yyyy") : string.Empty : string.Empty,
                                            PhoneNum2 = sign.PhoneNum2,
                                            PaymentReference = chosenOrder != null ? chosenOrder.Transaction.WvPaymentProviderTransactionId ?? string.Empty : string.Empty,
                                            PreferredContinent = string.Empty,
                                            PreferredCountry = string.Empty,
                                            PreferredGender = string.Empty,
                                            PreferredAge = string.Empty,
                                            Fundraised = sign.Fundraised,
                                            Comments = sign.SignUpEvent.NotesComments,
                                            ResponseEntity = string.Empty,
                                            ScheduledPayID = string.Empty,
                                            FaithAndFamily = string.Empty,
                                            DirectMailOptOut = string.Empty,
                                            EmailOptOut = string.Empty,
                                            SMSOptOut = string.Empty,
                                            RequestType = sign.RequestType,
                                            ExternalPaymentToken = string.Empty,
                                            IBAN = chosenOrder != null ? chosenOrder.PaymentSchedule.WvIban : string.Empty,
                                            ProductID = chosenOrder != null ? Convert.ToString(chosenOrder.OrderProduct.WvProductId) : string.Empty,
                                            Donationamount = chosenOrder != null ? chosenOrder.Transaction.Amount : 0,
                                            DonationvariantID = chosenOrder != null ? chosenOrder.OrderProduct.ProductTypeCodedisplay : string.Empty,
                                            Donationfrequency = chosenOrder != null ? chosenOrder.PaymentSchedule.Frequency == 1 ? "M" : "O" : string.Empty,
                                            AdrStatusID = string.Empty,
                                            AdrTypeID = string.Empty,
                                            Title1Descr = string.Empty,
                                            Title2Descr = string.Empty,
                                            MaritalStatusDescr = string.Empty,
                                            AddDate = string.Empty,
                                            FamilyName2 = string.Empty,
                                            OrgName1 = string.Empty,
                                            OrgName2 = string.Empty,
                                            OrgDepartmentName = string.Empty,
                                            OrgRoleDescr = string.Empty,
                                            PartnershipName = string.Empty,
                                            StatesProvCode = string.Empty,
                                            RegionDescr = string.Empty,
                                            NoMail = string.Empty,
                                            NoMailID = string.Empty,
                                            SpokenLanguageCode = string.Empty,
                                            PrintedLanguageCode = string.Empty,
                                            ReceiptingID = string.Empty,
                                            MotivationID = string.Empty,
                                            ReferenceText = string.Empty,
                                            PhoneTypeID1 = string.Empty,
                                            PhoneTypeID2 = string.Empty,
                                            ChosenTempSupporterID = Convert.ToString(sign.chosenSignUpId),
                                            ChosenUKEventName = sign.SignUpEvent.EventName,
                                            ChosenFieldEventId = sign.ChoosingParty != null ? Convert.ToString(sign.ChoosingParty.HorizonId) : string.Empty,
                                            ChosenFieldEventDate = sign.ChoosingParty != null ? Convert.ToString(sign.ChoosingParty.PartyDate) : string.Empty,
                                            ChosenRevealType = sign.RevealEvent != null ? sign.RevealEvent.TypeOfReveal : string.Empty,
                                            ChosenRevealDate = sign.RevealEvent != null ? Convert.ToString(sign.RevealEvent.EventDate) : string.Empty,
                                            AccountHoldersName = chosenOrder != null ? chosenOrder.PaymentSchedule.WvAccountHolder : string.Empty,
                                            ChoosingParty = sign.ChoosingParty,
                                            RevealEvent = sign.RevealEvent,
                                            SignUpEvent = sign.SignUpEvent,
                                            Status = sign.Status,
                                            ImageInfo = sign.ImageInfo,
                                            DateFulfilled = chosenOrder != null ? chosenOrder.Order.DateFulfilled.GetValueOrDefault() : DateTime.Now,
                                            ChildID = chosenOrder != null ? chosenOrder.OrderProduct.WvChildId : string.Empty,
                                            CommitmentAmount = chosenOrder != null ? chosenOrder.Transaction.Amount > 26 ? Convert.ToString((int)(chosenOrder.Transaction.Amount * 100)) : string.Empty : string.Empty,
                                            TransactionAmount = string.Empty,
                                            Dataprocessingconsent = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 1)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false : false,
                                            Marketingcommsconsent = chosenOrder != null ? (chosenOrder.Preference.Where(x => x.SalesOrderId.ToUpper() == sign.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                && (x.WvPreferenceTypeId == 2)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false : false,
                                            Exported = chosenOrder != null ? chosenOrder.Order.WvExtractStatus.GetValueOrDefault() : false
                                        }).ToList();
                return chosenSupporters;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<SignUpVM> GetSignupDataById(int signupId)
        {
            try
            {
                SignUpVM signUpVM = new SignUpVM();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.Load.Replace("{id}", Convert.ToString(signupId)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                signUpVM = JsonConvert.DeserializeObject<SignUpVM>(Convert.ToString(response.ResponseObject));


                ContactViewModel contactVM = new ContactViewModel();
                requestUrl = apiClient.CreateRequestUri(ChosenExtractApiUrls.chosenExtractLoad.Replace("{salesOrderId}", Convert.ToString(signUpVM.DDLSalesOrderId)));
                response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                contactVM = JsonConvert.DeserializeObject<ContactViewModel>(Convert.ToString(response.ResponseObject));

                if (contactVM != null)
                {
                    signUpVM.ContactId = contactVM.Contact.ContactId;
                    signUpVM.Title = contactVM.Contact.Salutation;
                    signUpVM.FirstName = contactVM.Contact.FirstName;
                    signUpVM.LastName = contactVM.Contact.LastName;
                    signUpVM.BuildingNumberName = "";
                    signUpVM.StreetName = contactVM.Contact.Address1Line1;
                    signUpVM.AddressLine2 = contactVM.Contact.Address1Line2;
                    signUpVM.TownCity = contactVM.Contact.Address1City;
                    signUpVM.Postcode = contactVM.Contact.Address1PostalCode;
                    signUpVM.PhoneNumber = contactVM.Contact.MobilePhone;
                    signUpVM.EmailAddress = contactVM.Contact.EmailAddress1;
                    signUpVM.SignUpDate = contactVM.Order.DateFulfilled ?? DateTime.Now;
                    signUpVM.CorrectedBankAccountNumber = contactVM.PaymentSchedule.WvAccountNumber;
                    signUpVM.CorrectedBankSortCode = contactVM.PaymentSchedule.WvSortCode;
                    signUpVM.DirectDebitCapable = false;
                    signUpVM.DirectDebitStatusInfo = (contactVM.PaymentSchedule.StateCode ?? 0) == 1 ? "true" : "false";
                    signUpVM.Iban = contactVM.PaymentSchedule.WvIban;
                    signUpVM.TaxConsent = contactVM.OrderProduct.WvTaxConsentOptIn ?? false;
                    signUpVM.Post = (contactVM.Preference.Where(x => x.SalesOrderId.ToUpper() == signUpVM.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                    && (x.WvPreferenceTypeId == 4)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false;
                    signUpVM.Email = (contactVM.Preference.Where(x => x.SalesOrderId.ToUpper() == signUpVM.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                    && (x.WvPreferenceTypeId == 3)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false;
                    signUpVM.Phone = (contactVM.Preference.Where(x => x.SalesOrderId.ToUpper() == signUpVM.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                    && (x.WvPreferenceTypeId == 5)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false;
                    signUpVM.SMS = (contactVM.Preference.Where(x => x.SalesOrderId.ToUpper() == signUpVM.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                    && (x.WvPreferenceTypeId == 6)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false;
                    signUpVM.DataConsent = (contactVM.Preference.Where(x => x.SalesOrderId.ToUpper() == signUpVM.DDLSalesOrderId.ToUpper() && x.EndDate == null
                                                    && (x.WvPreferenceTypeId == 1)).Select(x => x.WvPreferenceTypeId).FirstOrDefault()) != null ? true : false;
                    signUpVM.County = "";
                    signUpVM.AccountHoldersName = contactVM.PaymentSchedule.WvAccountHolder;
                }

                return signUpVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<SignUpVM>> GetPhotoApproval()
        {
            try
            {
                List<SignUpVM> signupVMList = new List<SignUpVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetPhotoApporval);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                signupVMList = JsonConvert.DeserializeObject<List<SignUpVM>>(Convert.ToString(response.ResponseObject));

                List<ContactViewModel> contactVMList = new List<ContactViewModel>();
                contactVMList = await GetChosenTransactionList();

                var chosenSupporters = (from sign in signupVMList
                                        join chosenOrder in contactVMList on sign.DDLSalesOrderId.ToUpper() equals chosenOrder.Order.SalesOrderId.ToUpper()
                                        select new SignUpVM
                                        {
                                            chosenSignUpId = sign.chosenSignUpId,
                                            ContactId = chosenOrder.Contact.ContactId,
                                            DDLSalesOrderId = Convert.ToString(sign.DDLSalesOrderId),
                                            Title = chosenOrder.Contact.Salutation,
                                            FirstName = chosenOrder.Contact.FirstName,
                                            LastName = chosenOrder.Contact.LastName,
                                            ImageStatusName = sign.ImageStatusName
                                        }).ToList();

                return chosenSupporters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> CreateImage(ImageInfoVM ImageInfoVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.CreateImage);
                var response = await apiClient.PostAsync<ImageInfoVM>(requestUrl, ImageInfoVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> UpdateImage(ImageInfoVM ImageInfoVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.UpdateImage);
                var response = await apiClient.PutSync<ImageInfoVM>(requestUrl, ImageInfoVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> DeleteImage(int id)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.DeleteImage.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<ApiResponse>(requestUrl);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ImageInfoVM> GetImage(int SignUpId)
        {
            try
            {
                ImageInfoVM imageInfoVM = new ImageInfoVM();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetImage.Replace("{id}", Convert.ToString(SignUpId)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                imageInfoVM = JsonConvert.DeserializeObject<ImageInfoVM>(Convert.ToString(response.ResponseObject));
                return imageInfoVM;
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
                List<ImageInfoVM> imageInfoVMList = new List<ImageInfoVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetImageList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                imageInfoVMList = JsonConvert.DeserializeObject<List<ImageInfoVM>>(Convert.ToString(response.ResponseObject));

                if (signUpVM.SignExtractType == "newtransaction")
                {
                    List<ContactViewModel> contactVMList = await GetChosenNewTransactionList();
                    List<SignUpVM> signupVMList = await GetSignupList();
                    if (signupVMList.Count > 0)
                    {
                        imageInfoVMList = (from img in contactVMList
                                           join chosenOrder in signupVMList on img.Order.SalesOrderId.ToUpper() equals chosenOrder.DDLSalesOrderId.ToUpper()
                                           select new ImageInfoVM
                                           {
                                               ImageInfoId = chosenOrder.ImageInfo != null ? chosenOrder.ImageInfo.ImageInfoId : 0,
                                               BlobGUID = chosenOrder.ImageInfo != null ? chosenOrder.ImageInfo.BlobGUID : "",
                                               ImageStatusId = chosenOrder.ImageInfo != null ? chosenOrder.ImageInfo.ImageStatusId : 0,
                                               ImageURL = chosenOrder.ImageInfo != null ? chosenOrder.ImageInfo.ImageURL : "",
                                               UploadDateTime = chosenOrder.ImageInfo != null ? chosenOrder.ImageInfo.UploadDateTime : DateTime.Now,
                                               ChosenSignUpId = chosenOrder.chosenSignUpId,
                                               ChosenSignUp = chosenOrder,
                                           }).ToList();
                    }
                    else
                    {
                        imageInfoVMList = new List<ImageInfoVM>();
                    }
                }
                else if (signUpVM.SignExtractType == "choosing")
                {
                    List<SignUpVM> signupVMList = await GetSignupList();
                    signupVMList = signupVMList.Where(x => x.ChoosingPartyId == signUpVM.SignChoosingPartyId).ToList();
                    if (signupVMList.Count > 0)
                    {
                        imageInfoVMList = (from img in imageInfoVMList
                                           join chosenOrder in signupVMList on img.ChosenSignUpId equals chosenOrder.chosenSignUpId
                                           select new ImageInfoVM
                                           {
                                               ImageInfoId = img.ImageInfoId,
                                               BlobGUID = img.BlobGUID,
                                               ImageStatusId = img.ImageStatusId,
                                               ImageURL = img.ImageURL,
                                               UploadDateTime = img.UploadDateTime,
                                               ChosenSignUpId = chosenOrder.chosenSignUpId,
                                               ChosenSignUp = chosenOrder,
                                           }).ToList();
                    }
                    else
                    {
                        imageInfoVMList = new List<ImageInfoVM>();
                    }
                }
                else if (signUpVM.SignExtractType == "date")
                {
                    imageInfoVMList = imageInfoVMList.Where(x => x.UploadDateTime >= signUpVM.SignStartDate.Value
                    && x.UploadDateTime <= signUpVM.SignEndDate.Value.AddDays(1)).ToList();
                }

                return imageInfoVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SignUpVM>> GetImageNotUploadData()
        {
            try
            {
                List<SignUpVM> signupVMList = new List<SignUpVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.GetImageNotUploadData);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                signupVMList = JsonConvert.DeserializeObject<List<SignUpVM>>(Convert.ToString(response.ResponseObject));

                List<ContactViewModel> contactVMList = new List<ContactViewModel>();
                contactVMList = await GetChosenTransactionList();

                var chosenSupporters = (from sign in signupVMList
                                        join chosenOrder in contactVMList on sign.DDLSalesOrderId.ToUpper() equals chosenOrder.Order.SalesOrderId.ToUpper()
                                        select new SignUpVM
                                        {
                                            chosenSignUpId = sign.chosenSignUpId,
                                            ContactId = chosenOrder.Contact.ContactId,
                                            DDLSalesOrderId = Convert.ToString(sign.DDLSalesOrderId),
                                            Title = chosenOrder.Contact.Salutation,
                                            FirstName = chosenOrder.Contact.FirstName,
                                            LastName = chosenOrder.Contact.LastName,
                                            RegistrationEventName = sign.RegistrationEventName
                                        }).ToList();


                return chosenSupporters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<KeyValuePair<string, string>> ValidatePhoto(IFormFile photo)
        {
            List<KeyValuePair<string, string>> errors = new List<KeyValuePair<string, string>>();
            try
            {
                //validate for size
                if (photo.Length > 5 * 1048576)//5MB
                {
                    errors.Add(new KeyValuePair<string, string>("TakePhotoFromDevice", "Your photo is over 5MB and too large to upload"));
                }
                //validate for type
                List<string> allowedExtensions = new List<string>() { ".jpg", ".jpeg" };
                string extension = Path.GetExtension(photo.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    errors.Add(new KeyValuePair<string, string>("TakePhotoFromDevice", "Please upload a jpeg type image"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errors;
        }

        //method to load signup list data
        public async Task<List<SignUpVM>> GetSignupList()
        {
            try
            {
                List<SignUpVM> signupVMList = new List<SignUpVM>();
                var requestUrl = apiClient.CreateRequestUri(RegistrationK8ApiUrls.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                signupVMList = JsonConvert.DeserializeObject<List<SignUpVM>>(Convert.ToString(response.ResponseObject));
                return signupVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //method to load chosen transactionlist data
        public async Task<List<ContactViewModel>> GetChosenTransactionList()
        {
            try
            {
                List<ContactViewModel> contactVMList = new List<ContactViewModel>();
                var requestUrl = apiClient.CreateRequestUri(ChosenExtractApiUrls.chosenExtractLoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                contactVMList = JsonConvert.DeserializeObject<List<ContactViewModel>>(Convert.ToString(response.ResponseObject));
                return contactVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //method to load chosen new transactionlist data
        public async Task<List<ContactViewModel>> GetChosenNewTransactionList()
        {
            try
            {
                List<ContactViewModel> contactVMList = new List<ContactViewModel>();
                var requestUrl = apiClient.CreateRequestUri(ChosenExtractApiUrls.chosenNewExtractLoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                contactVMList = JsonConvert.DeserializeObject<List<ContactViewModel>>(Convert.ToString(response.ResponseObject));
                return contactVMList;
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