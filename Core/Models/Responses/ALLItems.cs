using Core.Interface;

namespace FileManagerClient.Agent.Models.Responses
{
    internal class ALLItems
    {
        IEnumerable<IItemClient> Items { get; set; }
    }
}
