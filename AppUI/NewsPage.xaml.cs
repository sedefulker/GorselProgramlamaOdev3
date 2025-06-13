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

		// Kategori butonlarýný dinamik olarak oluþturur
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

			// Varsayýlan olarak "Manþet" kategorisini yükle
			var defaultBtn = categoryBar.Children
				.OfType<Button>()
				.FirstOrDefault(b => (string)b.CommandParameter == "Manþet");

			if (defaultBtn != null)
				CategoryClicked(defaultBtn, EventArgs.Empty);
		}

		// Kategori týklandýðýnda çaðrýlýr
		private async void CategoryClicked(object? sender, EventArgs e)
		{
			if (sender is not Button btn) return;

			string key = (string)btn.CommandParameter;
			System.Diagnostics.Debug.WriteLine("Kategori týklandý: " + key);
			await LoadCategoryAsync(key);

			// Önceki seçili butonu sýfýrla
			if (_selectedBtn != null)
			{
				_selectedBtn.BackgroundColor = Colors.LightGray;
				_selectedBtn.TextColor = Colors.Black;
			}

			// Yeni seçilen butonu güncelle
			_selectedBtn = btn;
			_selectedBtn.BackgroundColor = Colors.DarkBlue;
			_selectedBtn.TextColor = Colors.White;
		}

		// Kategoriye ait haberleri yükler
		private async Task LoadCategoryAsync(string key)
		{
			busy.IsVisible = busy.IsRunning = true;

			var newsListData = await _mgr.GetByCategoryAsync(key);

			if (newsListData.Count == 0)
				await DisplayAlert("Uyarý", "Bu kategoriye ait haber bulunamadý.", "Tamam");

			newsList.ItemsSource = null;
			newsList.ItemsSource = newsListData;

			busy.IsRunning = busy.IsVisible = false;
		}

		// Detay butonuna týklanýnca haber detay sayfasýný açar
		private async void DetailClicked(object? sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is NewsModel news)
			{
				await Navigation.PushAsync(new NewsDetailPage(news));
			}
		}

		// Paylaþ butonuna týklanýnca paylaþma ekraný açar
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