using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LearnApiDemo.Models;

[Table("tbl_ProductImages")]
public partial class TblProductImage
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("productCode")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ProductCode { get; set; }

    [Column("productImage", TypeName = "image")]
    public byte[]? ProductImage { get; set; }
}
