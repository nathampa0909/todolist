using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities;
using ToDoList.Infraestructure.Mappers;
using Task = ToDoList.Core.Entities.Task;

namespace ToDoList.Infraestructure.Context;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Task> Tasks { get; set; }
	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Task>(new MapperTask().Configure);
		modelBuilder.Entity<User>(new MapperUser().Configure);
	}
}