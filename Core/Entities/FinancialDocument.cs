using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FinancialDocument
    {
        public Guid TenantId { get; set; }
        public Guid DocumentId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}