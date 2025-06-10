namespace AppUI
{
	public partial class App : Application
	{
		protected override Window CreateWindow(IActivationState activationState)
		{
			// 🔥 Her açılışta giriş zorunlu olacak!
			Preferences.Set("user_logged_in", false);
			Preferences.Set("user_id", "");

			return new Window(new NavigationPage(new LoginPage())); // 🔥 Uygulama açıldığında giriş ekranı göster!
		}
	}
}