using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces.Notifications;
using ToDoList.Application.Interfaces.Service;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("task")]
[Authorize]
public class TaskController(INotificationError notificationError, IUserService userService, ITaskService taskService) : ControllerBase
{
	private readonly INotificationError _notificationError = notificationError;
	private readonly IUserService _userService = userService;
	private readonly ITaskService _taskService = taskService;

	[HttpGet("all")]
	public IActionResult GetAllTasks()
	{
		var tasks = _taskService.GetAllTasks();

		return Ok(tasks);
	}

	[HttpGet]
	public IActionResult GetTasksFromUser(Guid userId)
	{
		var tasks = _taskService.GetTasksFromUser(userId);

		if (_notificationError.HasErrors)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível obter a tarefa.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return Ok(tasks);
	}

	[HttpPost]
	public async Task<IActionResult> CreateNewTaskAsync(DTONewTask dto)
	{
		var createdTask = await _taskService.CreateNewTaskAsync(dto);

		if (_notificationError.HasErrors)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível criar uma nova tarefa.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return Ok(createdTask);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateTaskAsync([FromRoute] Guid id, [FromBody] DTOUpdateTask dto)
	{
		var updatedTask = await _taskService.UpdateTaskAsync(id, dto);

		if (_notificationError.HasErrors)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível atualizar a tarefa.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return Ok(updatedTask);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteTaskAsync([FromRoute] Guid id)
	{
		var deletedTask = await _taskService.DeleteTaskAsync(id);

		if (_notificationError.HasErrors)
		{
			return BadRequest(new ProblemDetails()
			{
				Detail = _notificationError.HasErrors ? _notificationError.Errors.FirstOrDefault() : string.Empty,
				Title = "Não foi possível deletar a tarefa.",
				Status = StatusCodes.Status400BadRequest
			});
		}

		return NoContent();
	}
}
