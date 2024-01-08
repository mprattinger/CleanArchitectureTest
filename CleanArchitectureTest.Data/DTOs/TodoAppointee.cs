namespace CleanArchitectureTest.Data.DTOs;

public class TodoAppointee
{
    public Guid TodoId { get; set; }
    public Todo Todo { get; set; } = default!;

    public Guid AppointeeId { get; set; }
    public Member Appointee { get; set; } = default!;
}
