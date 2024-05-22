using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc;

[Table("groupchat")]
public partial class Groupchat
{
    [Column("groupid")]
    public int? Groupid { get; set; }

    [Column("adminasp", TypeName = "character varying")]
    public string? Adminasp { get; set; }

    [Column("physicainasp", TypeName = "character varying")]
    public string? Physicainasp { get; set; }

    [Column("userasp", TypeName = "character varying")]
    public string? Userasp { get; set; }

    [Column("senttime", TypeName = "timestamp without time zone")]
    public DateTime? Senttime { get; set; }

    [Column("msg")]
    public string? Msg { get; set; }

    [Column("sender")]
    public int? Sender { get; set; }

    [Key]
    [Column("msgid")]
    public int Msgid { get; set; }
}
