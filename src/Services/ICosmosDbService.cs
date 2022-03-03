namespace todo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using todo.Models;
    using Microsoft.Azure.Cosmos;

    public interface ICosmosDbService
    {
        Task<IEnumerable<Prescription>> GetItemsAsync(string query);
        Task<IEnumerable<Prescription>> GetItemsAsync(QueryDefinition queryDefinition);
        Task<Prescription> GetItemAsync(string id);
        Task AddItemAsync(Prescription item);
        Task UpdateItemAsync(string id, Prescription item);
        Task DeleteItemAsync(string id);
    }
}
