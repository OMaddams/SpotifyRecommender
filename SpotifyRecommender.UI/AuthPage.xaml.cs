using SpotifyRecommender.App;
using System.Web;
namespace SpotifyRecommender.UI;

public partial class AuthPage : ContentPage
{
    private readonly ApiCaller apicaller;

    public AuthPage(ApiCaller apicaller, Uri uri)
    {
        InitializeComponent();
        Current = this;
        this.apicaller = apicaller;
        this.uri = uri;
        webViewA.Source = Convert.ToString(uri);

    }
    public static AuthPage Current { get; private set; }

    public Uri uri { get; private set; }
    public async Task GoBack()
    {
        await Navigation.PopToRootAsync();
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        Button button = sender as Button;
        button.BackgroundColor = Colors.AliceBlue;
        GoBack();
    }

    private async void webViewA_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (!e.Url.Contains("spotify"))
        {
            var code = HttpUtility.ParseQueryString(e.Url).Get(0);
            if (code != null)
            {
                await apicaller.RequestAccessToken(code);
                await GoBack();
            }
        }
    }
}