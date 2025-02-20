using Microsoft.Extensions.DependencyInjection;
using ToDoList.MVC.Filters;
using ToDoList.MVC.Handlers;

namespace ToDoList.MVC;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllersWithViews(options =>
		{
			options.Filters.Add<SessionTimeoutFilter>();
		});

		var toDoListApiUrl = builder.Configuration["Apis:ToDoListApiUrl"];

		builder.Services.AddHttpClient("ToDoListApi", client =>
		{
			client.BaseAddress = new Uri(toDoListApiUrl);
			client.DefaultRequestHeaders.Add("Accept", "application/json");
		}).AddHttpMessageHandler<AuthHttpClientHandler>();

		builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		builder.Services.AddTransient<AuthHttpClientHandler>();

		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(30);
			options.Cookie.HttpOnly = true;
			options.Cookie.IsEssential = true;
		});	

		var app = builder.Build();

		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseSession();
		app.UseHttpsRedirection();
		app.UseRouting();
		app.UseAuthorization();
		app.MapStaticAssets();
		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Login}/{action=Index}/{id?}")
			.WithStaticAssets();

		app.Run();
	}
}
