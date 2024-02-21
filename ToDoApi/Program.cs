using Microsoft.EntityFrameworkCore;
using ToDoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

RouteGroupBuilder todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(ToDoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new ToDoItemDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(ToDoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new ToDoItemDTO(x)).ToListAsync());
}

static async Task<IResult> GetTodo(int id, ToDoDb db)
{
    return await db.Todos.FindAsync(id)
        is ToDo todo
            ? TypedResults.Ok(new ToDoItemDTO(todo))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(ToDoItemDTO todoItemDTO, ToDoDb db)
{
    var todoItem = new ToDo
    {
        IsComplete = todoItemDTO.IsComplete,
        Name = todoItemDTO.Name
    };
    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();

    todoItemDTO = new ToDoItemDTO(todoItem);

    return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
}

static async Task<IResult> UpdateTodo(int id, ToDoItemDTO inputItemDTO, ToDoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = inputItemDTO.Name;
    todo.IsComplete = inputItemDTO.IsComplete;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, ToDoDb db)
{
    if (await db.Todos.FindAsync(id) is ToDo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}