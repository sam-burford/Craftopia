using Craftopia.WebSite.Models;
using Craftopia.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace Craftopia.WebSite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {

        public JsonFileProductsService ProductsService { get; }

        public ProductsController(JsonFileProductsService productsService)
        {
            ProductsService = productsService;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductsService.GetProductsOrDefault();
        }

        [Route("rate")]
        [HttpGet]
        public ActionResult Get([FromQuery] string productId, [FromQuery] int rating)
        {
            ProductsService.AddRating(productId, rating);
            return Ok();
        }

    }
}
