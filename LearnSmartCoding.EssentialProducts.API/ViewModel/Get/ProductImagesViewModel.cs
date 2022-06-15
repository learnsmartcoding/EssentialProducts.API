namespace LearnSmartCoding.EssentialProducts.API.ViewModel.Get
{
    public class ProductImagesViewModel
    {
        public int Id { get; set; }
        public string Base64Image { get; set; }
        public string Mime { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }
    }
}
