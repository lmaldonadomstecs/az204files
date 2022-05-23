using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;

namespace msal {
    class Program {
        private const string _clientId = "8c3e1989-be65-4576-af00-2353fafb9347";
        private const string _tenantId = "13ae5b60-0d77-41d7-80b6-6f2fe1586d2f";


        public static async Task Main(string[] args) {  

            var app = PublicClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
            .WithRedirectUri("http://localhost")
            .Build();

            string[] scopes = { "user.read" };

            AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
            Console.WriteLine($"Token:\t{result.AccessToken}");

            

        }
    }
}