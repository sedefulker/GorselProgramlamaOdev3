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
			// 🔥 Kullanıcının giriş durumunu kontrol et
			bool isLoggedIn = Preferences.Get("user_logged_in", false);
			string userId = Preferences.Get("user_id", "");

			// 🔥 Eğer giriş yapılmışsa, doğrudan ana sayfaya yönlendir!
			Page startPage = isLoggedIn && !string.IsNullOrEmpty(userId)
				? new AppShell() // 🚀 Eğer AppShell kullanıyorsan!
				: new NavigationPage(new LoginPage()); // 🚀 Eğer giriş yapılmamışsa LoginPage aç!

			return new Window(new NavigationPage(new LoginPage()));
		}
	}
}