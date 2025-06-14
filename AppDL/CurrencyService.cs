using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AppDL
{
	// Döviz verilerini API'den çeken servis sınıfı
	public class CurrencyService
	{
		private readonly HttpClient _httpClient;

		// HttpClient örneği oluşturulur
		public CurrencyService()
		{
			_httpClient = new HttpClient();
		}

		// Döviz verilerini JSON formatında çeker
		public async Task<JObject> GetCurrencyData()
		{
			try
			{
				// API adresi
				string url = "https://finans.truncgil.com/today.json";

				// API'den veri çekilir
				var response = await _httpClient.GetStringAsync(url);

				// JSON parse edilip döndürülür
				return JObject.Parse(response);
			}
			catch (Exception ex)
			{
				// Hata durumunda konsola yazılır ve null döner
				Console.WriteLine($"API Hatası: {ex.Message}");
				return null;
			}
		}
	}
}
