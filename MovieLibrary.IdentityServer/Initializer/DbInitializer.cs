using IdentityModel;
using Microsoft.AspNetCore.Identity;
using MovieLibrary.IdentityServer.Data;
using MovieLibrary.IdentityServer.Models;
using System.Security.Claims;

namespace MovieLibrary.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._db = dbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByIdAsync(Config.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(Config.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Config.Customer)).GetAwaiter().GetResult();
            }
            else return;

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "11111111",
                FirstName = "Ben",
                LastName = "Admin"
            };

            _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, Config.Admin).GetAwaiter().GetResult();

            var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{adminUser.FirstName} {adminUser.LastName}"),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, Config.Admin)
            }).Result;


            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "11111111",
                FirstName = "Ben",
                LastName = "User"
            };

            _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, Config.Customer).GetAwaiter().GetResult();

            var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{customerUser.FirstName} {customerUser.LastName}"),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, Config.Customer)
            }).Result;
        }
    }
}

