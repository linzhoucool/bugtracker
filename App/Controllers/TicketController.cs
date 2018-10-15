using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
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
            var userId = User.Identity.GetUserId();
            var projectModel = db.Users.Where(p => p.Id == userId).First().
                Projects.Select(p => p.Id).First();
            return View("Index", db.TicketModels.Where(p => p.Id == projectModel).ToList());
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
        public ActionResult Create([Bind(Include = "Id,Description,Name,ProjectId,Created,Updated,PriorityId,StatusId,TypeId,CreatorId,AssignedId")] TicketModels ticketModels)
        {
            if (ModelState.IsValid)
            {
                db.TicketModels.Add(ticketModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticketModels.CreatorId);
            ViewBag.PriorityId = new SelectList(db.Priority, "Id", "Name", ticketModels.PriorityId);
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
                db.Entry(ticketModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Name", ticketModels.CreatorId);
            ViewBag.PriorityId = new SelectList(db.Priority, "Id", "Name", ticketModels.PriorityId);
            return View(ticketModels);
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
