using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace AppEntity
{
	public class NewsModel
	{
		public string Title { get; set; } = "Başlık Yok";
		public string Description { get; set; } = "Açıklama Yok";
		public string Link { get; set; } = "#";
		public string Source { get; set; } = "Kaynak Yok";
		public string ImageUrl { get; set; } = "https://via.placeholder.com/160";
	}
}