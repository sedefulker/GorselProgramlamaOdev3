using AppBL;
using AppEntity;
using System.Collections.ObjectModel;

namespace AppUI
{
	public partial class WeatherPage : ContentPage
	{
		private readonly WeatherManager _manager = new();
		public ObservableCollection<WeatherModel> Cities { get; set; } = new();

		public WeatherPage()
		{
			InitializeComponent();
			cityList.ItemsSource = Cities;
		}

		private async void OnAddCityClicked(object sender, EventArgs e)
		{
			string city = await DisplayPromptAsync("Þehir Ekle", "Tahmin almak istediðiniz þehri giriniz:");
			if (!string.IsNullOrWhiteSpace(city))
			{
				var model = await _manager.CreateCityAsync(city.Trim());
				Cities.Add(model);
			}
		}

		private async void OnUpdateClicked(object sender, EventArgs e)
		{
			if ((sender as Button)?.CommandParameter is WeatherModel oldModel)
			{
				var updated = await _manager.CreateCityAsync(oldModel.CityName);
				int index = Cities.IndexOf(oldModel);
				if (index >= 0)
				{
					Cities.RemoveAt(index);
					Cities.Insert(index, updated);
				}
			}
		}

		private void OnDeleteClicked(object sender, EventArgs e)
		{
			if ((sender as Button)?.CommandParameter is WeatherModel model)
			{
				Cities.Remove(model);
			}
		}
	}
}
