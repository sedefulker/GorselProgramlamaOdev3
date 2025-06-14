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
		// �ehir bilgilerini tutan koleksiyon (UI'ya ba�lanacak)
		private readonly ObservableCollection<WeatherModel> _cities = new();

		// �ehirlerin kaydedilece�i dosya ad� ve yolu
		private const string FileName = "cities.json";
		private readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, FileName);

		// Hava durumu i�lemleri i�in i� katman� nesnesi
		private readonly WeatherManager _manager = new();

		public WeatherPage()
		{
			InitializeComponent();
			// CollectionView i�in veri kayna�� ayarlan�yor
			cityList.ItemsSource = _cities;
		}

		// Sayfa her g�r�nd���nde dosyadan �ehirler y�klenir
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await LoadCitiesFromFile();
		}

		// Toolbar'daki "�ehir Ekle" butonuna bas�ld���nda yeni �ehir ekleme i�lemi
		private async void OnAddCityClicked(object sender, EventArgs e)
		{
			// Kullan�c�dan �ehir ad� istenir
			string cityName = await DisplayPromptAsync("Yeni �ehir", "�ehir ad� girin:");

			// Bo� giri� olursa i�lem iptal edilir
			if (string.IsNullOrWhiteSpace(cityName)) return;

			// Girilen �ehir i�in WeatherModel olu�turulur
			var weatherModel = await _manager.CreateCityAsync(cityName);

			// Listeye eklenir ve dosyaya kaydedilir
			_cities.Add(weatherModel);
			await SaveCitiesToFile();
		}

		// Silme butonuna bas�ld���nda ilgili �ehir listeden ��kar�l�r ve kaydedilir
		private void OnDeleteClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is WeatherModel model)
			{
				_cities.Remove(model);
				SaveCitiesToFile();
			}
		}

		// G�ncelleme butonuna bas�ld���nda kullan�c�ya bilgi verilir
		private void OnUpdateClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is WeatherModel model)
			{
				// WebView zaten URL'den g�ncellendi�i i�in burada tekrar veri �ekmeye gerek yok
				DisplayAlert("G�ncellendi", $"{model.CityName} �ehri yeniden y�klendi.", "Tamam");
			}
		}

		// �ehir listesini dosyaya JSON olarak kaydeder
		private async Task SaveCitiesToFile()
		{
			try
			{
				string json = JsonSerializer.Serialize(_cities);
				await File.WriteAllTextAsync(FilePath, json);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Hata", $"�ehirler kaydedilemedi: {ex.Message}", "Tamam");
			}
		}

		// Dosyadan �ehir listesini y�kler, varsa listeyi temizler ve yeniler
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
				await DisplayAlert("Hata", $"�ehirler y�klenemedi: {ex.Message}", "Tamam");
			}
		}
	}
}
