using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
		[HttpPost("AddCategory")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
		{
			var category = new Category
			{
				Name = request.Name,
				UrlHandle = request.UrlHandle,
			};
			await _categoryRepository.CreateCategory(category);

			var response = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};
			return Ok(response);
			
		}

		[HttpGet("GetAllCategories")]
		public async Task<IActionResult> GetAllCategories()
		{
		
			var categories=await _categoryRepository.GetAllCategories();
			
			var response=new List<CategoryDto>();
			foreach (var category in categories)
			{
				response.Add(new CategoryDto { Id = category.Id, Name = category.Name,UrlHandle=category.UrlHandle });
			}
			
			return Ok(response);
		}
		[HttpGet("GetById/{id}")]
		
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var value=await _categoryRepository.GetCategoryById(id);
			if (value==null)
			{
				return NotFound();
			}
			var response = new CategoryDto
			{
				Id = value.Id,
				Name = value.Name,
				UrlHandle = value.UrlHandle

			};
			return Ok(response );
		}

		[HttpPut("UpdateCategory/{id}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequestDto request)
		{
			var category = new Category
			{
				Id = id,
				Name = request.Name,
				UrlHandle = request.UrlHandle
			};
			await _categoryRepository.UpdateCategory(category);
			var response=new CategoryDto { Id = category.Id,Name = category.Name, UrlHandle = category.UrlHandle };
			if (response == null)
			{
				return NotFound();

			}
			return Ok(response);
		}
		[HttpDelete("DeleteCategory/{id}")]
		[Authorize(Roles ="Writer")]
		public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
		{
			var value=await _categoryRepository.DeleteCategory(id);
			if(value==null)
			{
				return NotFound();
			}
			var response = new CategoryDto
			{
				Id = value.Id,
				Name = value.Name,
				UrlHandle = value.UrlHandle
			};
			return Ok(response);
		}
	
	
	}
}
