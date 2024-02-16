using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogImageController : ControllerBase
	{
		private readonly IBlogImageRepository _blogImageRepository;
        public BlogImageController(IBlogImageRepository blogImageRepository)
        {
            _blogImageRepository = blogImageRepository;
        }
        [HttpPost]
		public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
		{
			ValidateFileUpload(file);
			if(ModelState.IsValid)
			{
				var blogImage = new BlogImage
				{
					FileExtension = Path.GetExtension(file.FileName).ToLower(),
					Title = title,
					FileName = fileName,
					DateCreated = DateTime.Now
				}; 
				blogImage=await  _blogImageRepository.Upload(file, blogImage);

				var response = new BlogImageDto
				{
					Id = blogImage.Id,
					Title = blogImage.Title,
					DateCreated = blogImage.DateCreated,
					FileExtension = blogImage.FileExtension,
					FileName = blogImage.FileName,
					Url = blogImage.Url

				};
				return Ok(response);
			}return BadRequest(ModelState);

		}
		private void ValidateFileUpload(IFormFile file)
		{
			var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
			if (!allowedExtension.Contains(Path.GetExtension(file.FileName).ToLower()))
			{
				ModelState.AddModelError("file", "Unsupported file format");
			}
			if (file.Length > 10485760)
			{
				ModelState.AddModelError("file", "file size cannot be more than 10MB");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllImages()
		{
			var blogImages = await _blogImageRepository.GetAll();
			var response=new List<BlogImageDto>();
			foreach (var blogImage in blogImages)
			{
				response.Add(new BlogImageDto
				{
					Id = blogImage.Id,
					Title = blogImage.Title,
					DateCreated = blogImage.DateCreated,
					FileExtension = blogImage.FileExtension,
					FileName = blogImage.FileName,
					Url = blogImage.Url
				});
			}
			return Ok(response);	
		}
	}
}
