using Newtonsoft.Json;
using System.Net;

namespace RCHS.MS.App.Services
{
    public class ApiRequestProvider
    {
        private HttpClient _httpClient;

        public ApiRequestProvider(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("grocery.api");
        }

        public async Task<T?> Request<T>(string url)
        {
            HttpRequestMessage requestMessage = await GetDefaultRequest(url);
            requestMessage.Method = HttpMethod.Get;
            return await ProcessRequestAndResponse<T>(requestMessage);
        }

        public async Task<Tout?> RequestPost<Tin, Tout>(string url, Tin content)
        {
            HttpRequestMessage requestMessage = await GetDefaultRequest(url);
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Content = SetRequestBodyContent(content);
            return await ProcessRequestAndResponse<Tout>(requestMessage);
        }

        public async Task<T?> RequestPost<T>(string url, T content)
        {
            HttpRequestMessage requestMessage = await GetDefaultRequest(url);
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Content = SetRequestBodyContent(content);
            return await ProcessRequestAndResponse<T>(requestMessage);
        }

        public async Task<Tout?> RequestUpdate<Tin, Tout>(string url, Tin content)
        {
            HttpRequestMessage requestMessage = await GetDefaultRequest(url);
            requestMessage.Method = HttpMethod.Put;
            requestMessage.Content = SetRequestBodyContent(content);
            return await ProcessRequestAndResponse<Tout>(requestMessage);
        }

        public async Task<T?> RequestUpdate<T>(string url, T content)
        {
            HttpRequestMessage requestMessage = await GetDefaultRequest(url);
            requestMessage.Method = HttpMethod.Put;
            requestMessage.Content = SetRequestBodyContent(content);
            return await ProcessRequestAndResponse<T>(requestMessage);
        }
        //public async Task<T?> RequestDelete<T>(string url)
        //{
        //    HttpRequestMessage requestMessage = await GetDefaultRequest(url);
        //    requestMessage.Method = HttpMethod.Delete;
        //    return await ProcessRequestAndResponse<T>(requestMessage);
        //}

        private async Task<HttpRequestMessage> GetDefaultRequest(string url)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(_httpClient.BaseAddress!, url);
            return requestMessage;
        }

        private async Task<T?> ProcessRequestAndResponse<T>(HttpRequestMessage requestMessage)
        {
            try
            {
                var response = await _httpClient.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            {
                                throw new Exception("Record not found.");
                            }
                        case HttpStatusCode.BadRequest:
                            {
                                var result = await GetRequestBodyContent<string>(response.Content);
                                throw new Exception(result);
                            }
                    }
                }
                else
                    return await GetRequestBodyContent<T?>(response.Content);
                return default(T?);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private StringContent SetRequestBodyContent<T>(T content)
        {
            var jsonContent = (content != null) ? JsonConvert.SerializeObject(content) : string.Empty;
            return new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        }

        private async Task<T?> GetRequestBodyContent<T>(HttpContent httpContent)
        {
            var jsonResult = await httpContent.ReadAsStringAsync();
            var genResponse = JsonConvert.DeserializeObject<T>(jsonResult);
            return (T?)genResponse!;
        }
    }
}
