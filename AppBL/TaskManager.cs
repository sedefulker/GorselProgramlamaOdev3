using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppDL;
using AppEntity;

namespace AppBL
{
	public class TaskManager
	{
		private readonly FirebaseTaskService _taskService = new FirebaseTaskService();
		private ObservableCollection<UserTaskModel> _taskList = new ObservableCollection<UserTaskModel>();

		public async Task LoadUserTasksAsync(string userId)
		{
			var tasks = await _taskService.GetUserTasksAsync(userId);
			_taskList = new ObservableCollection<UserTaskModel>(tasks);
		}

		public ObservableCollection<UserTaskModel> GetUserTaskList() => _taskList;

		public async Task AddUserTaskAsync(string userId, UserTaskModel task)
		{
			await _taskService.AddUserTaskAsync(userId, task);
			_taskList.Add(task);
		}

		public async Task DeleteUserTaskAsync(string userId, UserTaskModel task)
		{
			await _taskService.DeleteUserTaskAsync(userId, task);
			_taskList.Remove(task);
		}

		public async Task UpdateUserTaskAsync(string userId, UserTaskModel task)
		{
			await _taskService.UpdateUserTaskAsync(userId, task);
		}

	}

}
