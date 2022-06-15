using LearnSmartCoding.EssentialProducts.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public interface IProductService
    {
        Product GetProduct(int productId);
        List<Product> GetProducts(int noOfProducts = 100);
        Task<Product> GetProductAsync(int productId);
        Task<List<Product>> GetProductsAsync(int noOfProducts = 100);

        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> IsProductNameExistAsync(string name);
        Task<bool> IsProductExistAsync(string name, int productId);
        Task<ProductImage> CreateProductImageAsync(byte[] fileBytes, int productId, string imageName, string mimeType);
        Task<Product> GetProductAndImagesAsync(int productId);
    }
}
