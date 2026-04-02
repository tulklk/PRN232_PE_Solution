using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Q1.Models;

public partial class Director
{
    [Key]
    public int Id { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    public bool Male { get; set; }

    public DateOnly Dob { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Nationality { get; set; } = null!;

    [Column(TypeName = "ntext")]
    public string Description { get; set; } = null!;

    [InverseProperty("Director")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
