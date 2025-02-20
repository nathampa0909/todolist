using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.MVC.Filters;

public class SessionTimeoutFilter : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
		var session = context.HttpContext.Session;
		var route = context.RouteData.Values["controller"]?.ToString();
		var action = context.RouteData.Values["action"]?.ToString();
		
		if ((action == "Index" || action == "Login" || action == "Register") && route == "Login")
		{
			return;
		}

		var strDueDate = session.GetString("TokenDueDate");

		if (!DateTime.TryParse(strDueDate, out var dueDate) || DateTime.UtcNow > dueDate)
		{
			session.Clear();
			context.Result = new RedirectToActionResult("Index", "Login", null);
			return;
		}
	}

	public void OnActionExecuted(ActionExecutedContext context) { }
}