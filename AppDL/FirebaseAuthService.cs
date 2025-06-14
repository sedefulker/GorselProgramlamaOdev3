using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AppEntity;

namespace AppDL
{
	// Firebase Authentication işlemlerini yapan servis sınıfı
	public class FirebaseAuthService
	{
		// Firebase Web API Key (kendi projenize göre değiştirin)
		private readonly string _webApiKey = "AIzaSyBMyT0wkfJHByp72YSsJEzjpE9SH8VnPDs";

		// Yeni kullanıcı kaydı yapar
		public async Task<AppUser?> Register(string username, string email, string password)
		{
			try
			{
				// Email ve şifre boşsa hata döner
				if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
				{
					Console.WriteLine("HATA: Email veya şifre boş olamaz!");
					return null;
				}

				using var client = new HttpClient();

				// Firebase kayıt API'sine gönderilecek istek gövdesi
				var requestBody = new
				{
					email = email,
					password = password,
					returnSecureToken = true
				};

				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

				// Firebase kayıt endpoint'ine POST isteği gönder
				var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_webApiKey}", content);

				// Başarısızsa hata mesajı yazdır ve null dön
				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Kayıt hatası: {await response.Content.ReadAsStringAsync()}");
					return null;
				}

				// Başarılı yanıtı al, parse et
				var responseString = await response.Content.ReadAsStringAsync();
				var firebaseResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

				Console.WriteLine($"Kayıt Başarılı! Firebase ID: {firebaseResponse.localId}");

				// Yeni kullanıcı bilgilerini AppUser nesnesi olarak dön
				return new AppUser { Id = firebaseResponse.localId, Username = username, Email = email };
			}
			catch (Exception ex)
			{
				// Hata durumunda mesaj yazdır ve null dön
				Console.WriteLine($"Firebase Kayıt Hatası: {ex.Message}");
				return null;
			}
		}

		// Mevcut kullanıcıyla giriş yapar
		public async Task<AppUser?> Login(string email, string password)
		{
			try
			{
				// Email ve şifre boşsa hata mesajı
				if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
				{
					Console.WriteLine("HATA: Email veya şifre boş olamaz!");
					return null;
				}

				using var client = new HttpClient();

				// Firebase giriş API'sine gönderilecek istek gövdesi
				var requestBody = new
				{
					email = email,
					password = password,
					returnSecureToken = true
				};

				var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

				// Firebase giriş endpoint'ine POST isteği gönder
				var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_webApiKey}", content);

				// Başarısızsa hata mesajı döndür
				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"Giriş hatası: {await response.Content.ReadAsStringAsync()}");
					return null;
				}

				// Başarılı yanıtı oku ve parse et
				var responseString = await response.Content.ReadAsStringAsync();
				var firebaseResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

				Console.WriteLine($"Giriş Başarılı! Firebase ID: {firebaseResponse.localId}");

				// Giriş yapan kullanıcı bilgilerini döndür
				return new AppUser { Id = firebaseResponse.localId, Email = email };
			}
			catch (Exception ex)
			{
				// Hata durumunda mesaj yazdır
				Console.WriteLine($"Firebase Giriş Hatası: {ex.Message}");
				return null;
			}
		}
	}
}
