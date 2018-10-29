using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebApplication6.Helper;
using WebApplication6.Models;

namespace WebApplication6.Controllers4
{
    public class TicketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ticket
        public ActionResult Index()
        {
            var ticketModels = db.TicketModels.
               Include(t => t.Project).
               Include(t => t.Assigned).
               Include(t => t.Creator).
               Include(t => t.Priority).
               Include(t => t.Status).
               Include(t => t.Type);
            return View(ticketModels.ToList());
        }

        [Authorize(Roles = "Submitter")]
        public ActionResult SubmitterOfTheTickets()
        {
            var userId = User.Identity.GetUserId();

            var ticketModels = db.TicketModels.
                Where(p => p.CreatorId == userId).
                Include(t => t.Project).
                Include(t => t.Assigned).
                Include(t => t.Creator).
                Include(t => t.Priority).
                Include(t => t.Status).
                Include(t => t.Type);

            return View("Index", ticketModels.ToList());
        }
        [Authorize(Roles = "Developer")]
        public ActionResult DeveloperOfTheTickets()
        {
            var userId = User.Identity.GetUserId();

            var ticketModels = db.TicketModels.
                Where(p => p.AssignedId == userId).
                Include(t => t.Project).
                Include(t => t.Assigned).
                Include(t => t.Creator).
                Include(t => t.Priority).
                Include(t => t.Status).
                Include(t => t.Type);

            return View("Index", ticketModels.ToList());
        }
    
        [Authorize(Roles = "Project Manager, Developer")]
        public ActionResult TheProjManagerTicketsAndTheDeveloperTickets()
        {
            //在数据库里找跟登录者一样的id，然后再在projects里找跟数据库一样用户id的project id，并且将这些挑选出来的project id做成list，在index页面显示出来
 
            var userId = User.Identity.GetUserId(); // find the same id with the peroson who log in in the database 
            var projectModel = db.Users.Where(p => p.Id == userId).First().// find the same project id in the project  
            Projects.Select(p => p.Id).First();
            return View("Index", db.TicketModels.Where(p => p.Id == projectModel).ToList()); // show a list of 
        }
        
        // GET: Ticket/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketModels ticketModels = db.TicketModels.Find(id);
            if (ticketModels == null)
            {
                return HttpNotFound();
            }
            return View(ticketModels);
        }

        // GET: Ticket/Create
       
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.AssignedId = new SelectList(db.Users, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name");
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.Type, "Id", "Name");
            ViewBag.PriorityId = new SelectList(db.Priority, "Id", "Name");
            return View();
        }

        // POST: Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Name,ProjectId,Created,Updated,PriorityId,StatusId,TypeId,CreatorId,AssignedId")] TicketModels ticketModels,HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                ticketModels.CreatorId = userId;
                ticketModels.Created = DateTimeOffset.Now;
                ticketModels.StatusId = 1;

                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));

                    var iomg = new TicketAttachment();
                    iomg.FilePath = "/Uploads/" + fileName;
                }

                db.TicketModels.Add(ticketModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticketModels.AssignedId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticketModels.CreatorId);
            ViewBag.PriorityId = new SelectList(db.Priority, "Id", "Name", ticketModels.PriorityId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", ticketModels.StatusId);
            ViewBag.TypeId = new SelectList(db.Type, "Id", "Name", ticketModels.TypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketModels.ProjectId);
            
            return View(ticketModels);
        }
        
        // GET: Ticket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketModels ticketModels = db.TicketModels.Find(id);
            if (ticketModels == null)
            {
                return HttpNotFound();
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticketModels.AssignedId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticketModels.CreatorId);
            ViewBag.PriorityId = new SelectList(db.Priority, "Id", "Name", ticketModels.PriorityId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", ticketModels.StatusId);
            ViewBag.TypeId = new SelectList(db.Type, "Id", "Name", ticketModels.TypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketModels.ProjectId);

            return View(ticketModels);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,Created,Updated,PriorityId,StatusId,TypeId,CreatorId,AssignedId")] TicketModels ticketModels)
        {
            if (ModelState.IsValid)
            {
                var dateChanged = DateTimeOffset.Now;
                var changes = new List<TicketHistories>();
                var dbTicket = db.TicketModels.First(p => p.Id == ticketModels.Id);
                dbTicket.Title = ticketModels.Title;
                dbTicket.Description = ticketModels.Description;
                dbTicket.TypeId = ticketModels.TypeId;
                dbTicket.Updated = dateChanged;
                var originalValues = db.Entry(dbTicket).OriginalValues;
                var currentValues = db.Entry(dbTicket).CurrentValues;
                foreach (var property in originalValues.PropertyNames)
                {
                    var originalValue = originalValues[property]?.ToString();
                    var currentValue = currentValues[property]?.ToString();
                    if (originalValue != currentValue)
                    {
                        var history = new TicketHistories();
                        history.Changed = dateChanged;
                        history.NewValue = GetValueFromKey(property, currentValue);
                        history.OldValue = GetValueFromKey(property, originalValue);
                        history.property = property;
                        history.TicketId = dbTicket.Id;
                        history.UserId = User.Identity.GetUserId();
                        changes.Add(history);
                    }
                }
                db.TicketHistories.AddRange(changes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Name", ticketModels.AssignedId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticketModels.CreatorId);
            ViewBag.PriorityId = new SelectList(db.Priority, "Id", "Name", ticketModels.PriorityId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", ticketModels.StatusId);
            ViewBag.TypeId = new SelectList(db.Type, "Id", "Name", ticketModels.TypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketModels.ProjectId);
            return View(ticketModels);
        }
        
        private string GetValueFromKey(string propertyName, string key)
        {
            if (propertyName == "TicketTypeId")
            {
                return db.Type.Find(Convert.ToInt32(key)).Name;
            }
            return key;
        }

        
        // GET: Ticket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketModels ticketModels = db.TicketModels.Find(id);
            if (ticketModels == null)
            {
                return HttpNotFound();
            }
            return View(ticketModels);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketModels ticketModels = db.TicketModels.Find(id);
            db.TicketModels.Remove(ticketModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Authorize(Roles ="Admin,Project Manager,Submitter,Developer")]
        public ActionResult CreateComment(int id, string body , IdentityMessage message)
        {
            var ticketcomment = db.TicketModels.Where(p => p.Id == id).FirstOrDefault();

            if (ticketcomment == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                TempData["ErrorMessage"] = "Comment is required";
                return RedirectToAction("Details", new {id=id });
            }

            var comment = new TicketComment();
            comment.UserId = User.Identity.GetUserId();
            comment.TicketId = ticketcomment.Id;
            comment.Created = DateTime.Now;
            comment.Comment = body;
            db.TicketComments.Add(comment);

            //Step 1: Find if there is a developer assigned to the ticket

            //Step 2: If yes, send him an e-email



            var attachmentFind = db.Users.Where(t => t.Id == comment.UserId).FirstOrDefault();

            var email = WebConfigurationManager.AppSettings["emailto"];



            var personalEmailService = new PersonalEmailService();
            var mailMessage = new MailMessage( 
               WebConfigurationManager.AppSettings["emailto"],
               attachmentFind.Email
               );



            mailMessage.Body = "fake";
            mailMessage.Subject = "comments";
            mailMessage.IsBodyHtml = true;
            personalEmailService.Send(mailMessage);

          
            db.SaveChanges();

            return RedirectToAction("Details", new { id=id });
        }

        // Attachment Create
        [HttpPost]
        [Authorize(Roles = "Admin,Project Manager,Submitter,Developer")]
        public ActionResult CreateAttachment(int id, HttpPostedFileBase image, IdentityMessage message)
        {

            var sbjl = new TicketAttachment();
            var sbgl =db.TicketModels.Where(p => p.Id == id).FirstOrDefault();

            if (ModelState.IsValid)
            {

                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    ViewBag.ErrorMessage = "Uploaded the avater is required";

                }
                 var fileName = Path.GetFileName(image.FileName);
                 image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                sbjl.FilePath = "/Uploads/" + fileName;
                sbjl.UserId = User.Identity.GetUserId();
                sbjl.TicketId = sbgl.Id;
                sbjl.Created = DateTime.Now;
                db.TicketAttachments.Add(sbjl);

                var attachmentFind2 = db.Users.Where(t => t.Id == sbjl.UserId).FirstOrDefault();
                var personalEmailService = new PersonalEmailService();
                var mailMessage = new MailMessage(
                   WebConfigurationManager.AppSettings["emailto"],
                  attachmentFind2.Email
                   );
                mailMessage.Body = "Correct";
                mailMessage.Subject = "Attachment";
                mailMessage.IsBodyHtml = true;
                personalEmailService.Send(mailMessage);
               
                db.SaveChanges();
                return RedirectToAction("Details",new{id});
            }

            return View(sbjl);
        }
        // Create Notification
        // Assign User
        [Authorize(Roles= "ProjectManager")]
        public ActionResult AssignUsers(int id)
        {
            var model = new TicketAssignViewModel();

            model.Id = id;

            var Ticket = db.TicketModels.FirstOrDefault(p => p.Id == id);
            var users = db.Users.ToList();
            var userIdsAssignedToTicket = Ticket.Users.Select(p => p.Id).ToList();

            model.UserList = new MultiSelectList(users, "Id", "DisplayName", userIdsAssignedToTicket);

            return View(model);
        }

        [HttpPost]
        [Authorize (Roles = "ProjectManager")]
        public ActionResult AssignUsers(TicketAssignViewModel model)
        {
            //STEP 1: Find the project
            var ticket = db.TicketModels.FirstOrDefault(p => p.Id == model.Id);

            //STEP 2: Remove all assigned users from this project
            var assignedUsers = ticket.Users.ToList();

            foreach (var user in assignedUsers)
            {
                ticket.Users.Remove(user);
            }

            //STEP 3: Assign users to the project
            if (model.SelectedUsers != null)
            {
                foreach (var userId in model.SelectedUsers)
                {
                    var user = db.Users.FirstOrDefault(p => p.Id == userId);

                    ticket.Users.Add(user);
                }
            }

            //STEP 4: Save changes to the database
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        //
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
