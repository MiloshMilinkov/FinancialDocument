using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string ClientVAT { get; set; }
        public string RegistrationNumber { get; set; }
        public string CompanyType { get; set; }  
    }
}