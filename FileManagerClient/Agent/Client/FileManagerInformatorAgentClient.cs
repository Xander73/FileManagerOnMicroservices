using Core.Models.Requests;
using Core.Models.Responses;
using FileManagerClient.Agent.Client.Interface;
using System;
using System.Net.Http;
using System.Text.Json;

namespace FileManagerClient.Agent.Client
{
    public class FileManagerInformatorAgentClient : IFileManagerInformatorAgentClient
    {
        HttpClient _httpClient;

        JsonSerializerOptions _options;
        //private ILogger<FileManagerAgentClient> _logger;
        public FileManagerInformatorAgentClient(HttpClient httpClient/*, ILogger<FileManagerAgentClient> logger*/)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            //_logger = logger;
        }

        public FolderAddInformationResponse AdditionalInformation(
            FolderAddInformationResponseRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerinformator/size"
                + @$"pathItem={request.PathItem}"
                );

            try
            {
                var httpResponse = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = httpResponse.Content.ReadAsStreamAsync().Result)
                {
                    var result = JsonSerializer.DeserializeAsync<string>(responseStream, _options).Result;

                    return new FolderAddInformationResponse()
                    {
                        AddInformation = result
                    };
                }
            }
            catch (Exception e)
            {

                string s = e.Message;
            }
            return new FolderAddInformationResponse();
        }

        public AllDrivesResponse GetDrives(AllDrivesRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                @$"{request.ClientBaseAddres}/api/filemanagerinformator/drives");

            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    var result = JsonSerializer.DeserializeAsync<AllDrivesResponse>(responseStream, _options).Result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
            }

            return null;
        }

        public ALLItemsResponse GetItemsInFolder(ALLItemsRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerinformator/directories?" 
                + @$"requiredDirectory={request.PathRequiredFolder}"
                );

            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    var items = JsonSerializer.DeserializeAsync<ALLItemsResponse>(responseStream, _options).Result;
                    return items;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
            }

            return new ALLItemsResponse();
        }

        public AllMyFilesResponse PostMyFiles(ALLItemsRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerinformator/files?"
                + @$"requiredDirectory={request.PathRequiredFolder}"
                );

            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    var items = JsonSerializer.DeserializeAsync<AllMyFilesResponse>(responseStream, _options).Result;
                    return items;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
            }

            return new AllMyFilesResponse();
        }

        public AllMyFoldersResponse PostMyFolders(ALLItemsRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerinformator/directories?" 
                + @$"requiredDirectory={request.PathRequiredFolder}"
                );

            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    var items = JsonSerializer.DeserializeAsync<AllMyFoldersResponse>(responseStream, _options).Result;
                    return items;
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
            }

            return new AllMyFoldersResponse();
        }

        
        public ALLItemsResponse PostSearch(SearchItemRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerinformator/search?"
                + @$"pathFolder={request.PathFolder}&searchNameItem={request.SearchNameItem}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                {
                    var item = JsonSerializer.Deserialize<ALLItemsResponse>(responseStream, _options);
                    return item;
                }
            }
            catch (Exception e )
            {

                string d = e.Message;
            }
            return new ALLItemsResponse();
        }      
    }
}
