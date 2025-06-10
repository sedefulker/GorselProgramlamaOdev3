using AppDL;
using AppEntity;
using System.Threading.Tasks;

namespace AppBL
{
	public class WeatherManager
	{
		public async Task<WeatherModel> CreateCityAsync(string city)
		{
			string url = await WeatherService.GetMgmUrlAsync(city);
			return new WeatherModel
			{
				CityName = city,
				Url = url
			};
		}
	}
}
