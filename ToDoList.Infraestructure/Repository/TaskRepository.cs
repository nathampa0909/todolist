using ToDoList.Application.Interfaces.Repository;
using ToDoList.Infraestructure.Context;
using Task = ToDoList.Core.Entities.Task;

namespace ToDoList.Infraestructure.Repository;

public class TaskRepository : BaseRepository<Task>, ITaskRepository
{
	public TaskRepository(AppDbContext dbContext) : base(dbContext) { }
}
