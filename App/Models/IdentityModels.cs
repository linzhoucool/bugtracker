using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication6.Models.Classes;



namespace WebApplication6.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public ApplicationUser()
        {

            Projects = new HashSet<Project>();
            CreatedTickets= new HashSet<TicketModels>();
            AssignedTickets = new HashSet<TicketModels>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistories>();
            Notifications = new HashSet<TicketNotification>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public ICollection<Project> Projects { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<TicketModels> CreatedTickets { get; set; }
        [InverseProperty("Assigned")]
        public virtual ICollection<TicketModels> AssignedTickets { get; set; }

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }

        public virtual ICollection<TicketHistories> TicketHistories { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }

        public virtual ICollection<TicketNotification> Notifications { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        

        public System.Data.Entity.DbSet<WebApplication6.Models.Classes.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.TicketModels> TicketModels { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.Priority> Priority { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.Status> Status { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.Type>Type { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.TicketComment> TicketComments { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.TicketAttachment> TicketAttachments { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.TicketHistories> TicketHistories { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.TicketNotification> TicketNotifications{ get; set; }


        //public System.Data.Entity.DbSet<WebApplication6.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}