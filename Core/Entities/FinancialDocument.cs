using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class FinancialDocument
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ProductId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}