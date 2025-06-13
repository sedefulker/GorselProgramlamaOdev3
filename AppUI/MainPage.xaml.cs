namespace AppUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			LoadUserData();
			BindingContext = this;
		}

		private void LoadUserData()
		{
			var name = Preferences.Get("user_name", "Sedef ÜLKER");
			var studentNumber = Preferences.Get("student_number", "22010310075");

			nameLabel.Text = $"Ad Soyad: {name}";
			studentNumberLabel.Text = $"Öğrenci No: {studentNumber}";
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			Preferences.Clear(); // tüm kullanıcı bilgilerini sil
			Application.Current.MainPage = new NavigationPage(new LoginPage());
		}

		
	}
}