using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using United4Admin.WebApplication.APIClient;


namespace United4Admin.WebApplication.ApiClientFactory
{
    public class GenericFactory
    {

    }
    //public class GenericFactory<T> : IApiClientFactory<T> where T : class, new()
    //{
    //    protected ApiClient apiClient;
    //    protected T Model;
    //    protected bool resultStatus = false;

    //    public GenericFactory()
    //    {
    //        this.apiClient = new ApiClient();
    //    }

    //    public GenericFactory(ApiClient _apiClient)
    //    {
    //        this.apiClient = _apiClient;
    //    }

    //    /// <summary>
    //    /// Load the details from Table of <T>
    //    /// </summary>
    //    /// <param name=""></param>
    //    /// <returns>List<T></returns>
    //    public virtual async Task<ApiResponse> Load(Uri requestUrl)
    //    {

    //        try
    //        {
    //            //var requestUrl = apiClient.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
    //            //"User/GetAllUsers"), "id=2");
    //            return await apiClient.GetAsync<ApiResponse>(requestUrl);
    //        }
    //        catch (Exception ex)
    //        {
    //             throw ex;
    //        }

    //    }
    //    /// <summary>
    //    /// Load the details from Table of <T>
    //    /// </summary>
    //    /// <param name=""></param>
    //    /// <returns>List<T></returns>
    //    public virtual async Task<ApiResponse> LoadListById(Uri requestUrl)
    //    {

    //        try
    //        {
    //            //var requestUrl = apiClient.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
    //            //"User/GetAllUsers"), "id=2");
    //            return await apiClient.GetAsync<ApiResponse>(requestUrl);
    //        }
    //        catch (Exception ex)
    //        {
    //             throw ex;
    //        }

    //    }

    //    /// <summary>
    //    /// Load the details from Table of <T>
    //    /// </summary>
    //    /// <param name=""></param>
    //    /// <returns>List<T></returns>
    //    public virtual async Task<ApiResponse> GetWorkflowStatuses(Uri requestUrl)
    //    {

    //        try
    //        {
    //            //var requestUrl = apiClient.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
    //            //"User/GetAllUsers"), "id=2");
    //            return await apiClient.GetAsync<ApiResponse>(requestUrl);
    //        }
    //        catch (Exception ex)
    //        {
    //             throw ex;
    //        }

    //    }



    //    /// <summary>
    //    ///Create the new entry in Table of<T>
    //    /// </summary>
    //    /// <param name="T"></param>
    //    /// <returns>no values</returns>
    //    public virtual async Task<ApiResponse> Create(Uri requestUrl, T t)
    //    {
    //        try
    //        {
    //            return await apiClient.PostAsync<T>(requestUrl, t);
    //        }
    //        catch (Exception ex)
    //        {
    //             throw ex;
    //        }

    //    }

    //    /// <summary>
    //    ///Update the entry in Table of<T> based on Id
    //    /// </summary>
    //    /// <param name="T"></param>
    //    /// <returns>no values</returns>
    //    public virtual async Task<ApiResponse> Update(Uri requestUrl, T t)
    //    {
    //        try
    //        {
    //            return await apiClient.PutSync<T>(requestUrl, t);
    //        }
    //        catch (Exception ex)
    //        {
    //             throw ex;
    //        }
    //    }
    //    /// <summary>
    //    ///Delete the entry in Table of<T> based on Id
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns>no values</returns>
    //    public virtual async Task<ApiResponse> Delete(Uri requestUrl)
    //    {
    //        try
    //        {
    //            return await apiClient.DeleteAsync<T>(requestUrl);
    //        }
    //        catch (Exception ex)
    //        {
    //             throw ex;
    //        }

    //    }
    //    private bool disposed = false;

    //    /// <summary>
    //    ///Dispose the object used
    //    /// </summary>
    //    /// <param name=""></param>
    //    /// <returns>no values</returns>
    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!this.disposed && disposing)
    //        {
    //            this.Dispose();
    //        }
    //        this.disposed = true;
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //}
}