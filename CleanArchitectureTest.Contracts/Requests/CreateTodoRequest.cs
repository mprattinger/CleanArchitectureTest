namespace CleanArchitectureTest.Contracts.Requests;

public class CreateTodoRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid CreatedBy { get; set; }
}
