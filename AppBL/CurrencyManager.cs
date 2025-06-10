using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AppDL;
using AppEntity;

namespace AppBL
{
	public class CurrencyManager
	{
		private readonly CurrencyService _currencyService;

		public CurrencyManager()
		{
			_currencyService = new CurrencyService();
		}

		public async Task<(List<CurrencyModel>, List<GoldModel>)> GetProcessedCurrencyData()
		{
			var jsonData = await _currencyService.GetCurrencyData();
			if (jsonData == null) return (new List<CurrencyModel>(), new List<GoldModel>());

			var currencyList = new List<CurrencyModel>();
			var goldList = new List<GoldModel>();

			foreach (var item in jsonData.Properties())
			{
				var currencyObject = item.Value as JObject;
				if (currencyObject != null && currencyObject["Tür"]?.ToString() == "Döviz")
				{
					currencyList.Add(new CurrencyModel
					{
						Code = item.Name,
						Alis = currencyObject["Alış"]?.ToString() ?? "Veri Yok",
						Satis = currencyObject["Satış"]?.ToString() ?? "Veri Yok",
						Fark = currencyObject["Değişim"]?.ToString().Replace("%", "").Replace("-", "") ?? "0.00",
						Yon = currencyObject["Değişim"]?.ToString().Contains("-") == true ? "↓" : "↑"
					});
				}
				else if (currencyObject != null && currencyObject["Tür"]?.ToString() == "Altın")
				{
					goldList.Add(new GoldModel
					{
						Code = item.Name,
						Alis = currencyObject["Alış"]?.ToString() ?? "Veri Yok",
						Satis = currencyObject["Satış"]?.ToString() ?? "Veri Yok",
						Degisim = currencyObject["Değişim"]?.ToString() ?? "%0.00"
					});
				}
			}
			return (currencyList, goldList);
		}
	}
}