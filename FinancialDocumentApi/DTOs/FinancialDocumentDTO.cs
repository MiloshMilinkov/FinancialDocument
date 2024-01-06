using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialDocumentApi.DTOs
{
    public class FinancialDocumentDTO
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public IEnumerable<TransactionDTO> Transactions { get; set; }
        public TenantDTO Tenant { get; set; }
    }
}