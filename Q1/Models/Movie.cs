using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Q1.Models;

public partial class Movie
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Language { get; set; } = null!;

    public int? ProducerId { get; set; }

    public int? DirectorId { get; set; }

    [ForeignKey("DirectorId")]
    [InverseProperty("Movies")]
    public virtual Director? Director { get; set; }

    [ForeignKey("ProducerId")]
    [InverseProperty("Movies")]
    public virtual Producer? Producer { get; set; }

    [ForeignKey("MovieId")]
    [InverseProperty("Movies")]
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    [ForeignKey("MovieId")]
    [InverseProperty("Movies")]
    public virtual ICollection<Star> Stars { get; set; } = new List<Star>();
}
