namespace ToDoApi
{
    public class ToDoItemDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        public ToDoItemDTO()
        {

        }

        public ToDoItemDTO(ToDo todoItem) =>
            (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
    }
}
