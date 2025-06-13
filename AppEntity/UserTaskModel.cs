using System;
using System.ComponentModel;

namespace AppEntity
{
	public class UserTaskModel : INotifyPropertyChanged
	{
		public string Id { get; set; } = string.Empty;
		private string _title = string.Empty;
		private string _details = string.Empty;
		private DateTime _dateTime = DateTime.Now;
		private bool _isCompleted;
		private bool _isExpanded;

		public event PropertyChangedEventHandler? PropertyChanged = delegate { };

		public string Title
		{
			get => _title;
			set { _title = value; OnPropertyChanged(nameof(Title)); }
		}

		public string Details
		{
			get => _details;
			set { _details = value; OnPropertyChanged(nameof(Details)); }
		}

		public DateTime DateTime
		{
			get => _dateTime;
			set { _dateTime = value; OnPropertyChanged(nameof(DateTime)); }
		}

		public bool IsCompleted
		{
			get => _isCompleted;
			set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
		}

		public bool IsExpanded
		{
			get => _isExpanded;
			set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
		}

		public UserTaskModel(string title, string details, DateTime dateTime, bool isCompleted)
		{
			Title = title;
			Details = details;
			DateTime = dateTime;
			IsCompleted = isCompleted;
		}

		public UserTaskModel() { }

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
