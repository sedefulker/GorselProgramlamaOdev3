using System.Threading.Tasks;

namespace AppDL
{
	public static class WeatherService
	{
		// Şehir isimlerinde Türkçe karakterleri İngilizce karakterlere dönüştürür
		public static Task<string> GetMgmUrlAsync(string city)
		{
			city = city.ToUpper()
					   .Replace("Ç", "C")
					   .Replace("Ğ", "G")
					   .Replace("İ", "I")
					   .Replace("Ö", "O")
					   .Replace("Ş", "S")
					   .Replace("Ü", "U");

			// Bazı şehir isimleri MGM sitesinde farklı yazılıyor, onları düzelt
			if (city == "KAHRAMANMARAS")
				city = "K.MARAS";
			else if (city == "AFYON")
				city = "AFYONKARAHISAR";

			// MGM sitesinden hava durumu tahmin sayfasının URL'sini oluştur
			string url = $"https://www.mgm.gov.tr/sunum/tahmin-show-2.aspx?m={city}&basla=1&bitir=5&rC=BBB&rZ=fff";

			// URL'yi Task olarak geri döndür
			return Task.FromResult(url);
		}
	}
}
