using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LearnApiDemo.Models;

[Table("tbl_RefreshToken")]
public partial class TblRefreshToken
{
    [Key]
    [Column("userId")]
    [StringLength(50)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    [Column("tokenId")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TokenId { get; set; }

    [Column("refreshToken")]
    [Unicode(false)]
    public string? RefreshToken { get; set; }
}
