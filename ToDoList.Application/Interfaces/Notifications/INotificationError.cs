namespace ToDoList.Application.Interfaces.Notifications;

public interface INotificationError
{
	bool HasErrors { get; }
	IReadOnlyCollection<string> Errors { get; }
	void AddError(string message);
	void Clear();
}
