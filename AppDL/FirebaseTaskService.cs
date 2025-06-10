using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEntity;

namespace AppDL
{
	public class FirebaseTaskService
	{
		private readonly FirebaseClient _firebaseClient = new FirebaseClient("https://bsm322mobilapp-default-rtdb.firebaseio.com/");

		public async Task<List<UserTaskModel>> GetUserTasksAsync(string userId)
		{
			var tasks = await _firebaseClient
	.Child($"users/{userId}/tasks")
	.OnceAsync<UserTaskModel>();

			if (tasks == null || !tasks.Any())
			{
				Console.WriteLine("HATA: Kullanıcıya ait veri bulunamadı veya erişim yok!");
				return new List<UserTaskModel>(); // 🔥 Hata önleme için boş liste döndürüyoruz
			}

			return tasks.Select(task => task.Object).ToList();
		}

		public async Task AddUserTaskAsync(string userId, UserTaskModel task)
		{
			await _firebaseClient.Child($"users/{userId}/tasks").Child(Guid.NewGuid().ToString()).PutAsync(task);
		}

		public async Task DeleteUserTaskAsync(string userId, UserTaskModel task)
		{
			var toDelete = (await _firebaseClient
				.Child($"users/{userId}/tasks")
				.OnceAsync<UserTaskModel>())
				.FirstOrDefault(x => x.Object.Title == task.Title);

			if (toDelete != null)
			{
				await _firebaseClient.Child($"users/{userId}/tasks").Child(toDelete.Key).DeleteAsync();
			}
		}

		public async Task UpdateUserTaskAsync(string userId, UserTaskModel task)
		{
			var toUpdate = (await _firebaseClient
				.Child($"users/{userId}/tasks")
				.OnceAsync<UserTaskModel>())
				.FirstOrDefault(x => x.Object.Title == task.Title);

			if (toUpdate != null)
			{
				await _firebaseClient.Child($"users/{userId}/tasks").Child(toUpdate.Key).PutAsync(task);
			}
		}
	}
}