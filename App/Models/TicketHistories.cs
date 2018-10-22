using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class TicketHistories
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public virtual TicketModels Ticket { get; set; }

        public DateTimeOffset Changed { get; set; }
        public string property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

 
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    

}