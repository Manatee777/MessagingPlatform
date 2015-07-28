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
    public class MessagesController : Controller
    {
        private MManateeEntities db = new MManateeEntities();

        // GET: Messages
        public ActionResult Index()
        {
            var messageSet = db.MessageSet.Include(m => m.Type).Where(u => u.OwnedBy == User.Identity.Name).Where(t => t.Type.Name == "Inbox").OrderByDescending(o => o.DateTime); //include vyhodí více výsledků, where je profiltruje
            return View(messageSet.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id) //nullovatelný typ
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.MessageSet.Find(id);
            message.ReadState = true;
            db.SaveChanges();
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
            ViewBag.TypeID = new SelectList(db.TypeSet, "TypeID", "Name");
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
                message.TypeID= 2;
                db.MessageSet.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.TypeSet, "TypeID", "Name", message.TypeID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.MessageSet.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(db.TypeSet, "TypeID", "Name", message.TypeID);
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
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db.TypeSet, "TypeID", "Name", message.TypeID);
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
            Message message = db.MessageSet.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.MessageSet.Find(id);
            db.MessageSet.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

         [Authorize]
        public ActionResult Response(int? id) 
        {
            var message = db.MessageSet.Find(id);
            message.Subject = "";
            message.Body = "";
             ViewBag.TypeID = new SelectList(db.TypeSet, "TypeID", "Name");
             return View(message);
        }


         [HttpPost, ActionName("Response")] //actionname dodaná kvůli shodě actionresults
         [ValidateAntiForgeryToken]
         [Authorize]
         public ActionResult ResponseConfirmed([Bind(Include = "MessageID,Subject,Body,OwnedBy,CreatedBy,DateTime,ReadState,TypeID")] Message message, string over)
         {
             if (ModelState.IsValid)
             {
                 message.CreatedBy = User.Identity.Name;
                 message.DateTime = DateTime.Now;
                 message.ReadState = false;
                 message.TypeID = 1;
                 db.MessageSet.Add(message);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.TypeID = new SelectList(db.TypeSet, "TypeID", "Name", message.TypeID);
             return View(message);
         }

        public ActionResult SwitchBox()
        {
         return RedirectToAction("Index", "Notify");
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
