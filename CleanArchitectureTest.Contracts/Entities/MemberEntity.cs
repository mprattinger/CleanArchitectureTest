using System.Text.Json.Serialization;

namespace CleanArchitectureTest.Contracts.Entities;

public class MemberEntity
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = "";
}