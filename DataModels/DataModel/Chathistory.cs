using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc;

[Table("chathistory")]
public partial class Chathistory
{
    [Key]
    [Column("msgid")]
    public int Msgid { get; set; }

    [Column("msg")]
    public string Msg { get; set; } = null!;

    [Column("sender", TypeName = "character varying")]
    public string Sender { get; set; } = null!;

    [Column("reciever", TypeName = "character varying")]
    public string Reciever { get; set; } = null!;

    [Column("isread")]
    public bool Isread { get; set; }

    [Column("senttime", TypeName = "timestamp without time zone")]
    public DateTime Senttime { get; set; }

    [Column("readtime", TypeName = "timestamp without time zone")]
    public DateTime Readtime { get; set; }

    [Column("issent")]
    public bool Issent { get; set; }

    [Column("recievetime", TypeName = "timestamp without time zone")]
    public DateTime Recievetime { get; set; }
}
