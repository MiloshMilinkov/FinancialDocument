using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int DocumentId { get; set; }  
        public string ClientVAT { get; set; }
        
        public bool IsWhitelisted { get; set; }

        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; }
    }
}