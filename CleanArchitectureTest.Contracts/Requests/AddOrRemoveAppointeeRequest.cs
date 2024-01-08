namespace CleanArchitectureTest.Contracts.Requests;

public record AddOrRemoveAppointeeRequest(Guid todoId, Guid memberId);
