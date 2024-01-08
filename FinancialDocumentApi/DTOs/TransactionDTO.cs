using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialDocumentApi.DTOs
{
    public class TransactionDTO
    {
        public string? TransactionId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}