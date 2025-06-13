using AppBL;
using Microsoft.Maui.Controls;

namespace AppUI
{
	public partial class LoginPage : ContentPage
	{
		private AuthManager _authManager = new AuthManager();

		public LoginPage()
		{
			InitializeComponent();
		}

		private async void OnLoginClicked(object sender, EventArgs e)
		{
			var email = loginEmailEntry.Text?.Trim();
			var password = loginPasswordEntry.Text?.Trim();

			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				await DisplayAlert("Hata", "Email ve şifre boş bırakılamaz!", "Tamam");
				return;
			}

			var user = await _authManager.LoginUser(email, password);
			if (user != null)
			{
				await DisplayAlert("Başarılı", "Giriş yapıldı!", "Tamam");

				// 🔥 Kullanıcının giriş durumunu kaydet
				Preferences.Set("user_logged_in", true);
				Preferences.Set("user_id", user.Id);

				// 🔥 Kullanıcı giriş yaptıktan sonra uygulamanın ana kısmına yönlendir!
				Application.Current.MainPage = new AppShell(); // 🚀 Eğer AppShell kullanıyorsan!
															   // Application.Current.MainPage = new MainPage(); // 🚀 Eğer Ana Sayfa kullanıyorsan!
			}
			else
			{
				await DisplayAlert("Hata", "Giriş başarısız!", "Tamam");
			}
		}

		private async void OnNavigateToRegister(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegisterPage()); //  Navigation içinde çalışıyor!
		}
	}
}