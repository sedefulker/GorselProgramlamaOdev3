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
			var name = Preferences.Get("user_name", "Misafir");
			var studentNumber = Preferences.Get("student_number", "Öğrenci Numarası Yok");

			nameLabel.Text = $"Ad Soyad: {name}";
			studentNumberLabel.Text = $"Öğrenci No: {studentNumber}";
		}

		
	}
}