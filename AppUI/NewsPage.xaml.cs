using AppBL;
using AppEntity;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppUI
{
	public partial class NewsPage : ContentPage
	{
		private readonly NewsManager _mgr = new();
		private Button? _selectedBtn;

		public NewsPage()
		{
			InitializeComponent();
			BuildCategoryBar();
		}

		// Kategori butonlar�n� dinamik olarak olu�turur
		private void BuildCategoryBar()
		{
			foreach (var category in _mgr.CategoryKeys)
			{
				var btn = new Button
				{
					Text = category,
					FontSize = 14,
					Padding = new Thickness(12, 4),
					BackgroundColor = Colors.LightGray,
					TextColor = Colors.Black,
					CornerRadius = 16,
					CommandParameter = category
				};

				btn.Clicked += CategoryClicked;
				categoryBar.Children.Add(btn);
			}

			// Varsay�lan olarak "Man�et" kategorisini y�kle
			var defaultBtn = categoryBar.Children
				.OfType<Button>()
				.FirstOrDefault(b => (string)b.CommandParameter == "Man�et");

			if (defaultBtn != null)
				CategoryClicked(defaultBtn, EventArgs.Empty);
		}

		// Kategori t�kland���nda �a�r�l�r
		private async void CategoryClicked(object? sender, EventArgs e)
		{
			if (sender is not Button btn) return;

			string key = (string)btn.CommandParameter;
			System.Diagnostics.Debug.WriteLine("Kategori t�kland�: " + key);
			await LoadCategoryAsync(key);

			// �nceki se�ili butonu s�f�rla
			if (_selectedBtn != null)
			{
				_selectedBtn.BackgroundColor = Colors.LightGray;
				_selectedBtn.TextColor = Colors.Black;
			}

			// Yeni se�ilen butonu g�ncelle
			_selectedBtn = btn;
			_selectedBtn.BackgroundColor = Colors.DarkBlue;
			_selectedBtn.TextColor = Colors.White;
		}

		// Kategoriye ait haberleri y�kler
		private async Task LoadCategoryAsync(string key)
		{
			busy.IsVisible = busy.IsRunning = true;

			var newsListData = await _mgr.GetByCategoryAsync(key);

			if (newsListData.Count == 0)
				await DisplayAlert("Uyar�", "Bu kategoriye ait haber bulunamad�.", "Tamam");

			newsList.ItemsSource = null;
			newsList.ItemsSource = newsListData;

			busy.IsRunning = busy.IsVisible = false;
		}

		// Detay butonuna t�klan�nca haber detay sayfas�n� a�ar
		private async void DetailClicked(object? sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is NewsModel news)
			{
				await Navigation.PushAsync(new NewsDetailPage(news));
			}
		}

		// Payla� butonuna t�klan�nca payla�ma ekran� a�ar
		private async void ShareClicked(object? sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is NewsModel news)
			{
				await Share.RequestAsync(new ShareTextRequest
				{
					Uri = news.Link,
					Title = news.Title
				});
			}
		}
	}
}