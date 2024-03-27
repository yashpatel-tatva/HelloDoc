﻿using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc;

[Table("smslog")]
public partial class Smslog
{
    [Key]
    [Column("smslogid")]
    public int Smslogid { get; set; }

    [Column("smstemplate", TypeName = "character varying")]
    public string Smstemplate { get; set; } = null!;

    [Column("mobilenumber")]
    [StringLength(50)]
    public string Mobilenumber { get; set; } = null!;

    [Column("confirmationnumber")]
    [StringLength(50)]
    public string? Confirmationnumber { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("adminid")]
    public int? Adminid { get; set; }

    [Column("requestid")]
    public int? Requestid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("createdate", TypeName = "timestamp without time zone")]
    public DateTime Createdate { get; set; }

    [Column("sentdate", TypeName = "timestamp without time zone")]
    public DateTime? Sentdate { get; set; }

    [Column("issmssent", TypeName = "bit(1)")]
    public BitArray? Issmssent { get; set; }

    [Column("senttries")]
    public int Senttries { get; set; }

    [Column("action")]
    public int? Action { get; set; }
}
