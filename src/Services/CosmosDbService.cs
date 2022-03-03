namespace todo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using todo.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.Configuration;

    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
        
        public async Task AddItemAsync(Prescription item)
        {
            await this._container.CreateItemAsync<Prescription>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Prescription>(id, new PartitionKey(id));
        }

        public async Task<Prescription> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Prescription> response = await this._container.ReadItemAsync<Prescription>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            { 
                return null;
            }

        }

        public async Task<IEnumerable<Prescription>> GetItemsAsync(string queryString)
        {
            return await GetItemsAsync(new QueryDefinition(queryString));
        }

        public async Task<IEnumerable<Prescription>> GetItemsAsync(QueryDefinition queryDefinition)
        {
            var query = this._container.GetItemQueryIterator<Prescription>(queryDefinition);
            List<Prescription> results = new List<Prescription>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Prescription item)
        {
            await this._container.UpsertItemAsync<Prescription>(item, new PartitionKey(id));
        }
    }
}
