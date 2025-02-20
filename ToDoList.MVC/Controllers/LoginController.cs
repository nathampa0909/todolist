using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.DTOs;

namespace ToDoList.MVC.Controllers
{
    public class LoginController(IHttpClientFactory clientFactory) : Controller
	{
		private readonly HttpClient _httpClientToDoList = clientFactory.CreateClient("ToDoListApi");

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			var dto = new DTOLoginRequest()
			{
				Username = username,
				Password = password
			};

			var response = await _httpClientToDoList.PostAsJsonAsync("/auth/login", dto);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<DTOTokenResponse>();

				HttpContext.Session.SetString("Token", result.Token);
				HttpContext.Session.SetString("UserId", result.UserId.ToString());

				var dueDate = result.DueDate;

				HttpContext.Session.SetString("TokenDueDate", dueDate.ToString());

				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Login ou senha incorretos." });
		}

		[HttpPost]
		public async Task<IActionResult> Register(string name, string username, string password)
		{
			var dto = new DTONewUser()
			{
				Name = name,
				Username = username,
				Password = password
			};

			var response = await _httpClientToDoList.PostAsJsonAsync("/user", dto);

			if (response.IsSuccessStatusCode)
			{
				return await Login(username, password);
			}

			var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();

			return Json(new { success = false, message = result.Detail });
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();

			return View("Index");
		}
	}
}
