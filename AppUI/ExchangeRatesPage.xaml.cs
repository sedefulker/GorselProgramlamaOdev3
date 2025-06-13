using System.Collections.ObjectModel;
using AppBL;
using AppEntity;
using Microsoft.Maui.Controls;

namespace AppUI
{
	public partial class ExchangeRatesPage : ContentPage
	{
		private readonly CurrencyManager _currencyManager;
		public ObservableCollection<CurrencyModel> CurrencyList { get; set; }
		public ObservableCollection<GoldModel> GoldList { get; set; }

		public ExchangeRatesPage()
		{
			InitializeComponent();
			_currencyManager = new CurrencyManager();
			CurrencyList = new ObservableCollection<CurrencyModel>();
			GoldList = new ObservableCollection<GoldModel>();

			currencyListView.ItemsSource = CurrencyList;
			goldListView.ItemsSource = GoldList;

			LoadCurrencyData();
		}

		private async void LoadCurrencyData()
		{
			try
			{
				loadingIndicator.IsVisible = true;
				var (currencyData, goldData) = await _currencyManager.GetProcessedCurrencyData();
				loadingIndicator.IsVisible = false;

				CurrencyList.Clear();
				foreach (var item in currencyData)
					CurrencyList.Add(item);

				GoldList.Clear();
				foreach (var item in goldData)
					GoldList.Add(item);
			}
			catch (Exception ex)
			{
				loadingIndicator.IsVisible = false;
				await DisplayAlert("Hata", $"Veriler alınırken bir hata oluştu: {ex.Message}", "Tamam");
			}
		}

		

	}
}
