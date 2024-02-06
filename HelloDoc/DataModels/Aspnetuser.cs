using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.DataModels;

[Table("aspnetusers")]
[Index("Username", Name = "aspnetusers_username_key", IsUnique = true)]
public partial class Aspnetuser
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("username")]
    [StringLength(256)]
    public string Username { get; set; } = null!;

    [Column("passwordhash", TypeName = "character varying")]
    public string? Passwordhash { get; set; }

    [Column("securitystamp", TypeName = "character varying")]
    public string? Securitystamp { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("emailconfirmed", TypeName = "bit(1)")]
    public BitArray Emailconfirmed { get; set; } = null!;

    [Column("phonenumber", TypeName = "character varying")]
    public string? Phonenumber { get; set; }

    [Column("phonenumberconfirmed", TypeName = "bit(1)")]
    public BitArray Phonenumberconfirmed { get; set; } = null!;

    [Column("twofactorenabled", TypeName = "bit(1)")]
    public BitArray Twofactorenabled { get; set; } = null!;

    [Column("lockoutenddateutc", TypeName = "timestamp without time zone")]
    public DateTime? Lockoutenddateutc { get; set; }

    [Column("lockoutenabled", TypeName = "bit(1)")]
    public BitArray Lockoutenabled { get; set; } = null!;

    [Column("accessfailedcount")]
    public int Accessfailedcount { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("corepasswordhash", TypeName = "character varying")]
    public string? Corepasswordhash { get; set; }

    [Column("hashversion")]
    public int? Hashversion { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Admin> AdminAspnetusers { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminCreatedbyNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminModifiedbyNavigations { get; set; } = new List<Admin>();

    [InverseProperty("User")]
    public virtual ICollection<Aspnetuserrole> Aspnetuserroles { get; set; } = new List<Aspnetuserrole>();

    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; set; } = new List<Business>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
