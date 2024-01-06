using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public bool IsSupported { get; set; }
    }
}