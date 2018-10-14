using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Status
    {
        public Status(){
            Ticket = new HashSet<TicketModels>();
        
}

        public int Id {get;set;}
        public string Name {get;set;}
        public ICollection<TicketModels> Ticket { get; set; }

    }
}