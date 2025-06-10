using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;

namespace AppDL
{
	public class NewsService
	{
		private readonly HttpClient _http = new();

		public async Task<XDocument?> GetRssXml(string rssUrl)
		{
			try
			{
				var response = await _http.GetStringAsync(rssUrl);
				return XDocument.Parse(response);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("XML OKUMA HATASI: " + ex.Message);
				return null;
			}
		}
	}
}

