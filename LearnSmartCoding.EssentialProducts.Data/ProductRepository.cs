using LearnSmartCoding.EssentialProducts.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly EssentialProductsDbContext dbContext;

        public ProductRepository(EssentialProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            dbContext.Product.Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<ProductImage> CreateProductImageAsync(ProductImage productImage)
        {
            var product = await GetProductAsync(Convert.ToInt32(productImage.ProductId));
            product.ProductImages = new List<ProductImage>() { productImage };

            await dbContext.SaveChangesAsync();
            return productImage;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            // this will return entity and that is tracked
            var productToRemove = await dbContext.Product.FindAsync(productId); 
            dbContext.Product.Remove(productToRemove);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public Product GetProduct(int productId)
        {
            return this.dbContext.Product.Find(productId);            
        }

        public Task<Product> GetProductAsync(int productId)
        {
            //here we did not put async/await because we dont need to await results here, we can await in service layer
            //or in countroller
            return this.dbContext.Product.FindAsync(productId).AsTask();
        }

        public Task<Product> GetProductByNameAsync(string name)
        {
            return dbContext.Product.Include(i => i.ProductImages)
                .FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower());
        }

        public Task<Product> GetProductByNameAsync(string name, int productId)
        {
            return dbContext.Product.AsNoTracking().FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower() && f.Id!=productId);
        }

        public List<Product> GetProducts(int noOfProducts)
        {
            //this will order the data and then take only specific count and returns
            var products = this.dbContext.Product
                .AsNoTracking()
                .Include(i=>i.ProductImages)
                .OrderByDescending(o=>o.CreatedDate)
                .Take(noOfProducts).ToList(); 
            return products;
        }

        public Task<List<Product>> GetProductsAsync(int noOfProducts)
        {
            var products = dbContext.Product.AsNoTracking()
                .Include(i => i.ProductImages)
                .OrderByDescending(o => o.CreatedDate)
                 .Take(noOfProducts).ToListAsync(); //this will order the data and then take only specific count and returns
            return products;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            dbContext.Product.Update(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public Task<Product> GetProductAndImagesAsync(int productId)
        {
            return dbContext.Product.AsNoTracking().Include(i => i.ProductImages).FirstOrDefaultAsync(f => f.Id == productId);
        }
        
    }
}
