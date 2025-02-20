using ToDoList.Application.Interfaces.Repository;
using ToDoList.Core.Entities;
using ToDoList.Infraestructure.Context;

namespace ToDoList.Infraestructure.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
	public UserRepository(AppDbContext dbContext) : base(dbContext) { }
}
