using ToDoList.Core.Enums;

namespace ToDoList.Application.DTOs;

public class DTONewTask
{
	public string Title { get; set; }
	public string Description { get; set; }
	public Guid UserId { get; set; }
}