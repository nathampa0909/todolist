using ToDoList.Application.DTOs;
using ToDoList.Core.Entities;

namespace ToDoList.Application.Interfaces.Service;

public interface IUserService
{
	User GetUserByUsername(string username);
	IQueryable<User> GetAllUsers();
	Task<User> CreateNewUserAsync(DTONewUser dtoNewUser);
	Task<DTOTokenResponse> LoginAsync(DTOLoginRequest dto);
	User GetUserById(Guid id);
	Task<bool> DeleteUserAsync(Guid userId);
}