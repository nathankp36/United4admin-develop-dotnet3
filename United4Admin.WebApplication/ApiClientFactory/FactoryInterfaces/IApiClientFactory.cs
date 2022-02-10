using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory
{
    public interface IApiClientFactory<T> : IDisposable
    {

        /// <summary>
        /// Load the details from Table of <T>
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<T></returns>
        Task<List<T>> LoadList();

        /// <summary>
        ///Load the details from Table of<T> based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>T</returns>
        Task<T> Load(int id);

        /// <summary>
        ///Create the new entry in Table of<T>
        /// </summary>
        /// <param name="T"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Create(T t);

        /// <summary>
        ///Update the entry in Table of<T> based on Id
        /// </summary>
        /// <param name="T"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Update(T t);

        /// <summary>
        ///Delete the entry in Table of<T> based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>no values</returns>
        Task<ApiResponse> Delete(int id);

        /// <summary>
        ///Get List of WorkflowStatus table.
        /// </summary>
        /// <param name=""></param>
        /// <returns>IList<WorkflowStatus></returns>
        Task<IList<WorkflowStatusVM>> GetWorkflowStatuses();


    }
}
