using LearnSmartCoding.EssentialProducts.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EssentialProductsDbContext dbContext;

        public CategoryRepository(EssentialProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(short categoryId)
        {
            // this will return entity and that is tracked
            var categoryToRemove = await dbContext.Category.FindAsync(categoryId);
            dbContext.Category.Remove(categoryToRemove);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public Task<Category> GetCategoryAsync(short categoryId)
        {
            return this.dbContext.Category.FindAsync(categoryId).AsTask();
        }

        public Task<Category> GetCategoryAsync(string name)
        {
            return this.dbContext.Category.FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower());
        }

        public Task<List<Category>> GetCategorysAsync()
        {
            return this.dbContext.Category.ToListAsync();
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            dbContext.Category.Update(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
    }
}
