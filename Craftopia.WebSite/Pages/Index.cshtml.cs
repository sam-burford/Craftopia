using Craftopia.WebSite.Models;
using Craftopia.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Craftopia.WebSite.Pages
{
    public class IndexModel : PageModel
    {
        public JsonFileProductsService ProductService;
        public IEnumerable<Product> Products { get; private set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, JsonFileProductsService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        public void OnGet()
        {
            Products = ProductService.GetProducts();
        }
    }
}