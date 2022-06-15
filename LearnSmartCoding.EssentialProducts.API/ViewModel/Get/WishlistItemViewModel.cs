using LearnSmartCoding.EssentialProducts.API.Validation;
using System.ComponentModel.DataAnnotations;
namespace LearnSmartCoding.EssentialProducts.API.ViewModel.Get
{
    public class WishlistItemViewModel : AbstractValidatableObject
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        [MaxLength(200)]
        public string OwnerADObjectId { get; set; } = "Admin";
    }
}
