using LearnSmartCoding.EssentialProducts.API.ViewModel.Create;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Get;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Update;
using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnSmartCoding.EssentialProducts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet("", Name = "GetProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
        {
            var products = await productService.GetProductsAsync();
            var models = products.Select(product => new ProductViewModel()
            {
                AvailableSince = product.AvailableSince,
                CategoryId = Convert.ToInt16(product.CategoryId),
                Descriptions = product.Descriptions,
                Id = product.Id,
                IsActive = product.IsActive,
                Name = product.Name,
                Price = product.Price,
                ProductImagesViewModels = product.ProductImages.Any() ? product.ProductImages.Select
                 (s => new ProductImagesViewModel()
                 {
                     Id = s.Id,
                     Mime = s.Mime,
                     ImageName = s.ImageName,
                     ProductId = Convert.ToInt32(s.ProductId),
                     Base64Image = Convert.ToBase64String(s.Image)
                 }).ToList() : new List<ProductImagesViewModel>()
            }).ToList();
            return Ok(models);
        }

        // GET api/<ProductController>/5

        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get(int id)
        {
            var product = await productService.GetProductAndImagesAsync(id);
            if (product == null)
                return NotFound();

            var model = new ProductViewModel()
            {
                AvailableSince = product.AvailableSince,
                CategoryId = Convert.ToInt16(product.CategoryId),
                Descriptions = product.Descriptions,
                Id = product.Id,
                IsActive = product.IsActive,
                Name = product.Name,
                Price = product.Price,
                ProductImagesViewModels = product.ProductImages.Any() ? product.ProductImages.Select
                 (s => new ProductImagesViewModel()
                 {
                     Id = s.Id,
                     Mime = s.Mime,
                     ImageName = s.ImageName,
                     ProductId = Convert.ToInt32(s.ProductId),
                     Base64Image = Convert.ToBase64String(s.Image)
                 }).ToList() : new List<ProductImagesViewModel>()
            };

            return Ok(model);


        }

        // POST api/<ProductController>
        [HttpPost("", Name = "CreateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateProduct createProduct)
        {
            var entityToAdd = new Product()
            {
                Name = createProduct.Name,
                AvailableSince = createProduct.AvailableSince,
                CategoryId = createProduct.CategoryId,
                CreatedDate = DateTime.Now,
                Descriptions = createProduct.Descriptions,
                IsActive = createProduct.IsActive,
                Price = createProduct.Price
            };

            entityToAdd.ProductOwner = new ProductOwner() { OwnerADObjectId = "Admin", OwnerName = "Admin" };

            var createdProduct = await productService.CreateProductAsync(entityToAdd);
            return new CreatedAtRouteResult("Get", new { id = createdProduct.Id });
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProduct updateProduct)
        {
            var entityToupdate = await productService.GetProductAsync(updateProduct.Id);

            entityToupdate.Name = updateProduct.Name;
            entityToupdate.AvailableSince = updateProduct.AvailableSince;
            entityToupdate.CategoryId = updateProduct.CategoryId;
            entityToupdate.ModifiedDate = DateTime.Now;
            entityToupdate.ModifiedBy = "Admin";
            entityToupdate.Descriptions = updateProduct.Descriptions;
            entityToupdate.IsActive = updateProduct.IsActive;
            entityToupdate.Price = updateProduct.Price;


            var updatedProduct = await productService.UpdateProductAsync(entityToupdate);
            return Ok();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productService.GetProductAsync(id);
            if (product == null)
                return NotFound();

            var isSuccess = await productService.DeleteProductAsync(id);
            return Ok();
        }

        [HttpPost("upload/{id}", Name = "UploadProductImage")]
        [ProducesResponseType(typeof(int),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadProductImageAsync(IFormFile file, [FromRoute] int id)
        {
            if (!IsValidFile(file))
            {
                return BadRequest(new { message = "Invalid file extensions" });
            }

            byte[] fileBytes = null;
            using(var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileBytes = stream.ToArray();
            }

            var productImage  = await productService.CreateProductImageAsync(fileBytes, id, file.FileName, file.ContentType);
            return Ok(productImage.Id);
        }

        private bool IsValidFile(IFormFile file)
        {
            List<string> validFormats = new List<string>() { ".jpg", ".png", ".svg", ".jpeg" };
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return validFormats.Contains(extension);
        }
    }
}
