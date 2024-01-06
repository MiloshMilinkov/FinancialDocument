using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Client
    {
        public Guid TenantId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientVAT { get; set; }
    }
}