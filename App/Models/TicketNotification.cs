using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class TicketNotification
    {

        public int Id { get; set; }


        public int TicketId { get; set; }


        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ApplicationUser Ticket { get; set; }






    }
}