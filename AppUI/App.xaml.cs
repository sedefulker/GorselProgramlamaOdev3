using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace AppUI
{
	public partial class App : Application
	{
		public App()
		{
			// Tercih edilen temayı uygula
			var theme = Preferences.Get("AppTheme", "Light");
			UserAppTheme = theme == "Dark" ? AppTheme.Dark : AppTheme.Light;
		}

		protected override Window CreateWindow(IActivationState activationState)
		{
			// Kullanıcının giriş durumunu kontrol et
			bool isLoggedIn = Preferences.Get("user_logged_in", false);
			string userId = Preferences.Get("user_id", "");

			Page startPage;

			if (isLoggedIn && !string.IsNullOrEmpty(userId))
			{
				// Giriş yapıldıysa ana sayfaya yönlendir
				startPage = new AppShell(); // Veya MainPage gibi bir sayfa
			}
			else
			{
				// Giriş yapılmamışsa LoginPage göster
				startPage = new LoginPage();
			}

			return new Window(new NavigationPage(startPage));
		}


	}
}