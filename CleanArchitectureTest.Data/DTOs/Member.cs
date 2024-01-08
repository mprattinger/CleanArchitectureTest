namespace CleanArchitectureTest.Data.DTOs;

public class Member : Base
{
    public required Guid Id { get; set; }

    public required string FirstName { get; set; } = "";

    public required string LastName { get; set; } = "";

    public IEnumerable<TodoAppointee> TodoAppointees { get; set; } = Enumerable.Empty<TodoAppointee>();
}
