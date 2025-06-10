using AppEntity;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System;

namespace AppUI
{
	public partial class NewsDetailPage : ContentPage
	{
		private readonly NewsModel _news;

		public NewsDetailPage(NewsModel news)
		{
			InitializeComponent();
			_news = news;
			BindingContext = _news;

			// WebView'e haberin linkini y�kle
			webView.Source = _news.Link;
		}

		// Toolbar'daki Payla� simgesine t�klan�nca �al���r
		private async void OnShareClicked(object sender, EventArgs e)
		{
			if (_news == null || string.IsNullOrWhiteSpace(_news.Link))
				return;

			await Share.RequestAsync(new ShareTextRequest
			{
				Uri = _news.Link,
				Title = _news.Title
			});
		}
	}
}
