using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AppDL
{
	public class CurrencyService
	{
		private readonly HttpClient _httpClient;

		public CurrencyService()
		{
			_httpClient = new HttpClient();
		}

		public async Task<JObject> GetCurrencyData()
		{
			try
			{
				string url = "https://finans.truncgil.com/today.json";
				var response = await _httpClient.GetStringAsync(url);
				return JObject.Parse(response);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"API Hatası: {ex.Message}");
				return null;
			}
		}
	}
}