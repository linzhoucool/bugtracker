using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models.Classes
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Project()
        {
            Tickets = new HashSet<TicketModels>();
            Users = new HashSet<ApplicationUser>();
        }
        public ICollection<ApplicationUser> Users { get; set;}
        public ICollection<TicketModels>Tickets { get; set; }
    }
}


