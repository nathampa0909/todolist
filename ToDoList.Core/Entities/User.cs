﻿namespace ToDoList.Core.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Username { get; set; }
	public string PasswordHash { get; set; }
	public List<Task> Tasks { get; set; } = [];
}
