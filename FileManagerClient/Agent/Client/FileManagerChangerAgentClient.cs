using Core.Models.Requests;
using FileManagerClient.Agent.Client.Interface;
using System.Net.Http;
using System.Text.Json;
using System;
using Core.Models.Responses;

namespace FileManagerClient.Agent.Client
{
    public class FileManagerChangerAgentClient : IFileManagerChangerAgentClient
    {
        HttpClient _httpClient;

        JsonSerializerOptions _options;


        public FileManagerChangerAgentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions();
        }
        public ExceptionResponse CopyItem(CopyItemRequest request)
        {
            ExceptionResponse result = new ExceptionResponse();
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/copy" +
                $@"?pathOldItem={request.OldPath}&pathNewItem={request.NewPath}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    result = JsonSerializer.DeserializeAsync<ExceptionResponse>(responseStream, _options).Result;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.ex = e;
            }
            return result;
        }

        public ExceptionResponse CreateItem(CreateRequest request)
        {
            ExceptionResponse result = new ExceptionResponse();
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/create" +
                $@"?pathNewItem={request.PathNewItem}&typeItem={request.TypeItem}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    result = JsonSerializer.DeserializeAsync<ExceptionResponse>(responseStream, _options).Result;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.ex = e;
            }
            return result;
        }

        public ExceptionResponse DeleteItem(DeleteRequest request)
        {
            ExceptionResponse result = new ExceptionResponse();
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/delete" +
                $@"?pathDelete={request.PathDelete}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    result = JsonSerializer.DeserializeAsync<ExceptionResponse>(responseStream, _options).Result;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.ex = e;
            }
            return result;
        }

        public ExceptionResponse RenameItem(RenameItemRequest request)
        {
            ExceptionResponse result = new ExceptionResponse();
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/rename" +
                $@"?pathOldItem={request.PathOldItem}&newNameItem={request.NewNameItem}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    result = JsonSerializer.DeserializeAsync<ExceptionResponse>(responseStream, _options).Result;

                    return result;
                }
            }
            catch (Exception e)
            {

                result.ex = e;
            }
            return result;
        }
    }
}
