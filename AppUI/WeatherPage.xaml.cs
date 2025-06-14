using AppBL;
using AppEntity;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppUI
{
	public partial class WeatherPage : ContentPage
	{
		// Þehir bilgilerini tutan koleksiyon (UI'ya baðlanacak)
		private readonly ObservableCollection<WeatherModel> _cities = new();

		// Þehirlerin kaydedileceði dosya adý ve yolu
		private const string FileName = "cities.json";
		private readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, FileName);

		// Hava durumu iþlemleri için iþ katmaný nesnesi
		private readonly WeatherManager _manager = new();

		public WeatherPage()
		{
			InitializeComponent();
			// CollectionView için veri kaynaðý ayarlanýyor
			cityList.ItemsSource = _cities;
		}

		// Sayfa her göründüðünde dosyadan þehirler yüklenir
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await LoadCitiesFromFile();
		}

		// Toolbar'daki "Þehir Ekle" butonuna basýldýðýnda yeni þehir ekleme iþlemi
		private async void OnAddCityClicked(object sender, EventArgs e)
		{
			// Kullanýcýdan þehir adý istenir
			string cityName = await DisplayPromptAsync("Yeni Þehir", "Þehir adý girin:");

			// Boþ giriþ olursa iþlem iptal edilir
			if (string.IsNullOrWhiteSpace(cityName)) return;

			// Girilen þehir için WeatherModel oluþturulur
			var weatherModel = await _manager.CreateCityAsync(cityName);

			// Listeye eklenir ve dosyaya kaydedilir
			_cities.Add(weatherModel);
			await SaveCitiesToFile();
		}

		// Silme butonuna basýldýðýnda ilgili þehir listeden çýkarýlýr ve kaydedilir
		private void OnDeleteClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is WeatherModel model)
			{
				_cities.Remove(model);
				SaveCitiesToFile();
			}
		}

		// Güncelleme butonuna basýldýðýnda kullanýcýya bilgi verilir
		private void OnUpdateClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is WeatherModel model)
			{
				// WebView zaten URL'den güncellendiði için burada tekrar veri çekmeye gerek yok
				DisplayAlert("Güncellendi", $"{model.CityName} þehri yeniden yüklendi.", "Tamam");
			}
		}

		// Þehir listesini dosyaya JSON olarak kaydeder
		private async Task SaveCitiesToFile()
		{
			try
			{
				string json = JsonSerializer.Serialize(_cities);
				await File.WriteAllTextAsync(FilePath, json);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Hata", $"Þehirler kaydedilemedi: {ex.Message}", "Tamam");
			}
		}

		// Dosyadan þehir listesini yükler, varsa listeyi temizler ve yeniler
		private async Task LoadCitiesFromFile()
		{
			if (!File.Exists(FilePath)) return;

			try
			{
				string json = await File.ReadAllTextAsync(FilePath);
				var cities = JsonSerializer.Deserialize<ObservableCollection<WeatherModel>>(json);

				_cities.Clear();
				foreach (var city in cities)
					_cities.Add(city);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Hata", $"Þehirler yüklenemedi: {ex.Message}", "Tamam");
			}
		}
	}
}
