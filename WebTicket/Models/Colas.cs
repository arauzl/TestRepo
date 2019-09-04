using System;
using System.Collections.Generic;

namespace WebTicket.Models
{
    public partial class Colas
    {
        public int Id { get; set; }
        public int? Cola { get; set; }
        public int? TicketId { get; set; }
    }
}
