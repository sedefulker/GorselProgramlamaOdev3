using AppBL;
using AppEntity;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AppUI
{
	public partial class TaskPage : ContentPage, INotifyPropertyChanged
	{
		private readonly TaskManager _taskManager = new();
		private string _userId = string.Empty;
		public ObservableCollection<UserTaskModel> Tasks { get; set; } = new();

		public event PropertyChangedEventHandler? PropertyChanged = delegate { };

		public TaskPage()
		{
			InitializeComponent();
			_userId = Preferences.Get("user_id", "default_user");
			LoadTasks();
		}

		private async void LoadTasks()
		{
			await _taskManager.LoadUserTasksAsync(_userId);
			Tasks.Clear();
			foreach (var task in _taskManager.GetUserTaskList())
			{
				Tasks.Add(task);
			}
			RefreshUI();
		}

		public void RefreshUI()
		{
			OnPropertyChanged(nameof(Tasks));
			lstTasks.ItemsSource = null;
			lstTasks.ItemsSource = Tasks;
		}

		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async void OnTaskClicked(object sender, EventArgs e)
		{
			if (sender is Border border && border.BindingContext is UserTaskModel task)
			{
				task.IsExpanded = !task.IsExpanded;
				RefreshUI();
			}
		}

		private async void OnAddTaskClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TaskDetailPage(null));
		}

		private async void OnDeleteTaskClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is UserTaskModel task)
			{
				bool confirm = await DisplayAlert("Silme Onayı", $"Görev silinsin mi? ({task.Title})", "Evet", "Hayır");
				if (confirm)
				{
					await _taskManager.DeleteUserTaskAsync(_userId, task);
					Tasks.Remove(task);
					RefreshUI();
				}
			}
		}

		private async void OnEditTaskClicked(object sender, EventArgs e)
		{
			if (sender is Button btn && btn.CommandParameter is UserTaskModel task)
			{
				await Navigation.PushAsync(new TaskDetailPage(task));
			}
		}
		private async void OnTaskCompletedChanged(object sender, ToggledEventArgs e)
		{
			if (sender is Switch toggleSwitch && toggleSwitch.BindingContext is UserTaskModel task)
			{
				task.IsCompleted = toggleSwitch.IsToggled;

				await _taskManager.UpdateUserTaskAsync(_userId, task);
			}
		}
	}
	}
