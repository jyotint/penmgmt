using System;
using System.ComponentModel.DataAnnotations;

namespace PenMgmt.Common.Domain
{
    public class PartMaster
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength=5)]
        public string Code { get; set; }
 
        [MaxLength(250)]
        public string Description { get; set; }
 
        [Required]
        [StringLength(20, MinimumLength=3)]
         public string Type { get; set; }
        public int Count { get; set; }
        public float Weight { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [Timestamp]
        
        public byte[] TimeStamp { get; set; }
    }
}
