using HW_8.Models;

namespace HW_8.Interfaces;

public interface ICategory
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<IEnumerable<Category>> GetCategoriesByNameAsync(string name);
    Task<Category> GetCategoryAsync(int id);
        
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Category category);
}
