using ToDoList.Application.Interfaces.Notifications;
using ToDoList.Application.Interfaces.Repository;
using ToDoList.Application.Interfaces.Service;
using ToDoList.Application.Notifications;
using ToDoList.Application.Services;
using ToDoList.Infraestructure.Repository;

namespace ToDoList.API
{
	public static class ExtensionMethods
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<ITaskRepository, TaskRepository>();
			services.AddScoped<IUserRepository, UserRepository>();

			return services;
		}

		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<ITaskService, TaskService>();
			services.AddScoped<IUserService, UserService>();

			return services;
		}

		public static IServiceCollection AddNotifications(this IServiceCollection services)
		{
			services.AddScoped<INotificationError, NotificationError>();

			return services;
		}
	}
}
