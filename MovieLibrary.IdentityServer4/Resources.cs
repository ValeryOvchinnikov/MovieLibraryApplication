using IdentityServer4.Models;

namespace MovieLibrary.IdentityServer4
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "movieLibraryApi",
                    DisplayName = "Movie Library API",
                    Description = "test description",
                    Scopes = new List<string> {"movieLibraryApi.read", "movieLibraryApi.write" },
                    ApiSecrets = new List<Secret> { new Secret("ApiSecret".Sha256()) },
                    UserClaims = new List<string> {"role"}
                }
            };
        }
    }
}
