using System.ComponentModel.DataAnnotations;
namespace TodoApi.Model;


public class TodoItem
{

	public uint TodoItemId { get; set; }
	public string? Task { get; set; }
	public string? Instructions { get; set; }

	[Range(1, 5)]
	public uint Priority { get; set; } = 5;

	public DateTime? Deadline { get; set; }
	public bool IsComplete { get; set; } = false;


}