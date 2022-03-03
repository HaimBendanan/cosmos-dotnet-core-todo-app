namespace todo.Controllers
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using todo.Models;
    using Microsoft.Azure.Cosmos;

    public class ItemController : Controller
    {
        private const string demoUserId = "john.shannon@dummy.com";

        private readonly ICosmosDbService _cosmosDbService;
        public ItemController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        [ActionName("VulnerableList")]
        public async Task<IActionResult> VulnerableList(string name)
        {
            var query = $"SELECT * FROM c WHERE c.userId='{demoUserId}' AND c.name LIKE '%{name}%'";
            return View(await _cosmosDbService.GetItemsAsync(query));
        }

        [ActionName("GoodList")]
        public async Task<IActionResult> GoodList(string name)
        {
            var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.userId = @userId AND c.name LIKE @taskNameSearchPattern")
                .WithParameter("@userId", demoUserId)
                .WithParameter("@taskNameSearchPattern", $"%{name}%");
            return View(await _cosmosDbService.GetItemsAsync(queryDefinition));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Id,Name,HowToUse,IsPurchased")] Prescription item)
        {
            if (ModelState.IsValid)
            {
                item.Id = Guid.NewGuid().ToString();
                item.UserId = demoUserId;
                await _cosmosDbService.AddItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,HowToUse,IsPurchased")] Prescription item)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Prescription item = await _cosmosDbService.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Prescription item = await _cosmosDbService.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await _cosmosDbService.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetItemAsync(id));
        }
    }
}
