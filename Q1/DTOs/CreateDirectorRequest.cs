using System.ComponentModel.DataAnnotations;

namespace Q1.DTOs;

public class CreateDirectorRequest
{
    [Required]
    public string FullName { get; set; } = null!;

    // POST body property is `male` (bool). When true => Gender = Male.
    public bool Male { get; set; }

    // Expected format: yyyy-MM-dd
    [Required]
    public string Dob { get; set; } = null!;

    [Required]
    public string Nationality { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}

