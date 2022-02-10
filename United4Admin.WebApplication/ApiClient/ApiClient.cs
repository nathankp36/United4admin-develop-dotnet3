﻿using United4Admin.WebApplication.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretToken;
        private readonly string _apiVersion;
        private bool disposed;

       
        private Uri _baseEndpoint { get; set; }

        public ApiClient()
        {            
            _baseEndpoint = new Uri(AppConfigValues.ApiBaseUrl);
            _secretToken = Convert.ToString(AppConfigValues.ApiToken);
            _apiVersion = "api-version=" + Convert.ToString(AppConfigValues.ApiVersion);
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse> GetAsync<T>(Uri requestUrl)
        {
            AddDefaultHeaders();
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(data);
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        public async Task<ApiResponse> PostAsync<T>(Uri requestUrl, T content)
        {
            AddDefaultHeaders();
            if(requestUrl.PathAndQuery.Contains("ImportChildId") || requestUrl.PathAndQuery.Contains("ExtractCRMData"))
            {
                //these processes can take an especially long time: extend timeout
                _httpClient.Timeout = TimeSpan.FromMinutes(30);
            }
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(data);
        }
        /// <summary>
        /// Common method for making POST File calls
        /// </summary>
        public async Task<ApiResponse> PostFileAsync(Uri requestUrl, MultipartFormDataContent file)
        {
            AddDefaultHeaders();            
            var response = await _httpClient.PostAsync(requestUrl.ToString(), file);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(data);
        }
        /// <summary>
        /// Common method for making PUT calls
        /// </summary>
        public async Task<ApiResponse> PutSync<T>(Uri requestUrl, T content)
        {
            AddDefaultHeaders();
            var response = await _httpClient.PutAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(data);
        }

        /// <summary>
        /// Common method for making delete calls
        /// </summary>
        public async Task<ApiResponse> DeleteAsync<T>(Uri requestUrl)
        {
            AddDefaultHeaders();
            var response = await _httpClient.DeleteAsync(requestUrl.ToString());
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponse>(data);
        }

        public Uri CreateRequestUri(string relativePath, string queryString = "")
        {           
            Uri endpoint = new Uri(_baseEndpoint, FormatUri(relativePath));
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = _apiVersion + (!string.IsNullOrEmpty(queryString) ? "&" + queryString : "");
            return uriBuilder.Uri;
        }

        public Uri CreateRequestUploadUri(string relativePath, string queryString = "")
        {
            Uri endpoint = new Uri(_baseEndpoint, FormatUri(relativePath));
            var uriBuilder = new UriBuilder(endpoint);
           // uriBuilder.Query = _apiVersion + (!string.IsNullOrEmpty(queryString) ? "&" + queryString : "");
            return uriBuilder.Uri;
        }

        private string FormatUri(string relativePath)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                               relativePath);
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        private void AddDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("Token");
            _httpClient.DefaultRequestHeaders.Add("Token", _secretToken);
        }

        /// <summary>
        ///Dispose the object used
        /// </summary>
        /// <param name=""></param>
        /// <returns>no values</returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this._httpClient.Dispose();
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
