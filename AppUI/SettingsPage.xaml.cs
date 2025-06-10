using Microsoft.Maui.Storage;

namespace AppUI
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
			themeSwitch.IsToggled = Preferences.Get("dark_mode", false);
			UpdateTheme(themeSwitch.IsToggled);
		}

		private void OnThemeChanged(object sender, ToggledEventArgs e)
		{
			Preferences.Set("dark_mode", e.Value);
			UpdateTheme(e.Value);
		}

		private void UpdateTheme(bool isDarkMode)
		{
			Application.Current.Resources["PageBackgroundColor"] = isDarkMode ? Color.FromArgb("#121212") : Colors.White;
			Application.Current.Resources["LabelTextColor"] = isDarkMode ? Colors.White : Colors.Black;
		}
	}
}