using System;
using System.ComponentModel.DataAnnotations;

namespace PenMgmt.Common.Domain
{
    public class PartMaster : BaseDomainModel
    {
        // Important Properties - Can be set only during object creation 
        [Required]
        [StringLength(50, MinimumLength=5)]
        public string Code { get; set; }

        [Required]
        [StringLength(20, MinimumLength=3)]
        public string Type { get; set; }

        // Object Properties - Can be updated during the lifetime of object as long as object is not committed
        [MaxLength(250)]
        public string Description { get; set; }
        public int Count { get; set; }
        public float Weight { get; set; }
    }
}
