using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }
        public bool IsWhitelisted { get; set; }
    }
}