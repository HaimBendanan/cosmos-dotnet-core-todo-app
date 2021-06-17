using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace todo.ViewComponents
{
    public class GoodListViewComponent : ViewComponent
    {
        private readonly ICosmosDbService _cosmosDbService;
        private const string demoUserId = "haimb@microsoft.com";
         public GoodListViewComponent(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            if(name == null){
                name = "";
            }
            name = Regex.Replace(name, "[^A-Za-z0-9 -]", "");
            var query = $"SELECT * FROM c WHERE c.userId='{demoUserId}' AND c.name LIKE '%{name}%'";
            return View(await _cosmosDbService.GetItemsAsync(query));
        }

    }
}