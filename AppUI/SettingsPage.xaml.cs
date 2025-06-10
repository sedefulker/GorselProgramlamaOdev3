using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace AppUI
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            
            // Kaydedilmiþ tema tercihine göre switch'i ayarla (varsayýlan Light olarak false)
            bool isDarkMode = Preferences.Get("dark_mode", false);
            themeSwitch.IsToggled = isDarkMode;
            
            // Sayfanýn ve uygulamanýn temasýný bu deðere göre uygula.
            ApplyTheme(isDarkMode);
        }

		private void TemaSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Preferences.Set("dark_mode", e.Value);
			App.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
		}
		private void ApplyTheme(bool isDarkMode)
		{
			if (isDarkMode)
			{
				Application.Current.Resources["PageBackgroundColor"] = Color.FromArgb("#121212");
				Application.Current.Resources["LabelTextColor"] = Colors.White;
				Application.Current.Resources["SecondaryLabelTextColor"] = Colors.Gray;
			}
			else
			{
				Application.Current.Resources["PageBackgroundColor"] = Colors.White;
				Application.Current.Resources["LabelTextColor"] = Colors.Black; // Light mode için koyu renk
				Application.Current.Resources["SecondaryLabelTextColor"] = Colors.DarkGray;
			}

			App.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
		}
	}
    }
