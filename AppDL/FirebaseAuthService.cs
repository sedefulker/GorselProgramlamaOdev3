using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AppEntity;

namespace AppDL
{
	public class FirebaseAuthService
	{
		private readonly string _webApiKey = "AIzaSyBMyT0wkfJHByp72YSsJEzjpE9SH8VnPDs"; // Buraya kendi Firebase Web API Key'ini ekle!

		public async Task<AppUser?> Register(string username, string email, string password)
		{
			try
			{
				// 🔥 Email ve şifre boş mu kontrol et
				if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
				{
					Console.WriteLine("HATA: Email veya şifre boş olamaz!");
					return null;
				}

				using var client = new HttpClient();
				var requestBody = new
				{
					email = email,
					password = password,
					returnSecureToken = true
				};

				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
				var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_webApiKey}", content);

				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Kayıt hatası: {await response.Content.ReadAsStringAsync()}");
					return null;
				}

				var responseString = await response.Content.ReadAsStringAsync();
				var firebaseResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

				Console.WriteLine($"Kayıt Başarılı! Firebase ID: {firebaseResponse.localId}");
				return new AppUser { Id = firebaseResponse.localId, Username = username, Email = email };
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Firebase Kayıt Hatası: {ex.Message}");
				return null;
			}
		}

		public async Task<AppUser?> Login(string email, string password)
		{
			try
			{
				// 🔥 Email ve şifre boş mu kontrol et
				if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
				{
					Console.WriteLine("HATA: Email veya şifre boş olamaz!");
					return null;
				}

				using var client = new HttpClient();
				var requestBody = new
				{
					email = email,
					password = password,
					returnSecureToken = true
				};

				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
				var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_webApiKey}", content);

				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Giriş hatası: {await response.Content.ReadAsStringAsync()}");
					return null;
				}

				var responseString = await response.Content.ReadAsStringAsync();
				var firebaseResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

				Console.WriteLine($"Giriş Başarılı! Firebase ID: {firebaseResponse.localId}");
				return new AppUser { Id = firebaseResponse.localId, Email = email };
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Firebase Giriş Hatası: {ex.Message}");
				return null;
			}
		}
	}
}