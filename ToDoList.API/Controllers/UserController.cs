using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces.Notifications;
using ToDoList.Application.Interfaces.Service;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController(IUserService userService, INotificationError notificationError) : ControllerBase
{
	private readonly IUserService _userService = userService;
	private readonly INotificationError _notificationError = notificationError;

	[HttpGet("all")]
	public IActionResult GetAllUsers()
	{
		var users = _userService.GetAllUsers();

		if (users is null)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível obter os usuários.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return Ok(users);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> CreateNewUserAsync(DTONewUser user)
	{
		var newUser = await _userService.CreateNewUserAsync(user);

		if (newUser is null)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível criar o usuário.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return Ok(newUser);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
	{
		var deletedTask = await _userService.DeleteUserAsync(id);

		if (_notificationError.HasErrors)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível deletar o usuário.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return NoContent();
	}
}
