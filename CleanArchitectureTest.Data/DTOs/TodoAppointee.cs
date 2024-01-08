using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureTest.Data.DTOs;

public class TodoAppointee : Base
{
    [Key]
    public Guid TodoId { get; set; }

    [Key]
    public Guid AppointeeId { get; set; } = default!;

}
