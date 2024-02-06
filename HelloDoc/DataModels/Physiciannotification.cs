﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.DataModels;

[Table("physiciannotification")]
public partial class Physiciannotification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("isnotificationstopped", TypeName = "bit(1)")]
    public BitArray? Isnotificationstopped { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physiciannotifications")]
    public virtual Physician Physician { get; set; } = null!;
}
