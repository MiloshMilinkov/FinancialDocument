using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
        public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductCode { get; set; }
        public bool IsSupported { get; set; }

        public string FinancialDocumentTemplate { get; set; }
    }
}