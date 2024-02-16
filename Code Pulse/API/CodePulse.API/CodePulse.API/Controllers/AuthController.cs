using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
			_tokenRepository = tokenRepository;
            _userManager = userManager;

        }
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
		{
			var identityUser=await _userManager.FindByEmailAsync(request.Email);
			if(identityUser is not null)
			{
				var checkPasswordResult=await _userManager.CheckPasswordAsync(identityUser, request.Password);
				if (checkPasswordResult)
				{
					var roles = await _userManager.GetRolesAsync(identityUser);
					var jwtToken =  _tokenRepository.CreateJwtToken(identityUser, roles.ToList());
					var response = new LoginResponseDto()
					{
						Email = request.Email,
						Roles = roles.ToList(),
						Token = jwtToken

					};
					return Ok(response);
				}
			}
			ModelState.AddModelError("", "Email or Password Incorrect");
			return ValidationProblem(ModelState);
		}

        [HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(RegisterRequestDto request)
		{
			var user = new IdentityUser
			{
				UserName = request.Email.Trim(),
				Email = request.Email.Trim()

			};
			var identityResult=await _userManager.CreateAsync(user,request.Password);

			if (identityResult.Succeeded)
			{
				identityResult = await _userManager.AddToRoleAsync(user, "Reader");
				if (identityResult.Succeeded)
				{
					return Ok();
				}
				else
				{
					if (identityResult.Errors.Any())
					{
						foreach (var error in identityResult.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
			}
			else
			{
				if (identityResult.Errors.Any())
				{
					foreach (var error in identityResult.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return ValidationProblem(ModelState);
		}
			


		}
	}

