using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
	public interface IBlogImageRepository
	{
		Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
		Task<IEnumerable<BlogImage>> GetAll();
	}
}
