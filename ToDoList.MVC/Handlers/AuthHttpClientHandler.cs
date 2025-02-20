using System.Net.Http.Headers;

namespace ToDoList.MVC.Handlers;

public class AuthHttpClientHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

		if (!string.IsNullOrEmpty(token))
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}