using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data.BGService
{
    public class ProductPriceChangerBGService : BackgroundService
    {
        private readonly ILogger<ProductPriceChangerBGService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ProductPriceChangerBGService(
            ILogger<ProductPriceChangerBGService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"ProductPriceChangerBGService is starting.");
            stoppingToken.Register(() =>
                logger.LogInformation($" ProductPriceChangerBGService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                bool shutDownProcess = false;

                /*
                 *retrive the staus from DB and dynamically start or stop this service without redeploying it
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var accountContext = scope.ServiceProvider.GetRequiredService<EssentialProductsDbContext>();
                    var shutDownProcessSetting = await accountContext.AppSettings.SingleOrDefaultAsync(s => s.AppSettingKey.Equals("ProductBGService"));
                    shutDownProcess = (shutDownProcessSetting != null && Convert.ToBoolean(shutDownProcessSetting.AppSettingValue));
                }

                */

                if (!shutDownProcess)
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<EssentialProductsDbContext>();
                        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                        var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();

                        var categories = await categoryRepository.GetCategorysAsync();
                        var categoryIds = categories.Select(s => s.Id).ToList();
                        var categoryToProcess = new Random().Next(1, categoryIds.Max());
                        logger.LogInformation($"Product with category id {categoryToProcess} will see a price discount for few seconds");

                       var productIds =  dbContext.Product.Where(w => w.CategoryId == categoryToProcess).AsNoTracking().Select(s => s.Id).ToList();

                        productIds.ForEach(async p =>
                        {
                            var productToUpdate = productRepository.GetProduct(p);
                            logger.LogInformation($"Product {productToUpdate.Name} current price is {productToUpdate.Price}");
                            if (productToUpdate.Price > 10 && productToUpdate.Price < 20)
                                productToUpdate.Price = productToUpdate.Price - 1;
                            else if (productToUpdate.Price > 30 && productToUpdate.Price < 50)
                                productToUpdate.Price = productToUpdate.Price - 5;
                            else if (productToUpdate.Price <10)
                                productToUpdate.Price = productToUpdate.Price + 1;
                            logger.LogInformation($"Product {productToUpdate.Name} current price updated to {productToUpdate.Price}");
                            await productRepository.UpdateProductAsync(productToUpdate);
                        });

                    }
                }


                logger.LogInformation($"ProductPriceChangerBGService task doing background work.");

                await Task.Delay(5000, stoppingToken);
            }

            logger.LogInformation($"ProductPriceChangerBGService background task is stopping.");
        }
    }
}
