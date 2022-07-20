using IdentityServer4.Models;

namespace MovieLibrary.IdentityServer4
{
    public class Scopes
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
            new ApiScope("movieLibraryApi", "Read Access to Movie Library API"),
        };
        }
    }
}
