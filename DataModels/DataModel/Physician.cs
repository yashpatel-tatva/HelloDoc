﻿using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloDoc;

[Table("physician")]
public partial class Physician
{
    [Key]
    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("aspnetuserid")]
    [StringLength(128)]
    public string? Aspnetuserid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("mobile")]
    [StringLength(20)]
    public string? Mobile { get; set; }

    [Column("medicallicense")]
    [StringLength(500)]
    public string? Medicallicense { get; set; }

    [Column("photo")]
    public string? Photo { get; set; }

    [Column("adminnotes")]
    [StringLength(500)]
    public string? Adminnotes { get; set; }

    [Column("isagreementdoc", TypeName = "bit(1)")]
    public BitArray? Isagreementdoc { get; set; }

    [Column("isbackgrounddoc", TypeName = "bit(1)")]
    public BitArray? Isbackgrounddoc { get; set; }

    [Column("istrainingdoc", TypeName = "bit(1)")]
    public BitArray? Istrainingdoc { get; set; }

    [Column("isnondisclosuredoc", TypeName = "bit(1)")]
    public BitArray? Isnondisclosuredoc { get; set; }

    [Column("address1")]
    [StringLength(500)]
    public string? Address1 { get; set; }

    [Column("address2")]
    [StringLength(500)]
    public string? Address2 { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("zip")]
    [StringLength(10)]
    public string? Zip { get; set; }

    [Column("altphone")]
    [StringLength(20)]
    public string? Altphone { get; set; }

    [Column("createdby")]
    [StringLength(128)]
    public string Createdby { get; set; } = null!;

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    [StringLength(128)]
    public string? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("businessname")]
    [StringLength(100)]
    public string Businessname { get; set; } = null!;

    [Column("businesswebsite")]
    [StringLength(200)]
    public string Businesswebsite { get; set; } = null!;

    [Column("isdeleted", TypeName = "bit(1)")]
    public BitArray? Isdeleted { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("npinumber")]
    [StringLength(500)]
    public string? Npinumber { get; set; }

    [Column("islicensedoc", TypeName = "bit(1)")]
    public BitArray? Islicensedoc { get; set; }

    [Column("signature")]
    public string? Signature { get; set; }

    [Column("iscredentialdoc", TypeName = "bit(1)")]
    public BitArray? Iscredentialdoc { get; set; }

    [Column("istokengenerate", TypeName = "bit(1)")]
    public BitArray? Istokengenerate { get; set; }

    [Column("syncemailaddress")]
    [StringLength(50)]
    public string? Syncemailaddress { get; set; }

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("PhysicianAspnetusers")]
    public virtual Aspnetuser? Aspnetuser { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("PhysicianCreatedbyNavigations")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [InverseProperty("Physician")]
    public virtual ICollection<Emaillog> Emaillogs { get; set; } = new List<Emaillog>();

    [ForeignKey("Modifiedby")]
    [InverseProperty("PhysicianModifiedbyNavigations")]
    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Physicianlocation> Physicianlocations { get; set; } = new List<Physicianlocation>();

    [InverseProperty("Pysician")]
    public virtual ICollection<Physiciannotification> Physiciannotifications { get; set; } = new List<Physiciannotification>();

    [InverseProperty("Physician")]
    public virtual ICollection<Physicianregion> Physicianregions { get; set; } = new List<Physicianregion>();

    [ForeignKey("Regionid")]
    [InverseProperty("Physicians")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    [InverseProperty("Physician")]
    public virtual ICollection<Requeststatuslog> RequeststatuslogPhysicians { get; set; } = new List<Requeststatuslog>();

    [InverseProperty("Transtophysician")]
    public virtual ICollection<Requeststatuslog> RequeststatuslogTranstophysicians { get; set; } = new List<Requeststatuslog>();

    [InverseProperty("Physician")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; set; } = new List<Requestwisefile>();

    [InverseProperty("Physician")]
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
