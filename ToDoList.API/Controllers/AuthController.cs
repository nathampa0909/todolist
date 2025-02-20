using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces.Notifications;
using ToDoList.Application.Interfaces.Service;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserService userService, INotificationError notificationError) : ControllerBase
{
	private readonly IUserService _userService = userService;
	private readonly INotificationError _notificationError = notificationError;

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] DTOLoginRequest request)
	{
		var tokenResponse = await _userService.LoginAsync(request);

		if (tokenResponse is null)
		{
			return Unauthorized(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível realizar login.",
				Status = StatusCodes.Status401Unauthorized
			});
		}

		return Ok(tokenResponse);
	}
}
