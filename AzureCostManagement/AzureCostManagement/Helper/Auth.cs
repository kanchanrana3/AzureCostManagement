using Azure.Core;
using Azure.Identity;

namespace AzureCostManagement.Helper
{


    public class Auth
    {
        private const string tenantId = "0fd81a0a-a77e-43a8-8b8c-xxxx";
        private const string clientId = "c35685ac-46cc-4864-9c7b-xxxxx";
        private const string clientSecret = "XQz8Q~H8UT4Ug4LNv59axxxxxxx";
        private const string scope = "https://management.azure.com/.default";
        public static  ClientSecretCredential getClientCredentials()
        {
            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            return credential;
        }

        public static async Task<string> getAuthToken()
        {
            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            // Get the bearer token
            var token = await credential.GetTokenAsync(new TokenRequestContext(new[] { scope }));
            return token.Token;
        }

    }
}
