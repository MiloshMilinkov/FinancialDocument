using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialDocumentApi.DTOs
{
    public class TenantDTO
    {
        public int Id { get; set; }
        public bool IsWhitelisted { get; set; }
    }
}