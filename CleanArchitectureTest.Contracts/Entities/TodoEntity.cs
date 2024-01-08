﻿namespace CleanArchitectureTest.Contracts.Entities;

public class TodoEntity
{
    public required Guid Id { get; set; }

    public required string Title { get; set; } = "";

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

}
