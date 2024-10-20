using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LearnApiDemo.Models;

[Table("tbl_Customers")]
public partial class TblCustomer
{
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CreditLimit { get; set; }

    public bool? IsActive { get; set; }

    public int? TaxCode { get; set; }
}
