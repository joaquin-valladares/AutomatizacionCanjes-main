using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CANJES.Models;

namespace CANJES.Controllers
{
    public class TBL_SociedadesController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: TBL_Sociedades
        public ActionResult Index()
        {
            return View(db.TBL_Sociedades.ToList());
        }

        // GET: TBL_Sociedades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Sociedades tBL_Sociedades = db.TBL_Sociedades.Find(id);
            if (tBL_Sociedades == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Sociedades);
        }

        // GET: TBL_Sociedades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_Sociedades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre_sociedad")] TBL_Sociedades tBL_Sociedades)
        {
            if (ModelState.IsValid)
            {
                db.TBL_Sociedades.Add(tBL_Sociedades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_Sociedades);
        }

        // GET: TBL_Sociedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Sociedades tBL_Sociedades = db.TBL_Sociedades.Find(id);
            if (tBL_Sociedades == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Sociedades);
        }

        // POST: TBL_Sociedades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre_sociedad")] TBL_Sociedades tBL_Sociedades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_Sociedades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_Sociedades);
        }

        // GET: TBL_Sociedades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Sociedades tBL_Sociedades = db.TBL_Sociedades.Find(id);
            if (tBL_Sociedades == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Sociedades);
        }

        // POST: TBL_Sociedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_Sociedades tBL_Sociedades = db.TBL_Sociedades.Find(id);
            db.TBL_Sociedades.Remove(tBL_Sociedades);
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
