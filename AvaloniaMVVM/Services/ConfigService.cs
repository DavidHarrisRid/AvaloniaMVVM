namespace AvaloniaMVVM.Services;

public class ConfigService
{
    public string ApiUrl { get; private  set; }

    public void UpdateSettings(string apiUrl)
    {
        ApiUrl = apiUrl;
    }
}