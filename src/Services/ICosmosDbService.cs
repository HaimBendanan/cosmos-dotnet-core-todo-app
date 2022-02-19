namespace todo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using todo.Models;
    using Microsoft.Azure.Cosmos;

    public interface ICosmosDbService
    {
        Task<IEnumerable<Item>> GetItemsAsync(string query);
        Task<IEnumerable<Item>> GetItemsAsync(QueryDefinition queryDefinition);
        Task<Item> GetItemAsync(string id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(string id, Item item);
        Task DeleteItemAsync(string id);
    }
}
