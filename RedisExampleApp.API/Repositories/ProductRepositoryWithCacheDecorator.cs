using RedisExampleApp.API.Models;
using RedisExampleApp.API.RedisServices;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private const string productKey = "productCaches";
        private readonly IDatabase _cacheRepository;
        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository,
                                                   RedisService redisService,
                                                   IDatabase database
                                                   )
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = database;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadCacheFromDbAsync();

            var productsList = new List<Product>();

            var cacheProducts = (await _cacheRepository.HashGetAllAsync(productKey)).ToList();
            foreach (var item in cacheProducts)
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);

                productsList.Add(product);
            }

            return productsList;
        }

        public async Task<Product> GetByIdAsync(int id)
        {

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);

                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);

        }

        private async Task<List<Product>> LoadCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return products;
        }
    }
}
