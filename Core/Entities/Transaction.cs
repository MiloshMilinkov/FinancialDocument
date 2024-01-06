using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int FinancialDocumentId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        [ForeignKey("FinancialDocumentId")]
        public FinancialDocument FinancialDocument { get; set; }
    }
}