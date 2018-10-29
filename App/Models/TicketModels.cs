using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication6.Models.Classes;

namespace WebApplication6.Models
{
    public class TicketModels
    {
      public TicketModels()
        {
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            TicketHistories = new HashSet<TicketHistories>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set;}
        public DateTimeOffset? Updated { get; set;}

        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; }

        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        public int TypeId { get; set; }
        public virtual Type Type { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator{ get; set; }

        public string AssignedId { get; set; }
        public virtual ApplicationUser Assigned { get; set; }
        
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
          public virtual ICollection<TicketComment> TicketComments { get; set; }
            public virtual ICollection<TicketHistories> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        public virtual ICollection<ApplicationUser>Users { get; set; }
    }
    }
