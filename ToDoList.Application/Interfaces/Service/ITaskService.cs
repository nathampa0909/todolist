using ToDoList.Application.DTOs;

namespace ToDoList.Application.Interfaces.Service;

public interface ITaskService
{
	Task<Core.Entities.Task> CreateNewTaskAsync(DTONewTask dto);
	Task<bool> DeleteTaskAsync(Guid taskId);
	Task<Core.Entities.Task> UpdateTaskAsync(Guid taskId, DTOUpdateTask dto);
	IQueryable<Core.Entities.Task> GetAllTasks();
	IQueryable<Core.Entities.Task> GetTasksFromUser(Guid userId);
}
