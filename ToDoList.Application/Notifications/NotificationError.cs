using ToDoList.Application.Interfaces.Notifications;

namespace ToDoList.Application.Notifications;

public class NotificationError : INotificationError
{
	private readonly List<string> _errors = [];

	public bool HasErrors => _errors.Any();
	public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

	public void AddError(string message)
	{
		_errors.Add(message);
	}

	public void Clear()
	{
		_errors.Clear();
	}
}