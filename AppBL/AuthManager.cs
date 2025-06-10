using System.Threading.Tasks;
using AppDL;
using AppEntity;

namespace AppBL
{
	public class AuthManager
	{
		private readonly FirebaseAuthService _firebaseAuthService;

		public AuthManager()
		{
			_firebaseAuthService = new FirebaseAuthService();
		}

		public async Task<AppUser?> RegisterUser(string username, string email, string password)
		{
			return await _firebaseAuthService.Register(username, email, password);
		}

		public async Task<AppUser?> LoginUser(string email, string password)
		{
			return await _firebaseAuthService.Login(email, password);
		}
	}
}