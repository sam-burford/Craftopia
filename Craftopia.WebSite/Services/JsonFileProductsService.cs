using Craftopia.WebSite.Models;
using System.Text.Json;

namespace Craftopia.WebSite.Services
{
    public class JsonFileProductsService
    {

        public JsonFileProductsService(IWebHostEnvironment webHostEnvironment) 
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");

        private string ReadJsonFile()
        {
            var json = "";

            try
            {
				using var jsonFileReader = File.OpenText(JsonFileName);
				json = jsonFileReader.ReadToEnd();
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return json;
		}

        private IEnumerable<Product>? ParseProducts(string jsonString)
        {
			return JsonSerializer.Deserialize<Product[]>(jsonString,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			);
		}

        /// <summary>
        /// Retrieves a list of Products from the JSON file. 
        /// </summary>
        /// <returns>A list of products or an empty list. </returns>
        public IEnumerable<Product> GetProductsOrDefault()
        {
            string json = ReadJsonFile();
            IEnumerable<Product>? products = ParseProducts(json);

            // Null-condition operator - will assign the products variable only if it is null. 
            products ??= Array.Empty<Product>();

            return products;
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetProductsOrDefault();
            var query = products.First(x => x.Id == productId);

            if (query.Ratings == null)
            {
                query.Ratings = new int[] { rating };
            }
            else
            {
                var ratingList = query.Ratings.ToList();
                ratingList.Add(rating);
                query.Ratings = ratingList.ToArray();
            }

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<Product>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }), products);
            }
        }
    }
}
