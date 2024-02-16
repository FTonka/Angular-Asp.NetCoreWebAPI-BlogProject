using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
	public class BlogImageRepository : IBlogImageRepository
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IHttpContextAccessor _httpContentAccessor;
		private readonly ApplicationDbContext _context;
		public BlogImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContentAccessor, ApplicationDbContext context)
		{
			_webHostEnvironment = webHostEnvironment;
			_httpContentAccessor = httpContentAccessor;
			_context = context;
		}

		public async Task<IEnumerable<BlogImage>> GetAll()
		{
			return await _context.BlogImages.ToListAsync();
		}

		public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
		{
			var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",$"{ blogImage.FileName}{ blogImage.FileExtension}");
			using var stream = new FileStream(localPath, FileMode.Create);
			await file.CopyToAsync(stream);

			var httpRequest = _httpContentAccessor.HttpContext.Request;
			var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
			blogImage.Url= urlPath;
			await _context.BlogImages.AddAsync(blogImage);
			await _context.SaveChangesAsync();
			return blogImage;		

		}
	}
}
