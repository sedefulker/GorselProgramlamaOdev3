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
			// Giriş durumu kontrolü yapılır
			bool isLoggedIn = Preferences.Get("user_logged_in", false);
			string userId = Preferences.Get("user_id", "");

			Page startPage;

			if (isLoggedIn && !string.IsNullOrEmpty(userId))
			{
				// Giriş yaptıysa AppShell'e yönlendir
				startPage = new AppShell();
			}
			else
			{
				// Giriş yapılmadıysa LoginPage göster
				startPage = new LoginPage();
			}

			return new Window(new NavigationPage(startPage));
		}


	}
}