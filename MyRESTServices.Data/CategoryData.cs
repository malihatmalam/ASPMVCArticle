using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data
{
    public class CategoryData : ICategoryData
    {
        private readonly AppDbContext _context;
        public CategoryData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category;
        }

        public async Task<IEnumerable<Category>> GetByName(string name)
        {
            var categories = await _context.Categories.Where(c => c.CategoryName.Contains(name)).ToListAsync();
            return categories;
        }

        public async Task<int> GetCountCategories(string name)
        {
            var count = await _context.Categories.Where(c => c.CategoryName.Contains(name)).CountAsync();
            return count;
        }

        public async Task<IEnumerable<Category>> GetWithPaging(int pageNumber, int pageSize, string name)
        {
            var categories = await _context.Categories
                .Where(c => c.CategoryName.Contains(name))
                .OrderBy(c => c.CategoryName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return categories;
        }

        public async Task<Category> Insert(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> InsertWithIdentity(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task<Category> Update(int id, Category entity)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
                return null;

            existingCategory.CategoryId = entity.CategoryId;
            existingCategory.CategoryName = entity.CategoryName;

            await _context.SaveChangesAsync();
            return existingCategory;
        }
    }
}
