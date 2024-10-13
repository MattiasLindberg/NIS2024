using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async void OnPostIncrease()
    {
        string url = "https://api-nis2024-01-a9ewgydcarc4h4ax.swedencentral-01.azurewebsites.net/";
        //string url = "https://localhost:7187/";

        BackendAPI api = new BackendAPI(url, new HttpClient());

        await api.SetWeatherRBACAsync(0, 20);
        //await api.SetWeatherAuthorizedAsync(20, 40);
    }
}
