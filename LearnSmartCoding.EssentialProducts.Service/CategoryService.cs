using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public Task<Category> CreateCategoryAsync(Category category)
        {
            return categoryRepository.CreateCategoryAsync (category);   
        }

        public Task<bool> DeleteCategoryAsync(short categoryId)
        {
            return categoryRepository.DeleteCategoryAsync(categoryId);
        }

        public Task<Category> GetCategoryAsync(short categoryId)
        {
            return categoryRepository.GetCategoryAsync(categoryId);
        }

        public Task<List<Category>> GetCategorysAsync()
        {
            return categoryRepository.GetCategorysAsync();
        }

        public async Task<bool> IsCategoryExistAsync(string name)
        {
            var category = await categoryRepository.GetCategoryAsync (name);
            return category != null;
        }

        public Task<Category> UpdateCategoryAsync(Category category)
        {
            return categoryRepository.UpdateCategoryAsync(category);
        }
    }
}
