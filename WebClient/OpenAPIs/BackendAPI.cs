using Azure.Core;
using Azure.Identity;

namespace WebClient;

public partial class BackendAPI
{
    public static string? ApplicationRegistrationAudience { get; set; }

    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
    {
        //var credentials = new DefaultAzureCredential();
        //string accessToken = credentials
        //    .GetToken(new TokenRequestContext(scopes: [ApplicationRegistrationAudience!], parentRequestId: null) { })
        //    .Token;

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }
}
