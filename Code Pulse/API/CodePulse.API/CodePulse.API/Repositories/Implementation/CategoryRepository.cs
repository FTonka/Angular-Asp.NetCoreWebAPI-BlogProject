using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;	
        }
        public async Task<Category> CreateCategory(Category category)
		{
			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return category;
		}

		public async Task<Category?> DeleteCategory(Guid id)
		{
			var exitedCategory=await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
			if (exitedCategory != null)
			{
				 _context.Categories.Remove(exitedCategory);
				await _context.SaveChangesAsync();
				return exitedCategory;
			}
			return null;
		}

		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			return await _context.Categories.ToListAsync();

		}

		public async Task<Category?> GetCategoryById(Guid id)
		{
			return await _context.Categories.FirstOrDefaultAsync(x=>x.Id==id);
			
		}

		public async Task<Category?> UpdateCategory(Category category)
		{
			var value=await _context.Categories.FirstOrDefaultAsync(x=>x.Id== category.Id);
			
			if (value!=null)
			{
				_context.Entry(value).CurrentValues.SetValues(category);
				await _context.SaveChangesAsync();
				return category;
			}
			return null;
		}
	}
}
