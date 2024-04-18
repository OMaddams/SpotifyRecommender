namespace SpotifyRecommender.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));
        }
    }
}
