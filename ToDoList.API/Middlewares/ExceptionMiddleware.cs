using System.Net;
using System.Text.Json;

namespace ToDoList.API.Middlewares;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception ex)
	{
		var traceId = Guid.NewGuid().ToString();
		var response = new
		{
			status = (int)HttpStatusCode.InternalServerError,
			error = "Internal Server Error",
			message = "Ocorreu um erro inesperado.",
			traceId
		};

		_logger.LogError(ex, $"Erro inesperado. TraceId: {traceId}");

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		await context.Response.WriteAsync(JsonSerializer.Serialize(response));
	}
}