using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace AppUI
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
		}

		private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
		{
			// Temayý uygula
			App.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;

			
			// AppShell'i yeniden oluþtur
			Application.Current.MainPage = new AppShell();

			Shell.Current.GoToAsync("//SettingsPage", false);
		}



	}
}