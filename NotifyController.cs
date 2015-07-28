using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MManatee.Models;

namespace MManatee.Controllers
{
    public class NotifyController : Controller
    {
        private MManateeEntities db2 = new MManateeEntities();

        // GET: Messages
        public ActionResult Index()
        {
            var messageSet = db2.MessageSet.Include(m => m.Type).Where(u => u.OwnedBy == User.Identity.Name).Where(t => t.Type.Name == "Notify").OrderByDescending(o => o.DateTime);
            return View(messageSet.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db2.MessageSet.Find(id);
            message.ReadState = true;
            db2.SaveChanges();
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.TypeID = new SelectList(db2.TypeSet, "TypeID", "Name");
            return View();
        }

        // POST: Messages/Create
        // Chcete-li zajistit ochranu před útoky typu OVERPOST, povolte konkrétní vlastnosti, k nimž chcete vytvořit vazbu. 
        // Další informace viz http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "MessageID,Subject,Body,OwnedBy,CreatedBy,DateTime,ReadState,TypeID")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.CreatedBy = User.Identity.Name;
                message.DateTime = DateTime.Now;
                message.ReadState = false;
                message.TypeID = 2;
                db2.MessageSet.Add(message);
                db2.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db2.TypeSet, "TypeID", "Name", message.TypeID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db2.MessageSet.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(db2.TypeSet, "TypeID", "Name", message.TypeID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // Chcete-li zajistit ochranu před útoky typu OVERPOST, povolte konkrétní vlastnosti, k nimž chcete vytvořit vazbu. 
        // Další informace viz http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageID,Subject,Body,OwnedBy,CreatedBy,DateTime,ReadState,TypeID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db2.Entry(message).State = EntityState.Modified;
                db2.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db2.TypeSet, "TypeID", "Name", message.TypeID);
            return View(message);
        }

        // GET: Messages/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db2.MessageSet.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db2.MessageSet.Find(id);
            db2.MessageSet.Remove(message);
            db2.SaveChanges();
            return RedirectToAction("Index");
        }

       
        public ActionResult ToInbox(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Message message = db2.MessageSet.Find(id);
            message.TypeID = 1;
            db2.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SwitchBox()
        {
            return RedirectToAction("Index", "Messages");
        }


        [Authorize]
        public ActionResult Notification(int? id)
        {
            var message = db2.MessageSet.Find(id);
            message.Type.Name = "Inbox";
            db2.SaveChanges();
            
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult Response(int? id)
        {
            var message = db2.MessageSet.Find(id);
            ViewBag.TypeID = new SelectList(db2.TypeSet, "TypeID", "Name");
            message.Subject = "";
            message.Body = "";
            return View(message);
        }


        [HttpPost, ActionName("Response")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ResponseConfirmed([Bind(Include = "MessageID,Subject,Body,OwnedBy,CreatedBy,DateTime,ReadState,TypeID")] Message message, string over)
        {
            if (ModelState.IsValid)
            {
                message.CreatedBy = User.Identity.Name;
                message.DateTime = DateTime.Now;
                message.ReadState = false;
                message.TypeID = 2;
                db2.MessageSet.Add(message);
                db2.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db2.TypeSet, "TypeID", "Name", message.TypeID);
            return View(message);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db2.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
