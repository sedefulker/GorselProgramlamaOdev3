using AppDL;
using AppEntity;
using System.Threading.Tasks;

namespace AppBL
{
	// Hava durumu işlemlerini yöneten iş katmanı sınıfı
	public class WeatherManager
	{
		// Verilen şehir adına göre, MGM URL'sini alıp WeatherModel oluşturur
		public async Task<WeatherModel> CreateCityAsync(string city)
		{
			// MGM sitesindeki şehre ait hava durumu URL'sini al
			string url = await WeatherService.GetMgmUrlAsync(city);

			// Şehir ve URL bilgilerini içeren yeni WeatherModel nesnesi oluştur ve döndür
			return new WeatherModel
			{
				CityName = city,
				Url = url
			};
		}
	}
}
