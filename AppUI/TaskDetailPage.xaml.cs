using AppBL;
using AppEntity;
using Microsoft.Maui.Controls;
using System;

namespace AppUI
{
	public partial class TaskDetailPage : ContentPage
	{
		private readonly TaskManager _taskManager = new();
		private UserTaskModel _task;
		private string _userId = string.Empty;

		public TaskDetailPage(UserTaskModel task)
		{
			InitializeComponent();
			_userId = Preferences.Get("user_id", "default_user");

			_task = task ?? new UserTaskModel("", "", DateTime.Now, false);

			titleEntry.Text = _task.Title;
			detailsEditor.Text = _task.Details;
			datePicker.Date = _task.DateTime.Date;
			timePicker.Time = _task.DateTime.TimeOfDay;
		}

		private async void OnSaveTaskClicked(object sender, EventArgs e)
		{
			_task.Title = titleEntry.Text;
			_task.Details = detailsEditor.Text;
			_task.DateTime = datePicker.Date.Add(timePicker.Time);

			if (string.IsNullOrEmpty(_task.Id)) //  Eğer yeni görevse, ekle!
			{
				_task.Id = Guid.NewGuid().ToString();
				await _taskManager.AddUserTaskAsync(_userId, _task);
			}
			else //  Eğer var olan görevse, güncelle!
			{
				await _taskManager.UpdateUserTaskAsync(_userId, _task);
			}

			// Eğer `TaskPage` açıksa, görevleri yeniden yükle!
			if (Application.Current.MainPage is NavigationPage navPage && navPage.CurrentPage is TaskPage taskPage)
			{
				await taskPage.LoadTasks();
			}

			await Navigation.PopAsync();
		}

		private async void OnCancelClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}
