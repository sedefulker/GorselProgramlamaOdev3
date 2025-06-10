using AppBL;
using AppEntity;
using Microsoft.Maui.Storage;

namespace AppUI
{
    public partial class ProfilePage : ContentPage
    {
        private AuthManager _authManager = new AuthManager();

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var email = Preferences.Get("user_email", string.Empty);
            var username = Preferences.Get("user_name", "Misafir");

            emailLabel.Text = email;
            nameLabel.Text = username;

            profileImage.Source = "profile_picture.png"; // Kullanýcýnýn profil resmini ekleyebilirsin
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Preferences.Remove("token");
            await Navigation.PushAsync(new LoginPage());
        }
    }
}