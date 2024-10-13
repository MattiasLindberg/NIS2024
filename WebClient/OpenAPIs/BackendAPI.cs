using Azure.Core;
using Azure.Identity;

namespace WebClient;

public partial class BackendAPI
{
    public static string ApplicationRegistrationAudience { get; set; }

    static BackendAPI()
    {
        // Should be read from configuration, but for demos this is easier
        BackendAPI.ApplicationRegistrationAudience = "api://nis2024-demo-01";
    }

    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
    {
        var credentials = new DefaultAzureCredential();
        string accessToken = credentials
            .GetToken(new TokenRequestContext(scopes: [ApplicationRegistrationAudience], parentRequestId: null) { })
            .Token;

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }
}
