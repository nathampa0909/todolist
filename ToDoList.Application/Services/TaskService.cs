using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces.Notifications;
using ToDoList.Application.Interfaces.Repository;
using ToDoList.Application.Interfaces.Service;
using ToDoList.Core.Enums;

namespace ToDoList.Application.Services;

public class TaskService(INotificationError notificationError, ITaskRepository taskRepository, IUserService userService) : ITaskService
{
	private readonly INotificationError _notificationError = notificationError;
	private readonly ITaskRepository _taskRepository = taskRepository;
	private readonly IUserService _userService = userService;
	private const int MAX_SIZE_TITLE = 50;
	private const int MAX_SIZE_DESCRIPTION = 500;

	public async Task<Core.Entities.Task> CreateNewTaskAsync(DTONewTask dto)
	{
		if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > MAX_SIZE_TITLE)
		{
			_notificationError.AddError($"Título é obrigatório e deve ter no máximo {MAX_SIZE_TITLE} caracteres.");
		}

		if (string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Length > MAX_SIZE_DESCRIPTION)
		{
			_notificationError.AddError($"Descrição é obrigatória e deve ter no máximo {MAX_SIZE_DESCRIPTION} caracteres.");
		}

		_ = _userService.GetUserById(dto.UserId);

		if (_notificationError.HasErrors)
		{
			return null;
		}

		var newTask = new Core.Entities.Task()
		{
			Title = dto.Title,
			Description = dto.Description,
			CreatedDate = DateTime.Now,
			Status = EnumStatusTask.TODO,
			UserId = dto.UserId
		};

		var taskAdded = await _taskRepository.Add(newTask);

		return taskAdded;
	}

	public async Task<Core.Entities.Task> UpdateTaskAsync(Guid taskId, DTOUpdateTask dto)
	{
		var task = _taskRepository.GetAll().FirstOrDefault(t => t.Id.Equals(taskId));

		if (task is null)
		{
			_notificationError.AddError($"Tarefa não encontrada.");
		}

		var idsStatus = Enum.GetValues<EnumStatusTask>().Cast<int>().ToList();

		if (!idsStatus.Contains((int)dto.Status))
		{
			_notificationError.AddError($"Status da tarefa inválido.");
		}

		if (_notificationError.HasErrors)
		{
			return null;
		}

		task.Status = dto.Status;
		task.FinishDate = dto.Status == EnumStatusTask.DONE ? DateTime.Now : null;

		return await _taskRepository.Update(task);
	}

	public async Task<bool> DeleteTaskAsync(Guid taskId)
	{
		var task = _taskRepository.GetAll().FirstOrDefault(t => t.Id.Equals(taskId));

		if (task is null)
		{
			_notificationError.AddError($"Tarefa não encontrada.");
		}

		if (_notificationError.HasErrors)
		{
			return false;
		}

		return await _taskRepository.Delete(task);
	}

	public IQueryable<Core.Entities.Task> GetAllTasks()
	{
		var tasks = _taskRepository.GetAll();
		return tasks;
	}

	public IQueryable<Core.Entities.Task> GetTasksFromUser(Guid userId)
	{
		var tasks = _taskRepository.GetAll().Where(t => t.UserId == userId);

		if (userId == Guid.Empty)
		{
			_notificationError.AddError($"Usuário inválido.");
		}

		if (_notificationError.HasErrors)
		{
			return null;
		}

		return tasks;
	}
}
