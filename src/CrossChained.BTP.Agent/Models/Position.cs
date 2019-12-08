using CrossChained.BTP.Agent.API;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.Models
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public PositionState state { get; set; }

        public DateTime proposed { get; set; }
        public DateTime? opened { get; set; }
        public DateTime? closed { get; set; }
        public CloseReason? close_reason { get; set; }

        public OrderSide side { get; set; }
        
        public decimal amount { get; set; }
        public decimal margin_value { get; set; }
        public decimal? entry_price { get; set; }
        public decimal? closed_price { get; set; }
        public decimal decay_rate { get; set; }
        public decimal? rebate { get; set; }
        public string trx { get; set; }
        public bool confirmed { get; set; }

        [MaxLength(50)]
        public string bsv_address { get; set; }
    }
}
