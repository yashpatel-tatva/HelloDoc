using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.DataModels;

[Table("users")]
public partial class User
{
    [Key]
    [Column("userid")]
    public int Userid { get; set; }

    [Column("aspnetuserid")]
    [StringLength(128)]
    public string Aspnetuserid { get; set; } = null!;

    [Column("firstname")]
    [Required]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Name.")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [Required]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Name.")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("mobile")]
    [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
    [StringLength(20)]
    public string? Mobile { get; set; }

    [Column("ismobile", TypeName = "bit(1)")]
    public BitArray? Ismobile { get; set; }

    [Column("street")]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
    [StringLength(100)]
    public string? Street { get; set; }

    [Column("city")]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("state")]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
    [StringLength(100)]
    public string? State { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("zip")]
    [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
    public string? Zip { get; set; }

    [Column("strmonth")]
    [StringLength(20)]
    public string? Strmonth { get; set; }

    [Column("intyear")]
    public int? Intyear { get; set; }

    [Column("intdate")]
    public int? Intdate { get; set; }

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

    [Column("isdeleted", TypeName = "bit(1)")]
    public BitArray? Isdeleted { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("isrequestwithemail", TypeName = "bit(1)")]
    public BitArray? Isrequestwithemail { get; set; }

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("Users")]
    public virtual Aspnetuser Aspnetuser { get; set; } = null!;

    [ForeignKey("Regionid")]
    [InverseProperty("Users")]
    public virtual Region? Region { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public static implicit operator User(Task<User?> v)
    {
        throw new NotImplementedException();
    }
}
