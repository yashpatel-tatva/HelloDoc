using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Test;

[Table("Doctor")]
public partial class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    [Column(TypeName = "character varying")]
    public string? Specialist { get; set; }

    [InverseProperty("Docter")]
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
