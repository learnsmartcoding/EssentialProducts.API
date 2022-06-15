using LearnSmartCoding.EssentialProducts.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly EssentialProductsDbContext dbContext;

        public WishlistRepository(EssentialProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<WishlistItem> CreateWishlistItemAsync(WishlistItem wishlistItem)
        {
            dbContext.WishlistItem.Add(wishlistItem);
            await dbContext.SaveChangesAsync();
            return wishlistItem;
        }

        public async Task<bool> DeleteWishlistItemAsync(long id)
        {
            var entityToDelete = await dbContext.WishlistItem.FindAsync(id);
            dbContext.WishlistItem.Remove(entityToDelete);
            return await dbContext.SaveChangesAsync()>0;
        }

        public Task<WishlistItem> GetWishlistItemAsync(string adObjName, int productId)
        {
            return dbContext.WishlistItem.FirstOrDefaultAsync(f => f.ProductId == productId
            && f.OwnerADObjectId == adObjName);
        }

        public Task<List<WishlistItem>> GetWishlistItemsAsync(string adObjName = "Admin")
        {
            return dbContext.WishlistItem.Where(w => w.OwnerADObjectId == adObjName).ToListAsync();
        }

        public async Task<bool> IsWishlistItemExistAsync(long wishlistItemId)
        {
            var entity = await dbContext.WishlistItem.FindAsync(wishlistItemId);
            return entity != null;
        }

        public async Task<bool> IsWishlistItemExistAsync(string ownerADObjectId, int productId)
        {
            var wishlistItem = await dbContext.WishlistItem.FirstOrDefaultAsync(f => f.ProductId == productId && f.OwnerADObjectId == ownerADObjectId);
            return wishlistItem != null;
        }
    }
}
