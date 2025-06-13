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
			App.Current.UserAppTheme = e.Value ? AppTheme.Light : AppTheme.Dark;
		}
	}

}