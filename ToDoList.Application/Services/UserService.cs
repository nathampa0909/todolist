using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces.Notifications;
using ToDoList.Application.Interfaces.Repository;
using ToDoList.Application.Interfaces.Service;
using ToDoList.Application.Utils;
using ToDoList.Core.Entities;

namespace ToDoList.Application.Services;

public class UserService(IUserRepository userRepository, INotificationError notificationError, IConfiguration configuration) : IUserService
{
	private readonly IConfiguration _configuration = configuration;
	private readonly IUserRepository _userRepository = userRepository;
	private readonly INotificationError _notificationError = notificationError;

	public User GetUserByUsername(string username)
	{
		if (string.IsNullOrWhiteSpace(username))
		{
			_notificationError.AddError($"Obrigatório informar o usuário.");
			return null;
		}

		var user = _userRepository.GetAll().FirstOrDefault(u => u.Username.Equals(username));
		return user;
	}

	public User GetUserById(Guid id)
	{
		if (id == Guid.Empty)
		{
			_notificationError.AddError($"Obrigatório informar o usuário.");
			return null;
		}

		var user = _userRepository.GetAll().FirstOrDefault(u => u.Id.Equals(id));

		if (user is null)
		{
			_notificationError.AddError("Usuário não encontrado.");
		}

		return user;
	}

	public IQueryable<User> GetAllUsers()
	{
		var users = _userRepository.GetAll();
		return users;
	}

	public async Task<User> CreateNewUserAsync(DTONewUser dtoNewUser)
	{
		const int MAX_SIZE_NAME = 50;
		const int MAX_SIZE_USERNAME = 30;

		if (string.IsNullOrWhiteSpace(dtoNewUser.Name) || dtoNewUser.Name.Length > MAX_SIZE_NAME)
		{
			_notificationError.AddError($"Nome é obrigatório e deve ter no máximo {MAX_SIZE_NAME} caracteres.");
		}

		var alphaNumericalRegex = @"^[A-Za-z][A-Za-z0-9._]*$";

		if (string.IsNullOrWhiteSpace(dtoNewUser.Username) || !Regex.IsMatch(dtoNewUser.Username, alphaNumericalRegex) || dtoNewUser.Username.Length > MAX_SIZE_USERNAME)
		{
			_notificationError.AddError($"Usuário é obrigatório, deve ser alfanumérico e ter no máximo {MAX_SIZE_USERNAME} caracteres.");
		}

		if (string.IsNullOrWhiteSpace(dtoNewUser.Password))
		{
			_notificationError.AddError($"Obrigatório informar senha.");
		}

		var usernameAlreadyExists = GetAllUsers().FirstOrDefault(u => u.Username.Equals(dtoNewUser.Username)) != null;

		if (usernameAlreadyExists)
		{
			_notificationError.AddError($"Usuário informado já existe.");
		}

		if (_notificationError.HasErrors)
		{
			return null;
		}

		var passwordHash = CryptographyUtils.GetSHA256HashFromString(dtoNewUser.Password);
		var user = new User()
		{
			Name = dtoNewUser.Name,
			Username = dtoNewUser.Username,
			PasswordHash = passwordHash
		};

		var userAdded = await _userRepository.Add(user);

		return userAdded;
	}

	public async Task<DTOTokenResponse> LoginAsync(DTOLoginRequest dto)
	{
		var user = GetAllUsers().FirstOrDefault(u => u.Username.Equals(dto.Username));
		var passwordHash = CryptographyUtils.GetSHA256HashFromString(dto.Password);

		if (user == null)
		{
			_notificationError.AddError("Usuário não existe.");
		}
		else if (!user.PasswordHash.Equals(passwordHash))
		{
			_notificationError.AddError("Senha informada inválida.");
		}

		if (_notificationError.HasErrors)
		{
			return null;
		}

		var token = JwtUtils.GenerateJwtToken(dto.Username, _configuration);
		token.UserId = user.Id;

		return token;
	}

	public async Task<bool> DeleteUserAsync(Guid userId)
	{
		var task = _userRepository.GetAll().FirstOrDefault(t => t.Id.Equals(userId));

		if (task is null)
		{
			_notificationError.AddError($"Usuário não encontrado.");
		}

		if (_notificationError.HasErrors)
		{
			return false;
		}

		return await _userRepository.Delete(task);
	}
}
