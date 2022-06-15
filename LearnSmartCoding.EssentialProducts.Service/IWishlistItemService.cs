using LearnSmartCoding.EssentialProducts.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public interface IWishlistItemService
    {
        Task<List<WishlistItem>> GetWishlistItemsAsync(string adName);
        Task<WishlistItem> CreateWishlistItemAsync(WishlistItem wishlistItem);
        Task<bool> IsWishlistItemExistAsync(long wishlistItemId);
        Task<bool> IsWishlistItemExistAsync(string ownerADObjectId, int productId);
        Task<bool> DeleteWishlistItemAsync(long id);
        Task<WishlistItem> GetWishlistItemAsync(string adObjName, int productId);
    }
}
