using Core.Models.Requests;
using FileManagerClient.Agent.Client.Interface;
using System.Net.Http;
using System.Text.Json;
using System;

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
        public void CopyItem(CopyItemRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/copy" +
                $@"?pathOldItem={request.OldPath}&pathNewItem={request.NewPath}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public void CreateItem(CreateRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/create" +
                $@"?pathNewItem={request.PathNewItem}&typeItem={request.TypeItem}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
        }

        public void DeleteItem(DeleteRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/delete" +
                $@"?pathDelete={request.PathDelete}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
            }
            catch (Exception e)
            {
                string s = e.Message;   
            }
        }

        public void RenameItem(RenameItemRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                @$"{request.ClientBaseAddres}/api/filemanagerchanger/rename" +
                $@"?pathOldItem={request.PathOldItem}&newNameItem={request.NewNameItem}");
            try
            {
                var response = _httpClient.SendAsync(httpRequest).Result;
            }
            catch (Exception e)
            {

                string s = e.Message;
            }
        }
    }
}
