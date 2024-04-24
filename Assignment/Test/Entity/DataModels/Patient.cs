using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Test;

[Table("Patient")]
public partial class Patient
{
    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    public string? FirstName { get; set; }

    [StringLength(128)]
    public string? LastName { get; set; }

    public int? DocterId { get; set; }

    public decimal? Age { get; set; }

    [StringLength(128)]
    public string? Email { get; set; }

    [StringLength(128)]
    public string? Phone { get; set; }

    [Column(TypeName = "character varying")]
    public string? Gender { get; set; }

    [StringLength(128)]
    public string? Dieses { get; set; }

    [StringLength(128)]
    public string? Specialist { get; set; }

    public bool? Isdeleted { get; set; }

    [ForeignKey("DocterId")]
    [InverseProperty("Patients")]
    public virtual Doctor? Docter { get; set; }
}
