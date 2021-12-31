using FileManagerClient.Agent.Client.Interface;
using FileManagerClient.Agent.Interface;
using FileManagerClient.Agent.Models.Requests;
using FileManagerClient.Agent.Models.Responses;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileManagerClient.Agent.Client
{
    public class FileManagerInformatorAgentClient : IFileManagerInformatorClient
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
        public void CopyFolder(string oldName, string newName)
        {
            throw new NotImplementedException();
        }

        public void CreateFolder()
        {
            throw new NotImplementedException();
        }

        public void CreateFolder(string nameFolder)
        {
            throw new NotImplementedException();
        }

        public void DeleteFolder(string fullName)
        {
            throw new NotImplementedException();
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

        public IEnumerable<string> GetItemsInFolder()
        {
            throw new NotImplementedException();
        }

        public void RenameFolder(string oldName, string newName)
        {
            throw new NotImplementedException();
        }

        public List<string> Search(string path, string name)
        {
            throw new NotImplementedException();
        }

        public int SizeFolder(string name)
        {
            throw new NotImplementedException();
        }        
    }
}
