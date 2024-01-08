using System.Text.Json.Serialization;

namespace CleanArchitectureTest.Contracts.Entities;

public class TodoEntity
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; } = "";

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

}
