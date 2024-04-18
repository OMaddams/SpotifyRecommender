namespace SpotifyRecommender.UI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void GoToAuth()
        {
            await Shell.Current.GoToAsync("AuthPage");
        }
    }
}
