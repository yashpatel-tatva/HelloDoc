﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc;

[Table("physicianlocation")]
public partial class Physicianlocation
{
    [Key]
    [Column("locationid")]
    public int Locationid { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("latitude")]
    [Precision(9, 3)]
    public decimal? Latitude { get; set; }

    [Column("longitude")]
    [Precision(9, 3)]
    public decimal? Longitude { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("physicianname")]
    [StringLength(50)]
    public string? Physicianname { get; set; }

    [Column("address")]
    [StringLength(500)]
    public string? Address { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physicianlocations")]
    public virtual Physician Physician { get; set; } = null!;
}
