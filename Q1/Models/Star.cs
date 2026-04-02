using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Q1.Models;

public partial class Star
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    public bool? Male { get; set; }

    public DateOnly? Dob { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Nationality { get; set; }

    [ForeignKey("StarId")]
    [InverseProperty("Stars")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
