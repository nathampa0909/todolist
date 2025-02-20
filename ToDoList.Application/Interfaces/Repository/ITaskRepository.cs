using TaskToDo = ToDoList.Core.Entities.Task;

namespace ToDoList.Application.Interfaces.Repository;

public interface ITaskRepository : IBaseRepository<TaskToDo>
{
}
