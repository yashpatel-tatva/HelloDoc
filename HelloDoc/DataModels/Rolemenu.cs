using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.DataModels;

[Table("rolemenu")]
public partial class Rolemenu
{
    [Key]
    [Column("rolemenuid")]
    public int Rolemenuid { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("menuid")]
    public int Menuid { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Rolemenus")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("Rolemenuid")]
    [InverseProperty("Rolemenu")]
    public virtual Menu RolemenuNavigation { get; set; } = null!;
}
