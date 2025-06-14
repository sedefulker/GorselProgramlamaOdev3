using System.Threading.Tasks;
using AppDL;
using AppEntity;

namespace AppBL
{
	// Kullanıcı kimlik doğrulama işlemlerini yöneten sınıf
	public class AuthManager
	{
		// Firebase Authentication servisini tutan nesne
		private readonly FirebaseAuthService _firebaseAuthService;

		// Yapıcı metot: FirebaseAuthService sınıfından bir örnek oluşturulur
		public AuthManager()
		{
			_firebaseAuthService = new FirebaseAuthService();
		}

		// Yeni kullanıcı kaydı yapar
		// Parametreler: kullanıcı adı, email ve şifre
		// Dönen değer: Kayıt başarılıysa AppUser nesnesi, değilse null
		public async Task<AppUser?> RegisterUser(string username, string email, string password)
		{
			return await _firebaseAuthService.Register(username, email, password);
		}

		// Mevcut kullanıcı için giriş yapma işlemi
		// Parametreler: email ve şifre
		// Dönen değer: Giriş başarılıysa AppUser nesnesi, değilse null
		public async Task<AppUser?> LoginUser(string email, string password)
		{
			return await _firebaseAuthService.Login(email, password);
		}
	}
}
