using AppBL;

namespace AppUI
{
	public partial class RegisterPage : ContentPage
	{
		private AuthManager _authManager = new AuthManager();

		public RegisterPage()
		{
			InitializeComponent();
		}

		private async void OnRegisterClicked(object sender, EventArgs e)
		{
			var user = await _authManager.RegisterUser(usernameEntry.Text, emailEntry.Text, passwordEntry.Text);
			if (user != null)
			{
				await DisplayAlert("Baþarýlý", "Kullanýcý kaydedildi!", "Tamam");
				await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
			}
			else
			{
				await DisplayAlert("Hata", "Kayýt baþarýsýz!", "Tamam");
			}
		}
	}
}