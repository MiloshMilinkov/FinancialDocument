using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Tenant
    {
        public Guid TenantId { get; set; }
        public bool IsWhitelisted { get; set; }
    }
}