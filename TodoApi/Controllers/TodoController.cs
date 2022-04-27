#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodoController : ControllerBase
	{
		private readonly TodoDbContext _context;

		public TodoController(TodoDbContext context)
		{
			_context = context;
		}

		// GET: api/Todo
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
		{
            // var sortedItems = from tds in _context.TodoItems
            // orderby tds Deadline, tds Priority
            // select tds;
            // return await sortedItems.ToListAsync();

			return await _context.TodoItems.OrderBy(tds => tds.Deadline).ThenBy(tds=>tds.Priority).ToListAsync();
		}

		// GET: api/Todo/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItem>> GetTodoItem(uint id)
		{
			var todoItem = await _context.TodoItems.FindAsync(id);

			if (todoItem == null)
			{
				return NotFound();
			}

			return todoItem;
		}

		// PUT: api/Todo/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(uint id, TodoItem todoItem)
		{
			if (id != todoItem.TodoItemId)
			{
				return BadRequest();
			}

			_context.Entry(todoItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TodoItemExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Todo
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
		{
			_context.TodoItems.Add(todoItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.TodoItemId }, todoItem);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchTodoItem(uint id, TodoItem item)
		{
			if (id != item.TodoItemId)
			{
				return BadRequest();
			}

			var taskItem = await _context.TodoItems.FindAsync(id);

			if (taskItem == null)
			{
				return NotFound();
			}

			taskItem.IsComplete = item.IsComplete;
			_context.Update(taskItem);
			await _context.SaveChangesAsync();

			return NoContent();

		}

		// DELETE: api/Todo/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTodoItem(uint id)
		{
			var todoItem = await _context.TodoItems.FindAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			_context.TodoItems.Remove(todoItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool TodoItemExists(uint id)
		{
			return _context.TodoItems.Any(e => e.TodoItemId == id);
		}
	}
}
