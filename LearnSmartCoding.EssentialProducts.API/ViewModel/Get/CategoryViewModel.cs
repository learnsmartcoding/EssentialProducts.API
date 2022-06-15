
using LearnSmartCoding.EssentialProducts.API.Validation;
namespace LearnSmartCoding.EssentialProducts.API.ViewModel.Get
{
    public class CategoryViewModel : AbstractValidatableObject
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
