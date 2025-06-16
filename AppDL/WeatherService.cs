using System.Threading.Tasks;

namespace AppDL
{
	public static class WeatherService
	{
		public static Task<string> GetMgmUrlAsync(string city)
		{
			city = city.ToUpper()
					   .Replace("Ç", "C")
					   .Replace("Ğ", "G")
					   .Replace("İ", "I")
					   .Replace("Ö", "O")
					   .Replace("Ş", "S")
					   .Replace("Ü", "U");

			if (city == "KAHRAMANMARAS")
				city = "K.MARAS";
			else if (city == "AFYON")
				city = "AFYONKARAHISAR";

			string url = $"https://www.mgm.gov.tr/sunum/tahmin-show-2.aspx?m={city}&basla=1&bitir=5&rC=BBB&rZ=fff";
			return Task.FromResult(url);
		}
	}
}
