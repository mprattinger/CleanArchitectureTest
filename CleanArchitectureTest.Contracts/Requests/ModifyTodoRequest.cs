namespace CleanArchitectureTest.Contracts.Requests;

public class ModifyTodoRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}
