using LearnSmartCoding.EssentialProducts.API.ViewModel.Create;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Get;
using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {

        private readonly IWishlistItemService wishlistService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ILogger<WishlistController> Logger { get; }

        public WishlistController(ILogger<WishlistController> logger,
            IWishlistItemService wishlistService, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
            this.wishlistService = wishlistService;
            this.httpContextAccessor = httpContextAccessor;
        }




        [HttpGet("all", Name = "GetWishlists")]
        [ProducesResponseType(typeof(List<WishlistItemViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWishlistItemsAsync()
        {           
            var adObjName = "Admin";
            Logger.LogInformation($"Executing {nameof(GetWishlistItemsAsync)}");

            var wishlists = await wishlistService.GetWishlistItemsAsync(adObjName);

            var wishlistsViewModel = wishlists.Select(s => new WishlistItemViewModel()
            {
                OwnerADObjectId = s.OwnerADObjectId,
                ProductId = Convert.ToInt32(s.ProductId),
                Id = s.Id
            }).ToList();

            return Ok(wishlistsViewModel);
        }




        [HttpPost("", Name = "CreateWishlist")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostWishlistAsync([FromBody] CreateWishlistItem createWishlistItem)
        {
            Logger.LogInformation($"Executing {nameof(PostWishlistAsync)}");            
            var wishListInDB = await wishlistService.GetWishlistItemAsync("Admin",
                createWishlistItem.ProductId);

            if (wishListInDB == null)
            {
                var entity = new WishlistItem()
                {
                    OwnerADObjectId = "Admin",
                    ProductId = createWishlistItem.ProductId
                };

                var isSuccess = await wishlistService.CreateWishlistItemAsync(entity);
                return new CreatedAtRouteResult("GetWishlist",
                  new { id = entity.Id });
            }
            return new CreatedAtRouteResult("GetWishlist",
                   new { id = wishListInDB.Id });
        }




        [HttpDelete("{id}", Name = "DeleteWishlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteWishlistAsync([FromRoute] long id)
        {

            Logger.LogInformation($"Executing {nameof(DeleteWishlistAsync)}");

            var exist = await wishlistService.IsWishlistItemExistAsync(id);

            if (!exist)
                return NotFound();

            await wishlistService.DeleteWishlistItemAsync(id);

            return Ok();
        }


    }
}
