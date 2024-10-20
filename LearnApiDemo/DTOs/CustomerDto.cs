using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnApiDemo.DTOs
{
    public class CustomerDto
    {
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

        //adding additional Property
        // we want if IsActive is true then StatusName => active, if IsActive is false, StatusName => inactive
        public string? StatusName { get; set; }
    }
}
