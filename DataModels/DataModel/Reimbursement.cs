using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc;

[Table("reimbursement")]
public partial class Reimbursement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("item")]
    public string? Item { get; set; }

    [Column("amount")]
    public decimal? Amount { get; set; }

    [Column("bill")]
    public string? Bill { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    [StringLength(128)]
    public string? Createdby { get; set; }

    [Column("modifirdby")]
    [StringLength(128)]
    public string? Modifirdby { get; set; }

    [Column("biweektimeid")]
    public int? Biweektimeid { get; set; }

    [Column("date", TypeName = "timestamp without time zone")]
    public DateTime? Date { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("billname")]
    public string? Billname { get; set; }

    [ForeignKey("Biweektimeid")]
    [InverseProperty("Reimbursements")]
    public virtual Biweektime? Biweektime { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Reimbursements")]
    public virtual Physician? Physician { get; set; }
}
