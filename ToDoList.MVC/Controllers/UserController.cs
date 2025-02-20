using Microsoft.AspNetCore.Mvc;
using ToDoList.Core.Entities;

namespace ToDoList.MVC.Controllers;

public class UserController(IHttpClientFactory clientFactory) : Controller
{
	private readonly HttpClient _httpClientToDoList = clientFactory.CreateClient("ToDoListApi");

	public async Task<IActionResult> IndexAsync()
	{
		var response = await _httpClientToDoList.GetAsync($"/user/all");
		var users = new List<User>();

		if (response.IsSuccessStatusCode)
		{
			users = await response.Content.ReadFromJsonAsync<List<User>>();
		}

		return View(users);
	}

	[HttpPost]
	public async Task<IActionResult> DeleteAsync(string id)
	{
		var response = await _httpClientToDoList.DeleteAsync($"/user/{id}");
		return RedirectToAction("Index");
	}
}