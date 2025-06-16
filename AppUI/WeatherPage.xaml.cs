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
		private readonly ObservableCollection<WeatherModel> _cities = new();
		private const string FileName = "cities.json";
		private readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
		private readonly WeatherManager _manager = new();

		public WeatherPage()
		{
			InitializeComponent();
			cityList.ItemsSource = _cities;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await LoadCitiesFromFile();
		}

		private async void OnAddCityClicked(object sender, EventArgs e)
		{
			string cityName = await DisplayPromptAsync("Yeni Þehir", "Þehir adý girin:");

			if (string.IsNullOrWhiteSpace(cityName)) return;

			var weatherModel = await _manager.CreateCityAsync(cityName);
			_cities.Add(weatherModel);
			await SaveCitiesToFile();
		}

		private void OnDeleteClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is WeatherModel model)
			{
				_cities.Remove(model);
				SaveCitiesToFile();
			}
		}

		private void OnUpdateClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is WeatherModel model)
			{
				// WebView zaten URL'den güncellendiði için tekrar çaðýrmaya gerek yok.
				DisplayAlert("Güncellendi", $"{model.CityName} þehri yeniden yüklendi.", "Tamam");
			}
		}

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