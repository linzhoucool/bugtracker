namespace WebApplication6.Migrations
{
    using WebApplication6.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication6.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(WebApplication6.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }

            if (!context.Roles.Any(p => p.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole("ProjectManager"));
            }

            if (!context.Roles.Any(p => p.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole("Developer"));
            }

            if (!context.Roles.Any(p => p.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole("Submitter"));
            }

            ApplicationUser adminUser ;

            if (!context.Users.Any(p => p.UserName == "admin@bugtracker.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.Email = "admin@bugtracker.com";
                adminUser.UserName = "admin@bugtracker.com";
                
                adminUser.FirstName = "Admin";
                adminUser.LastName = "User";
                adminUser.DisplayName = "Admin User";
                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context.Users.Where(p => p.UserName == "admin@bugtracker.com")
                    .FirstOrDefault();
            }
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }



            ApplicationUser projectmanagerUser ;

        
           if (!context.Users.Any(p => p.UserName == "projectmanager@bugtracker.com"))
            {
                projectmanagerUser = new ApplicationUser();
                projectmanagerUser.Email = "projectmanager@bugtracker.com";
                projectmanagerUser.UserName = "projectmanager@bugtracker.com";
                projectmanagerUser.FirstName = "Project";
                projectmanagerUser.LastName = "Manager";
                projectmanagerUser.DisplayName = "Project Manager";
                userManager.Create(projectmanagerUser, "Password-2");       
            }
            else
            {
                projectmanagerUser = context.Users.Where(p => p.UserName == "ProjectManager@bugtracker.com")
                    .FirstOrDefault();
            }
            if (!userManager.IsInRole(projectmanagerUser.Id, "ProjectManager"))
            {
                userManager.AddToRole(projectmanagerUser.Id, "ProjectManager");
            }


            ApplicationUser developerUser;
        
            if (!context.Users.Any(p => p.UserName == "developer@bugtracker.com"))
            {
                developerUser = new ApplicationUser();
                developerUser.Email = "developer@bugtracker.com";
                developerUser.UserName = "developer@bugtracker.com";
                developerUser.FirstName = "Developer";
                developerUser.LastName = "User";
                developerUser.DisplayName = "Developer User";

                userManager.Create(developerUser, "Password-3");
            }
            else
            {
                developerUser = context.Users.Where(p => p.UserName == "developer@bugtracker.com")
                    .FirstOrDefault();
            }
            if (!userManager.IsInRole(developerUser.Id, "Developer"))
            {
                userManager.AddToRole(developerUser.Id, "Developer");
            }


            ApplicationUser submitterUser;


             if (!context.Users.Any(p => p.UserName == "submitter@bugtracker.com"))
            {
                submitterUser = new ApplicationUser();
                submitterUser.Email = "submitter@bugtracker.com";
                submitterUser.UserName = "submitter@bugtracker.com";
                submitterUser.FirstName = "Submitter";
                submitterUser.LastName = "Submitter";
                submitterUser.DisplayName = "Submitter!";


                userManager.Create(submitterUser, "Password-4");
            }
            else
            {
                submitterUser = context.Users.Where(p => p.UserName == "submitter@bugtracker.com")
                    .FirstOrDefault();
            }
            if (!userManager.IsInRole(submitterUser.Id, "Submitter"))
            {
                userManager.AddToRole(submitterUser.Id, "Submitter");
            }

            context.Type.AddOrUpdate(t => t.Name,
               new Type {Id=1, Name = "Bug Fixes" },
               new Type {Id=2,Name = "Software Update" },
               new Type {Id=3,Name = "Adding Helpers" },
               new Type {Id=4, Name = "Database Error" });

            context.Priority.AddOrUpdate(t => t.Name,
               new Priority { Id = 1, Name = "Low" },
               new Priority { Id = 2,Name = "Medium" },
               new Priority { Id = 3,Name = "High" },
               new Priority { Id = 4,Name = "Urgent" });

            context.Status.AddOrUpdate(t => t.Name,
              new Status { Id = 1, Name = "Not Started" },
              new Status { Id = 2, Name = "In Progress" },
              new Status { Id = 3, Name = "On Hold" },
              new Status { Id = 4, Name = "Finished" });

        }
    }
}