using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace United4Admin.WebApplication.BLL
{
    public static class FieldDataExport
    {
        public static List<ExportDataVM.FieldDataExport> ConvertData(List<ImageInfoVM> query)
        {
            List<ExportDataVM.FieldDataExport> fieldData = query.Select(x => new ExportDataVM.FieldDataExport
            {
                SponsorAccount = ApplicationConstants.REFERENCEBEGINSTRING + x.ChosenSignUpId.ToString(ApplicationConstants.REFERENCEFORMAT),
                SponsorName = x.FirstName + " " + x.LastName,
                Country = x.Country,
                SponsorPhotoFileName = x.BlobGUID
            }).ToList();
            return fieldData;
        }
    }
    public static class AddOnDataExport
    {
        public static List<ExportDataVM.AddOnDataExport> ConvertAddOnData(List<AddOnDonationVM> query)
        {
            List<ExportDataVM.AddOnDataExport> fieldData = query.Select(x => new ExportDataVM.AddOnDataExport
            {
                SponsorName = x.Forename + " " + x.Surname,
                ChildId = x.ChildID,
                Message = x.Message,
                Date = x.DateOfRequest.ToString("dd/MM/yyyy")
            }).ToList();
            return fieldData;
        }
    }
    public static class EchoDataExport
    {
        public static string ConvertData(List<SignUpVM> query, bool iSPreRevealflag)
        {
            string csv = string.Empty;
            // if PreRevealData extract
            if (iSPreRevealflag)
            {
                ExportDataVM.PreRevealDataExport echoData = new ExportDataVM.PreRevealDataExport();
                if (query != null && query.Count > 0)
                {
                    foreach (var property in echoData.GetType().GetProperties())
                    {
                        if (property.Name != "Id")
                        { csv += property.Name + ","; }

                    }
                    csv += "\n";
                }

                //"\"" is entered before and after strings which might contain commas to prevent those values messing up the file
                csv += string.Concat(from data in query
                                     select data.RequestType + ","
                                     + data.TransactionReferenceNo + ","
                                     + data.RequestReferenceNo + ","
                                     + data.DateOfRequest + ","
                                     + data.TriggerCode + ","
                                     + data.ConsentStatementID + ","
                                     + data.supporterID + ","
                                     + data.SupporterType + ","
                                     + data.Title + ","
                                     + "\"" + data.Forename + "\"" + ","
                                     + "\"" + data.Surname + "\"" + ","
                                     + data.Organisation_GroupName + ","
                                     + data.Organisation_GroupType + ","
                                     + data.gender + ","
                                     + data.YearOfBirth + ","
                                     + (data.TaxConsent ? "1" : "0") + ","
                                     + data.AddressStructure + ","
                                     + "\"" + (data.BuildingNumberName.Any(char.IsDigit) ? data.BuildingNumberName : "") + "\"" + ","
                                     + "\"" + (data.BuildingNumberName.Any(char.IsDigit) ? "" : data.BuildingNumberName) + "\"" + ","
                                     + "\"" + data.StreetName + "\"" + ","
                                     + "\"" + data.AddressLine2 + "\"" + ","
                                     + "\"" + data.AddressLine3 + "\"" + ","
                                     + "\"" + data.AddressLine4 + "\"" + ","
                                     + data.Postcode + ","
                                     + data.Country + ","
                                     + data.PhoneNum1 + ","
                                     + data.PhoneNum2 + ","
                                     + data.EmailAddress + ","
                                     + data.Location + ","
                                     + data.CommitmentAmount + ","
                                     + data.TransactionAmount + ","
                                     + data.PaymentMethod + ","
                                     + data.PaymentType + ","
                                     + data.PaymentReference + ","
                                     + data.PaymentFrequency + ","
                                     + data.PreferredDDDate + ","
                                     + data.SortCode + ","
                                     + data.AccountCode + ","
                                     + data.CardHolder + ","
                                     + data.PreferredContinent + ","
                                     + data.PreferredCountry + ","
                                     + data.PreferredGender + ","
                                     + data.PreferredAge + ","
                                     + data.Receipt_Required + ","
                                     + data.Fundraised + ","
                                     + data.CommitmentCode + ","
                                     + data.PaymentTransactionID + ","
                                     + data.Comments + ","
                                     + data.ResponseEntity + ","
                                     + data.ScheduledPayID + ","
                                     + data.AiName1 + ","
                                     + data.AiValue1 + ","
                                     + data.AiName2 + ","
                                     + data.AiValue2 + ","
                                     + data.FaithAndFamily + ","
                                     + data.DirectMailOptIn + ","
                                     + data.EmailOptIn + ","
                                     + data.PhoneOptIn + ","
                                     + data.SMSOptIn + ","
                                     + data.DirectMailOptOut + ","
                                     + data.EmailOptOut + ","
                                     + data.PhoneOptOut + ","
                                     + data.SMSOptOut + ","
                                     + data.ChosenTempSupporterID + ","
                    + data.ChosenUKEventName + "," + data.ChosenFieldEventId + "," + data.ChosenFieldEventDate + "," + data.ChosenRevealType + "," + data.ChosenRevealDate + ","
                    + data.ExternalPaymentToken + "," + data.IBAN + "," + data.ProductID + "," + data.Donationamount + "," + data.DonationvariantID + "," + data.Donationfrequency + ","
                    + data.AdrStatusID + "," + data.AdrTypeID + "," + data.Title1Descr + "," + data.Title2Descr + "," + data.MaritalStatusDescr + "," + data.AddDate + ","
                    + data.FamilyName2 + "," + data.OrgName1 + "," + data.OrgName2 + "," + data.OrgDepartmentName + "," + data.OrgRoleDescr + ","
                    + data.PartnershipName + "," + data.StatesProvCode + "," + data.RegionDescr + "," + data.NoMail + "," + data.NoMailID + ","
                    + data.SpokenLanguageCode + "," + data.PrintedLanguageCode + "," + data.ReceiptingID + "," + data.MotivationID + ","
                    + data.ReferenceText + "," + data.PhoneTypeID1 + "," + data.PhoneTypeID2 + "\n");
            }

            // if chosen echo extract
            else
            {
                ExportDataVM.EchoDataExport echoData = new ExportDataVM.EchoDataExport();
                if (query != null && query.Count > 0)
                {
                    foreach (var property in echoData.GetType().GetProperties())
                    {
                        if (property.Name != "Id")
                        { csv += property.Name + ","; }

                    }
                    csv += "\n";
                }


                //"\"" is entered before and after strings which might contain commas to prevent those values messing up the file
                csv += string.Concat(from data in query
                                     select data.RequestType + ","
                                     + data.TransactionReferenceNo + ","
                                     + data.RequestReferenceNo + ","
                                     + data.DateOfRequest + ","
                                     + data.TriggerCode + ","
                                     + data.ConsentStatementID + ","
                                     + data.supporterID + ","
                                     + data.SupporterType + ","
                                     + data.Title + ","
                                     + "\"" + data.Forename + "\"" + ","
                                     + "\"" + data.Surname + "\"" + ","
                                     + data.Organisation_GroupName + ","
                                     + data.Organisation_GroupType + ","
                                     + data.gender + ","
                                     + data.YearOfBirth + ","
                                     + (data.TaxConsent ? "1" : "0") + ","
                                     + data.AddressStructure + ","
                                     + "\"" + (data.BuildingNumberName.Any(char.IsDigit) ? data.BuildingNumberName : "") + "\"" + ","
                                     + "\"" + (data.BuildingNumberName.Any(char.IsDigit) ? "" : data.BuildingNumberName) + "\"" + ","
                                     + "\"" + data.StreetName + "\"" + ","
                                     + "\"" + data.AddressLine2 + "\"" + ","
                                     + "\"" + data.AddressLine3 + "\"" + ","
                                     + "\"" + data.AddressLine4 + "\"" + ","
                                     + data.Postcode + ","
                                     + data.Country + ","
                                     + data.PhoneNum1 + ","
                                     + data.PhoneNum2 + ","
                                     + data.EmailAddress + ","
                                     + data.Location + ","
                                     + data.CommitmentAmount + ","
                                     + data.TransactionAmount + ","
                                     + data.PaymentMethod + ","
                                     + data.PaymentType + ","
                                     + data.PaymentReference + ","
                                     + data.PaymentFrequency + ","
                                     + data.PreferredDDDate + ","
                                     + data.SortCode + ","
                                     + data.AccountCode + ","
                                     + data.CardHolder + ","
                                     + data.ChildID + ","
                                     + data.PreferredContinent + ","
                                     + data.PreferredCountry + ","
                                     + data.PreferredGender + ","
                                     + data.PreferredAge + ","
                                     + data.Receipt_Required + ","
                                     + data.Fundraised + ","
                                     + data.CommitmentCode + ","
                                     + data.PaymentTransactionID + ","
                                     + data.Comments + ","
                                     + data.ResponseEntity + ","
                                     + data.ScheduledPayID + ","
                                     + data.AiName1 + ","
                                     + data.AiValue1 + ","
                                     + data.AiName2 + ","
                                     + data.AiValue2 + ","
                                     + data.FaithAndFamily + ","
                                     + data.DirectMailOptIn + ","
                                     + data.EmailOptIn + ","
                                     + data.PhoneOptIn + ","
                                     + data.SMSOptIn + ","
                                     + data.DirectMailOptOut + ","
                                     + data.EmailOptOut + ","
                                     + data.PhoneOptOut + ","
                                     + data.SMSOptOut + ","
                                     + data.ChosenTempSupporterID + ","
                    + data.ChosenUKEventName + "," + data.ChosenFieldEventId + "," + data.ChosenFieldEventDate + "," + data.ChosenRevealType + "," + data.ChosenRevealDate + ","
                    + data.Exported + "," + data.ExtractDate + "," + data.MiddleName + "," + data.taxId + "," + data.Dataprocessingconsent + ","
                    + data.Marketingcommsconsent + "," + data.ExternalPaymentToken + "," + data.IBAN + "," + data.ProductID + "," + data.Donationamount + ","
                    + data.DonationvariantID + "," + data.Donationfrequency + "," + data.AdrStatusID + "," + data.AdrTypeID + "," + data.Title1Descr + ","
                    + data.Title2Descr + "," + data.MaritalStatusDescr + "," + data.AddDate + "," + data.FamilyName2 + "," + data.OrgName1 + "," + data.OrgName2 + ","
                    + data.OrgDepartmentName + "," + data.OrgRoleDescr + "," + data.PartnershipName + "," + data.StatesProvCode + "," + data.RegionDescr + ","
                    + data.NoMail + "," + data.NoMailID + "," + data.SpokenLanguageCode + "," + data.PrintedLanguageCode + "," + data.ReceiptingID + ","
                    + data.MotivationID + "," + data.ReferenceText + "," + data.PhoneTypeID1 + "," + data.PhoneTypeID2 + "\n");
            }

            return csv;
        }

        public static string ConvertCRMExtractData(List<CRMExtractVM> query)
        {
            List<ExportDataVM.EchoDataExport> echoData = query.Select(x => new ExportDataVM.EchoDataExport
            {
                PaymentTransactionID = x.PaymentTransactionID,
                TaxConsent = Convert.ToString(x.TaxConsentOptIn),
                SortCode = x.SortCode,
                AccountCode = x.AccountCode,
                CardHolder = x.CardHolder,
                ChildID = Convert.ToString(x.ChildID),
                Exported = x.Exported ? "1" : "0",
                TriggerCode = x.TriggerCode,
                BuildingNumber = x.buildingNumber,
                BuildingName = x.buildingName,
                StreetName = x.StreetName,
                AddressLine2 = x.AddressLine2,
                AddressLine3 = x.AddressLine3,
                AddressLine4 = x.AddressLine4,
                Postcode = x.Postcode,
                Country = x.Country,
                PhoneNum1 = x.PhoneNum1,
                EmailAddress = x.EmailAddress,
                Location = x.Location,
                AiValue1 = x.AiValue1,
                AiValue2 = x.AiValue2,
                DirectMailOptIn = x.DirectMailOptIn,
                EmailOptIn = x.EmailOptIn,
                PhoneOptIn = x.PhoneOptOut,
                SMSOptIn = x.SMSOptIn,
                DateOfRequest = x.DateOfRequest,
                Title = x.Title,
                Forename = x.Forename,
                Surname = x.Surname,
                ConsentStatementID = Convert.ToString(x.UKConsentStatementID),
                supporterID = Convert.ToString(x.supporterID),
                WvSupporterId = Convert.ToString(x.WvSupporterId),
                SupporterType = x.SupporterType,
                OrganisationOrGroupName = x.Organisation_GroupName,
                OrganisationOrGroupType = x.Organisation_GroupType,
                Gender = x.gender,
                YearOfBirth = x.YearOfBirth,
                AddressStructure = x.AddressStructure,
                PhoneNum2 = x.PhoneNum2,
                CommitmentAmount = Convert.ToString(x.CommitmentAmount),
                TransactionAmount = Convert.ToString(x.TransactionAmount),
                PaymentReference = x.PaymentReference,
                PreferredContinent = x.PreferredContinent,
                PreferredCountry = x.PreferredCountry,
                PreferredGender = x.PreferredGender,
                PreferredAge = x.PreferredAge,
                FundRaised = Convert.ToString(x.Fundraised),
                Comments = x.Comments,
                ResponseEntity = x.ResponseEntity,
                ScheduledPayID = x.ScheduledPayID,
                PrayerAndFamily = x.FaithAndFamily,
                DirectMailOptOut = x.DirectMailOptOut,
                EmailOptOut = x.EmailOptOut,
                SMSOptOut = x.SMSOptOut,
                RequestType = x.RequestType,
                MiddleName = x.MiddleName,
                TaxId = x.taxId,
                Dataprocessingconsent = x.Dataprocessingconsent ? "1" : "0",
                Marketingcommsconsent = x.Marketingcommsconsent ? "1" : "0",
                ExternalPaymentToken = x.ExternalPaymentToken,
                IBAN = x.IBAN,
                ProductID = x.ProductID,
                Donationamount = Convert.ToString(x.Donationamount),
                DonationvariantID = x.DonationvariantID,
                Donationfrequency = x.Donationfrequency,
                AdrStatusID = x.AdrStatusID,
                AdrTypeID = x.AdrTypeID,
                Title1Descr = x.Title1Descr,
                Title2Descr = x.Title2Descr,
                MaritalStatusDescr = x.MaritalStatusDescr,
                AddDate = x.AddDate,
                FamilyName2 = x.FamilyName2,
                OrgName1 = x.OrgName1,
                OrgName2 = x.OrgName2,
                OrgDepartmentName = x.OrgDepartmentName,
                OrgRoleDescr = x.OrgRoleDescr,
                PartnershipName = x.PartnershipName,
                StatesProvCode = x.StatesProvCode,
                RegionDescr = x.RegionDescr,
                NoMail = x.NoMail,
                NoMailID = x.NoMailID,
                SpokenLanguageCode = x.SpokenLanguageCode,
                PrintedLanguageCode = x.PrintedLanguageCode,
                ReceiptingID = x.ReceiptingID,
                MotivationID = x.MotivationID,
                ReferenceText = x.ReferenceText,
                PhoneTypeID1 = x.PhoneTypeID1,
                PhoneTypeID2 = x.PhoneTypeID2,
            }).ToList();

            string csv = string.Empty;
            if (echoData[0] != null)
            {

                foreach (var property in echoData[0].GetType().GetProperties())
                {
                    if (property.Name != "Id")
                    { csv += property.Name + ","; }

                }
                csv += "\n";
            }
            //"\"" is entered before and after strings which might contain commas to prevent those values messing up the file
            csv += string.Concat(from data in echoData
                                 select data.RequestType + ","
                                 + data.TransactionReferenceNo + ","
                                 + data.RequestReferenceNo + ","
                                 + data.DateOfRequest + ","
                                 + data.TriggerCode + ","
                                 + data.ConsentStatementID + ","
                                 + data.supporterID + ","
                                 + data.WvSupporterId + ","
                                 + data.SupporterType + ","
                                 + data.Title + ","
                                 + "\"" + data.Forename + "\"" + ","
                                 + "\"" + data.Surname + "\"" + ","
                                 + data.OrganisationOrGroupName + ","
                                 + data.OrganisationOrGroupType + ","
                                 + data.Gender + ","
                                 + data.YearOfBirth + ","
                                 + data.TaxConsent + ","
                                 + data.AddressStructure + ","
                                 + "\"" + (data.BuildingNumber.Any(char.IsDigit) ? data.BuildingNumber : "") + "\"" + ","
                                 + "\"" + (data.BuildingName.Any(char.IsDigit) ? "" : data.BuildingName) + "\"" + ","
                                 + "\"" + data.StreetName + "\"" + ","
                                 + "\"" + data.AddressLine2 + "\"" + ","
                                 + "\"" + data.AddressLine3 + "\"" + ","
                                 + "\"" + data.AddressLine4 + "\"" + ","
                                 + data.Postcode + ","
                                 + data.Country + ","
                                 + data.PhoneNum1 + ","
                                 + data.PhoneNum2 + ","
                                 + data.EmailAddress + ","
                                 + data.Location + ","
                                 + data.CommitmentAmount + ","
                                 + data.TransactionAmount + ","
                                 + data.PaymentMethod + ","
                                 + data.PaymentType + ","
                                 + data.PaymentReference + ","
                                 + data.PaymentFrequency + ","
                                 + data.PreferredDDDate + ","
                                 + data.SortCode + ","
                                 + data.AccountCode + ","
                                 + data.CardHolder + ","
                                 + data.ChildID + ","
                                 + data.PreferredContinent + ","
                                 + data.PreferredCountry + ","
                                 + data.PreferredGender + ","
                                 + data.PreferredAge + ","
                                 + data.ReceiptRequired + ","
                                 + data.FundRaised + ","
                                 + data.CommitmentCode + ","
                                 + data.PaymentTransactionID + ","
                                 + data.Comments + ","
                                 + data.ResponseEntity + ","
                                 + data.ScheduledPayID + ","
                                 + data.AiName1 + ","
                                 + data.AiValue1 + ","
                                 + data.AiName2 + ","
                                 + data.AiValue2 + ","
                                 + data.PrayerAndFamily + ","
                                 + data.DirectMailOptIn + ","
                                 + data.EmailOptIn + ","
                                 + data.PhoneOptIn + ","
                                 + data.SMSOptIn + ","
                                 + data.DirectMailOptOut + ","
                                 + data.EmailOptOut + ","
                                 + data.PhoneOptOut + ","
                                 + data.SMSOptOut + ","
                                 + data.RequestType + ","
                + data.MiddleName + "," + data.TaxId + "," + data.Dataprocessingconsent + "," + data.Marketingcommsconsent + "," + data.IBAN + ","
                + data.ProductID + "," + data.Donationamount + "," + data.DonationvariantID + "," + data.Donationfrequency + "," + data.AdrStatusID + ","
                + data.AdrTypeID + "," + data.Title1Descr + "," + data.Title2Descr + "," + data.MaritalStatusDescr + "," + data.AddDate + ","
                + data.FamilyName2 + "," + data.OrgName1 + "," + data.OrgName2 + "," + data.OrgDepartmentName + "," + data.OrgRoleDescr + "," + data.PartnershipName + ","
                + data.StatesProvCode + "," + data.RegionDescr + "," + data.NoMail + "," + data.NoMailID + "," + data.SpokenLanguageCode + ","
                + data.PrintedLanguageCode + "," + data.ReceiptingID + "," + data.MotivationID + "," + data.ReferenceText + ","
                + data.PhoneTypeID1 + "," + data.PhoneTypeID2 + "\n");
            return csv;
        }
    }
}