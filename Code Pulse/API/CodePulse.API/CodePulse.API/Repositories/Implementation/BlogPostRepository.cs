using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
	public class BlogPostRepository : IBlogPostRepository
	{
		private readonly ApplicationDbContext _context;
        public BlogPostRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<BlogPost> CreateBlogPost(BlogPost blogPost)
		{
			await _context.BlogPosts.AddAsync(blogPost);
			_context.SaveChanges();
			return blogPost;
		}

		public async Task<BlogPost> UpdateBlogPost(BlogPost blogPost)
		{
			var existingBlogPost = await _context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
			if (existingBlogPost == null)
			{
				return null;
			}
			_context.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
			existingBlogPost.Categories = blogPost.Categories;
			await _context.SaveChangesAsync();
			return blogPost;
		}

		public async  Task<IEnumerable<BlogPost>> GetAllBlogPosts()
		{
			return await _context.BlogPosts.Include(x=>x.Categories).ToListAsync();
		}

		public async Task<BlogPost?> GetBlogById(Guid id)
		{
			return await _context.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id == id);
		}

		public async Task<BlogPost?> DeleteBlogPost(Guid id)
		{
			var existingBlogPost = await _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
			if (existingBlogPost != null)
			{
				 _context.BlogPosts.Remove(existingBlogPost);
				await _context.SaveChangesAsync();
				return existingBlogPost; 
			}
			return null;
		}

		public async Task<BlogPost?> GetBlogPostByUrl(string urlHandled)
		{
			return await _context.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.UrlHandled == urlHandled);
		}
	}
}
