using LearnSmartCoding.EssentialProducts.API.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LearnSmartCoding.EssentialProducts.API.ViewModel.Get
{
    public class ProductViewModel : AbstractValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Name { get; set; }
        [Required]
        [MinLength(100), MaxLength(8000)]
        public string Descriptions { get; set; }
        [Range(5, 9000)]
        public decimal Price { get; set; }
        public DateTime AvailableSince { get; set; }
        public bool IsActive { get; set; }
        public short CategoryId { get; set; }
        public List<ProductImagesViewModel> ProductImagesViewModels { get; set; }
    }
}
