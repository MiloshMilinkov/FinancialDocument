using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class WhiteListedClient
    {
        [Key]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client Client { get; set; }
        public bool IsWhitelisted { get; set; }
    }
}