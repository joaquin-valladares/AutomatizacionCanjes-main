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
    public class TBL_CentrosController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: TBL_Centros
        public ActionResult Index()
        {
            return View(db.TBL_Centros.ToList());
        }

        // GET: TBL_Centros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Centros tBL_Centros = db.TBL_Centros.Find(id);
            if (tBL_Centros == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Centros);
        }

        // GET: TBL_Centros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_Centros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_sociedad,nombre_centro")] TBL_Centros tBL_Centros)
        {
            if (ModelState.IsValid)
            {
                db.TBL_Centros.Add(tBL_Centros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_Centros);
        }

        // GET: TBL_Centros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Centros tBL_Centros = db.TBL_Centros.Find(id);
            if (tBL_Centros == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Centros);
        }

        // POST: TBL_Centros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_sociedad,nombre_centro")] TBL_Centros tBL_Centros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_Centros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_Centros);
        }

        // GET: TBL_Centros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Centros tBL_Centros = db.TBL_Centros.Find(id);
            if (tBL_Centros == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Centros);
        }

        // POST: TBL_Centros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_Centros tBL_Centros = db.TBL_Centros.Find(id);
            db.TBL_Centros.Remove(tBL_Centros);
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
