namespace ToDoList.Application.DTOs;

public class DTOTokenResponse
{
	public DateTime CreatedAt { get; set; }
	public DateTime DueDate { get; set; }
	public string Token { get; set; }
	public Guid UserId { get; set; }
}