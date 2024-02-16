using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
	public class AuthDbContext:IdentityDbContext
	{
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        
        }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			var readerRoleId = "650622bd-22a9-4374-8563-c01d736de80d";
			var writerRoleId = "329e7985-c067-47cf-bd88-814c5df2a3bf";

			var roles = new List<IdentityRole>()
			{
				new IdentityRole() {
					Id = readerRoleId,
					Name = "Reader",
					NormalizedName="Reader".ToUpper(),
					ConcurrencyStamp = readerRoleId

				},
				new IdentityRole()
				{
					Id = writerRoleId,
					Name = "Writer",
					NormalizedName="Writer".ToUpper(),
					ConcurrencyStamp = writerRoleId
				}
			};
			builder.Entity<IdentityRole>().HasData(roles);

			var adminUserId = "6fea26cd-dc5e-4b5d-aef6-003adc18f28b";
			var admin = new IdentityUser()
			{
				Id = adminUserId,
				UserName = "admin@codepulse.com",
				Email = "admin@codepulse.com",
				NormalizedEmail = "admin@codepulse.com".ToUpper(),
				NormalizedUserName = "admin@codepulse.com".ToUpper()
			};
			admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
			builder.Entity<IdentityUser>().HasData(admin);

			var adminRoles = new List<IdentityUserRole<string>>()
			{
				new()
				{
					UserId=adminUserId,
					RoleId=readerRoleId

				},
				new()
				{
					UserId=adminUserId,
					RoleId=writerRoleId

				}
			};
			builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

		}

	}
}
