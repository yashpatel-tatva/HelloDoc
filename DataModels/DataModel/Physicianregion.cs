﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc;

[Table("physicianregion")]
public partial class Physicianregion
{
    [Key]
    [Column("physicianregionid")]
    public int Physicianregionid { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("regionid")]
    public int Regionid { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physicianregions")]
    public virtual Physician Physician { get; set; } = null!;

    [ForeignKey("Regionid")]
    [InverseProperty("Physicianregions")]
    public virtual Region Region { get; set; } = null!;
}
