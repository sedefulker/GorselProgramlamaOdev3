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

			await _taskManager.AddUserTaskAsync(_userId, _task);

			if (Application.Current.Windows.Count > 0 && Application.Current.Windows[0].Page is TaskPage taskPage)
			{
				if (!taskPage.Tasks.Contains(_task))
				{
					taskPage.Tasks.Add(_task);
				}
				taskPage.RefreshUI();
			}

			await Navigation.PopAsync();
		}

		private async void OnCancelClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}