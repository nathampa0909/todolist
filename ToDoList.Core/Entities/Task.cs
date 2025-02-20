using System.Text.Json.Serialization;
using ToDoList.Core.Enums;

namespace ToDoList.Core.Entities;

public class Task
{
	public Guid Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public EnumStatusTask Status { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? FinishDate { get; set; }
	public Guid UserId { get; set; }

	[JsonIgnore]
	public User User { get; set; }
}
