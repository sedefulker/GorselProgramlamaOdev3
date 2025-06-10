using AppDL;
using AppEntity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;

namespace AppBL
{
	public class NewsManager
	{
		private readonly NewsService _svc = new NewsService();

		private readonly Dictionary<string, string> _feeds = new()
		{

			["Manşet"] = "https://www.trthaber.com/manset_articles.rss",
			["Son Dakika"] = "https://www.trthaber.com/sondakika_articles.rss",
			["Türkiye"] = "https://www.trthaber.com/turkiye_articles.rss",
			["Gündem"] = "https://www.trthaber.com/gundem_articles.rss",
			["Ekonomi"] = "https://www.trthaber.com/ekonomi_articles.rss",
			["Spor"] = "https://www.trthaber.com/spor_articles.rss",
			["Bilim&Tek"] = "https://www.trthaber.com/bilim_teknoloji_articles.rss",
			["Kültür&Sanat"] = "https://www.trthaber.com/kultur_sanat_articles.rss",
			["Dünya"] = "https://www.trthaber.com/dunya_articles.rss",
			["Sağlık"] = "https://www.trthaber.com/saglik_articles.rss",
			["Yaşam"] = "https://www.trthaber.com/yasam_articles.rss",
			["Teknoloji"] = "https://www.trthaber.com/teknoloji_articles.rss",
			["Eğitim"] = "https://www.trthaber.com/egitim_articles.rss",
			["İnfografik"] = "https://www.trthaber.com/infografik_articles.rss",
			["Interaktif"] = "https://www.trthaber.com/interaktif_articles.rss",
			["Özel Haber"] = "https://www.trthaber.com/ozel_haber_articles.rss",
			["Dosya Haber"] = "https://www.trthaber.com/dosya_haber_articles.rss"
		};

		public IEnumerable<string> CategoryKeys => _feeds.Keys;

		public async Task<List<NewsModel>> GetByCategoryAsync(string key)
		{
			if (!_feeds.TryGetValue(key, out var url))
			{
				Debug.WriteLine("Kategori bulunamadı: " + key);
				return new List<NewsModel>();
			}

			return await FetchXmlAsync(url);
		}

		private async Task<List<NewsModel>> FetchXmlAsync(string url)
		{
			var result = new List<NewsModel>();
			var xml = await _svc.GetRssXml(url);
			if (xml == null)
				return result;

			var items = xml.Descendants("item");
			foreach (var item in items)
			{
				string raw = item.Element("description")?.Value ?? "";
				string text = Regex.Replace(raw, "<.*?>", " ");
				text = WebUtility.HtmlDecode(text).Trim();

				result.Add(new NewsModel
				{
					Title = item.Element("title")?.Value ?? "Başlık Yok",
					Description = text,
					Link = item.Element("link")?.Value ?? "#",
					Source = xml.Root?.Element("channel")?.Element("title")?.Value ?? "Kaynak Yok",
					ImageUrl = ExtractImageUrl(raw) ?? "https://via.placeholder.com/160"
				});
			}

			Debug.WriteLine($"{result.Count} haber yüklendi: {url}");
			return result;
		}

		// Description içinden img src bulmaya çalışır
		private string? ExtractImageUrl(string html)
		{
			var match = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"']");
			return match.Success ? match.Groups[1].Value : null;
		}
	}
}
