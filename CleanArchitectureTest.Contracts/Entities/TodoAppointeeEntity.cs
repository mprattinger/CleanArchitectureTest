namespace CleanArchitectureTest.Contracts.Entities;

public class TodoAppointeeEntity
{
    public TodoEntity? Todo { get; set; }

    public MemberEntity? Appointee { get; set; }
}
