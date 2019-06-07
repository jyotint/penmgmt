using System;
using System.ComponentModel.DataAnnotations;

namespace PenMgmt.Common.Domain
{
    public class BaseDomainModel
    {
        // System Properties - Auto-generated Primary Key
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        public int Committed { get; set; }

        // System Properties - Managed by Server API application 
        public int Deleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        
        // System Properties - Will be used for managing concurrency
        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
