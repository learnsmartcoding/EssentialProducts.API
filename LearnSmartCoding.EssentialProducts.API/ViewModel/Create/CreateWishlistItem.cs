using LearnSmartCoding.EssentialProducts.API.ViewModel.Get;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.API.ViewModel.Create
{
    public class CreateWishlistItem : WishlistItemViewModel
    {
        /* Enable it if yyou need validation and return 400 Bad request
      
        */

        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
      CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();
            var wishlistService = validationContext.GetService<IWishlistItemService>();

            if (await wishlistService.IsWishlistItemExistAsync(OwnerADObjectId, ProductId))
            {
                errors.Add(new ValidationResult($"Product id {ProductId} exist for owner {OwnerADObjectId}", new[] { nameof(ProductId) }));
            }

            return errors;

        }
    }
}
