namespace CleanArchitectureTest.Data.DTOs;

public class Todo : Base
{
    public required Guid Id { get; set; }

    public required string Title { get; set; } = "";

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public required Guid CreatedById { get; set; }

    public virtual Member? CreatedBy { get; set; }
}
