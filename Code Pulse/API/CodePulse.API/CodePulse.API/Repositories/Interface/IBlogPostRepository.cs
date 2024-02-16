using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
	public interface IBlogPostRepository
	{

		Task<BlogPost> CreateBlogPost(BlogPost blogPost);
		Task<IEnumerable<BlogPost>> GetAllBlogPosts();
		Task<BlogPost?> GetBlogById(Guid id);
		Task<BlogPost> UpdateBlogPost(BlogPost blogPost);
		Task<BlogPost?> DeleteBlogPost(Guid id);
		Task<BlogPost?> GetBlogPostByUrl(string urlHandled);
	}
}
