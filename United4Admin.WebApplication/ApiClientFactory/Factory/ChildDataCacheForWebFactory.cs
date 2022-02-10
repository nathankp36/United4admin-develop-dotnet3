using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using United4Admin.WebApplication.Models;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
	public class ChildDataCacheForWebFactory : IChildDataCacheForWebFactory
	{

		protected ApiClient apiClient;

		public ChildDataCacheForWebFactory()
		{
			this.apiClient = new ApiClient();
		}

		public ChildDataCacheForWebFactory(ApiClient _apiClient)
		{
			this.apiClient = _apiClient;
		}

		async Task<ApiResponse> IChildDataCacheForWebFactory.DeleteChildRecord(int Id)
		{
			try
			{
				var requestUrl = apiClient.CreateRequestUri(ChildDataCacheForWebApiUrls.DeleteChildRecord, "Id=" + Convert.ToString(Id));
				var response = await apiClient.DeleteAsync<ApiResponse>(requestUrl);
				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ChildInternalDataPagedVM> GetAllChildren(int PageNumber, int PageSize)
		{
			try
			{
				ChildInternalDataPagedVM childDataPaged = new ChildInternalDataPagedVM();
				var requestUrl = apiClient.CreateRequestUri(ChildDataCacheForWebApiUrls.GetAllRecords, "PageNumber=" + PageNumber.ToString() + "&PageSize=" + PageSize.ToString());
				var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
				childDataPaged = JsonConvert.DeserializeObject<ChildInternalDataPagedVM>(Convert.ToString(response.ResponseObject));
				return childDataPaged;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<ChildInternalDataVM> Load(int id)
		{
			try
			{
				ChildInternalDataVM result = new ChildInternalDataVM();
				var requestUrl = apiClient.CreateRequestUri(ChildDataCacheForWebApiUrls.GetChildRecordById, "Id=" + Convert.ToString(id));
				var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
				ChildInternalDataVM childInternalDataVM = JsonConvert.DeserializeObject<ChildInternalDataVM>(Convert.ToString(response.ResponseObject));
				return childInternalDataVM;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ApiResponse> ChangeStatusSponsored(UpdateChildStatusViewModel updateChildStatusViewModel)
		{
			try
			{
				var requestUrl = apiClient.CreateRequestUri(ChildDataCacheForWebApiUrls.UpdateChildSponsored);
				var response = await apiClient.PutSync<UpdateChildStatusViewModel>(requestUrl, updateChildStatusViewModel);
				return response;
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
