using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogPostController : ControllerBase
	{
		private readonly IBlogPostRepository _blogPostRepository;
		private readonly ICategoryRepository _categoryRepository;
		public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
			_blogPostRepository = blogPostRepository;
		}
		[HttpPost("AddBlogPost")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> CreateBlogPostAsync(CreateBlogPostRequestDto request)
		{
			var value = new BlogPost
			{
				Title = request.Title,
				ShortDescription = request.ShortDescription,
				Content = request.Content,
				FeaturedImageUrl = request.FeaturedImageUrl,
				UrlHandled = request.UrlHandled,
				PublishedDate = request.PublishedDate,
				Author = request.Author,
				IsVisible = request.IsVisible,
				Categories = new List<Category>()
			};
			foreach (var item in request.Categories)
			{
				var existedCategory = await _categoryRepository.GetCategoryById(item);
				if (existedCategory is not null)
				{
					value.Categories.Add(existedCategory);
				}
			}
			value = await _blogPostRepository.CreateBlogPost(value);

			var response = new BlogPostDto
			{
				Id = value.Id,
				Title = value.Title,
				ShortDescription = value.ShortDescription,
				Content = value.Content,
				FeaturedImageUrl = value.FeaturedImageUrl,
				UrlHandled = value.UrlHandled,
				PublishedDate = value.PublishedDate,
				Author = value.Author,
				IsVisible = value.IsVisible,
				Categories = value.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
			};

			return Ok(response);

		}
		[HttpGet("GetAllBlogPosts")]
		public async Task<IActionResult> GetAllBlogPosts()
		{
			var blogPosts = await _blogPostRepository.GetAllBlogPosts();

			var response = new List<BlogPostDto>();
			foreach (var blogPost in blogPosts)
			{
				response.Add(new BlogPostDto
				{
					Id = blogPost.Id,
					Title = blogPost.Title,
					ShortDescription = blogPost.ShortDescription,
					Content = blogPost.Content,
					FeaturedImageUrl = blogPost.FeaturedImageUrl,
					UrlHandled = blogPost.UrlHandled,
					PublishedDate = blogPost.PublishedDate,
					Author = blogPost.Author,
					IsVisible = blogPost.IsVisible,
					Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()


				});
			}
			return Ok(response);
		}
		[HttpGet]
		[Route("{id:Guid}")]
		public async Task<IActionResult> GetBlogById([FromRoute] Guid id)
		{
			var blogPost=await _blogPostRepository.GetBlogById(id);
			if(blogPost == null)
			{
				return NotFound();
			}
			var response = new BlogPostDto
			{
				Id = blogPost.Id,
				Title = blogPost.Title,
				ShortDescription = blogPost.ShortDescription,
				Content = blogPost.Content,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				UrlHandled = blogPost.UrlHandled,
				PublishedDate = blogPost.PublishedDate,
				Author = blogPost.Author,
				IsVisible = blogPost.IsVisible,
				Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
			};
			return Ok(response);	
			
		}

		[HttpPut]
		[Route("{id:Guid}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> UpdateBlogpost([FromRoute] Guid id ,UpdateBlogPostRequestDto request)
		{
			var blogPost = new BlogPost
			{
				Id = id,
				Title = request.Title,
				ShortDescription = request.ShortDescription,
				Content = request.Content,
				FeaturedImageUrl = request.FeaturedImageUrl,
				UrlHandled = request.UrlHandled,
				PublishedDate = request.PublishedDate,
				Author = request.Author,
				IsVisible = request.IsVisible,
				Categories = new List<Category>()

			};
			foreach(var item in request.Categories) {
				var existedCategory = await _categoryRepository.GetCategoryById(item);
				if(existedCategory is not null)
				{
					blogPost.Categories.Add(existedCategory);
				}

			}
			var updatedBlogPost = await _blogPostRepository.UpdateBlogPost(blogPost);
			if (updatedBlogPost == null)
			{
				return NotFound();
			}
			var response = new BlogPostDto
			{
				Id = updatedBlogPost.Id,
				Title = updatedBlogPost.Title,
				ShortDescription = updatedBlogPost.ShortDescription,
				Content = updatedBlogPost.Content,
				FeaturedImageUrl = updatedBlogPost.FeaturedImageUrl,
				UrlHandled = updatedBlogPost.UrlHandled,
				PublishedDate = updatedBlogPost.PublishedDate,
				Author = updatedBlogPost.Author,
				IsVisible = updatedBlogPost.IsVisible,
				Categories = updatedBlogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
			};
			return Ok(response);
		}
		[HttpDelete]
		[Route("{id:Guid}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
		{
			var removedBlogPost = await _blogPostRepository.DeleteBlogPost(id);
			if(removedBlogPost == null)
			{
				return NotFound();
			}
			var response = new BlogPostDto
			{
				Id = removedBlogPost.Id,
				Title = removedBlogPost.Title,
				ShortDescription = removedBlogPost.ShortDescription,
				Content = removedBlogPost.Content,
				FeaturedImageUrl = removedBlogPost.FeaturedImageUrl,
				UrlHandled = removedBlogPost.UrlHandled,
				PublishedDate = removedBlogPost.PublishedDate,
				Author = removedBlogPost.Author,
				IsVisible = removedBlogPost.IsVisible,
				
			};
			return Ok(response);
		}

		[HttpGet]
		[Route("{urlHandled}")]
		public async Task<IActionResult> GetBlogDetailsByUrl([FromRoute] string urlHandled)
		{
			var blogPost = await _blogPostRepository.GetBlogPostByUrl(urlHandled);
			if(blogPost is null)
			{
				return NotFound();
			}
			var response = new BlogPostDto
			{
				Id = blogPost.Id,
				Title = blogPost.Title,
				ShortDescription = blogPost.ShortDescription,
				Content = blogPost.Content,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				UrlHandled = blogPost.UrlHandled,
				PublishedDate = blogPost.PublishedDate,
				Author = blogPost.Author,
				IsVisible = blogPost.IsVisible,
				Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
			};
			return Ok(response);
		}
	}

}
