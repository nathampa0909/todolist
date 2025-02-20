using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Xml.Linq;
using ToDoList.Application.DTOs;
using ToDoList.Core.Enums;

namespace ToDoList.MVC.Controllers;

public class TaskController : Controller
{
	private readonly HttpClient _httpClientToDoList;

	public TaskController(IHttpClientFactory clientFactory)
	{
		_httpClientToDoList = clientFactory.CreateClient("ToDoListApi");
	}

	public async Task<IActionResult> IndexAsync()
	{
		var userId = HttpContext.Session.GetString("UserId");
		var response = await _httpClientToDoList.GetAsync($"/task?userId={userId}");
		var groupedTasks = new Dictionary<int, List<Core.Entities.Task>>();

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<List<Core.Entities.Task>>();
			groupedTasks = result.GroupBy(t => (int)t.Status).ToDictionary(g => g.Key, g => g.ToList());
		}

		return View(groupedTasks);
	}

	[HttpPost]
	public async Task<IActionResult> CreateAsync(string title, string description)
	{
		var userId = HttpContext.Session.GetString("UserId");
		var dto = new DTONewTask()
		{
			Title = title,
			Description = description,
			UserId = Guid.Parse(userId)
		};

		var response = await _httpClientToDoList.PostAsJsonAsync("/task", dto);

		if (response.IsSuccessStatusCode)
		{
			return Json(new { success = true });
		}

		var resultError = await response.Content.ReadFromJsonAsync<ProblemDetails>();

		return Json(new { success = false, message = resultError.Detail });
	}

	[HttpPost]
	public async Task<IActionResult> UpdateAsync(string taskId, int status)
	{
		var dto = new DTOUpdateTask()
		{
			Status = (EnumStatusTask)status,
		};

		var response = await _httpClientToDoList.PutAsJsonAsync($"/task/{taskId}", dto);

		if (response.IsSuccessStatusCode)
		{
			return Json(new { success = true });
		}

		var resultError = await response.Content.ReadFromJsonAsync<ProblemDetails>();

		return Json(new { success = false, message = resultError.Detail });
	}

	[HttpPost]
	public async Task<IActionResult> DeleteAsync(string id)
	{
		var response = await _httpClientToDoList.DeleteAsync($"/task/{id}");
		return RedirectToAction("Index");
	}
}