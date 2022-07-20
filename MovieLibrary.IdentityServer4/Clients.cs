using IdentityServer4;
using IdentityServer4.Models;

namespace MovieLibrary.IdentityServer4
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oidcWPFApp",
                    ClientName = "WPF MovieLibrary App",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "movieLibraryApi" },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                }
            };
        }
    }
}
