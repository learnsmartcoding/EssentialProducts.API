using LearnSmartCoding.EssentialProducts.API.ViewModel.Create;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.API.ViewModel.Update
{
    public class UpdateProductAndImage: UpdateProduct
    {
        public IFormFile Image { get; set; }
    }
    public class UpdateProduct : CreateProduct
    {
        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(
            ValidationContext validationContext,
            CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();

            var productService = validationContext.GetService<IProductService>();
            var categoryService = validationContext.GetService<ICategoryService>();

            var productEntity = await productService.GetProductAsync(Id);            

            if (productEntity == null)
            {
                errors.Add(new ValidationResult($"No such product id {Id} exist", new[] { nameof(Id) }));
            }

            var productExist = await productService.IsProductExistAsync(Name, Id);
            var category = await categoryService.GetCategoryAsync(CategoryId);

            if (category == null)
            {
                errors.Add(new ValidationResult($"Category id {CategoryId} doesn't exist", new[] { nameof(CategoryId) }));
            }

            if (productExist)
            {
                errors.Add(new ValidationResult($"Product with name {Name} exist, provide a different name", new[] { nameof(Name) }));
            }
            if (Price < 5)
            {
                errors.Add(new ValidationResult($"Price cannot be less than $5. Entered price is {Price} ", new[] { nameof(Price) }));
            }



            return errors;
        }
    }
}
