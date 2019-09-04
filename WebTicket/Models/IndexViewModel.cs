using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTicket.Models
{
    public class IndexViewModel
    {
        public int ticket_id { get; set; }
        public String nombre { get; set; }
        public List<ColaViewModel> cola1 { get; set; }
        public List<ColaViewModel> cola2 { get; set; }
        public int cola { get; set; }


    }
}
