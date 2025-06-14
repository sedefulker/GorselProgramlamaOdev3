using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEntity;

namespace AppDL
{
	// Firebase Realtime Database üzerinde görev işlemlerini yöneten servis
	public class FirebaseTaskService
	{
		// Firebase RTDB istemcisi (Realtime Database URL ile)
		private readonly FirebaseClient _firebaseClient = new FirebaseClient("https://bsm322mobilapp-default-rtdb.firebaseio.com/");

		// Kullanıcıya ait görev listesini asenkron şekilde getirir
		public async Task<List<UserTaskModel>> GetUserTasksAsync(string userId)
		{
			// Belirtilen kullanıcı altındaki görevleri oku
			var tasks = await _firebaseClient
				.Child($"users/{userId}/tasks")
				.OnceAsync<UserTaskModel>();

			// Görev yoksa veya erişim yoksa boş liste döndürür (null dönmüyor)
			if (tasks == null || !tasks.Any())
			{
				Console.WriteLine("HATA: Kullanıcıya ait veri bulunamadı veya erişim yok!");
				return new List<UserTaskModel>(); // Boş liste döndürerek hata önleniyor
			}

			// Firebase'den gelen veriyi UserTaskModel listesine dönüştürüp döndür
			return tasks.Select(task => task.Object).ToList();
		}

		// Yeni bir kullanıcı görevini ekler
		public async Task AddUserTaskAsync(string userId, UserTaskModel task)
		{
			// Yeni görevi kullanıcı altındaki 'tasks' koleksiyonuna, benzersiz key ile kaydet
			await _firebaseClient
				.Child($"users/{userId}/tasks")
				.Child(Guid.NewGuid().ToString()) // Her görev için benzersiz ID oluşturuyoruz
				.PutAsync(task);
		}

		// Kullanıcı görevini siler (başlığa göre arama yapıyor)
		public async Task DeleteUserTaskAsync(string userId, UserTaskModel task)
		{
			// Öncelikle silinecek görevi bul (başlığa göre eşleşme)
			var toDelete = (await _firebaseClient
				.Child($"users/{userId}/tasks")
				.OnceAsync<UserTaskModel>())
				.FirstOrDefault(x => x.Object.Title == task.Title);

			// Görev varsa sil
			if (toDelete != null)
			{
				await _firebaseClient
					.Child($"users/{userId}/tasks")
					.Child(toDelete.Key)
					.DeleteAsync();
			}
		}

		// Var olan kullanıcı görevini günceller (ID ile kontrol ediyoruz)
		public async Task UpdateUserTaskAsync(string userId, UserTaskModel task)
		{
			// Firebase'den görevleri çek, ID eşleşen görevi bul
			var toUpdate = (await _firebaseClient
				.Child($"users/{userId}/tasks")
				.OnceAsync<UserTaskModel>())
				.FirstOrDefault(x => x.Object.Id == task.Id); // ID kontrolü yapılmalı!

			// Görev varsa veriyi güncelle
			if (toUpdate != null)
			{
				await _firebaseClient
					.Child($"users/{userId}/tasks")
					.Child(toUpdate.Key)
					.PutAsync(task);
			}
		}
	}
}
