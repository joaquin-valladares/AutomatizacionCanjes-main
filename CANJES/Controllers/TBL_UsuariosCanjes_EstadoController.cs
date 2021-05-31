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
    public class TBL_UsuariosCanjes_EstadoController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: TBL_UsuariosCanjes_Estado
        public ActionResult Index()
        {
            return View(db.TBL_UsuariosCanjes_Estado.ToList());
        }

        // GET: TBL_UsuariosCanjes_Estado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_UsuariosCanjes_Estado tBL_UsuariosCanjes_Estado = db.TBL_UsuariosCanjes_Estado.Find(id);
            if (tBL_UsuariosCanjes_Estado == null)
            {
                return HttpNotFound();
            }
            return View(tBL_UsuariosCanjes_Estado);
        }

        // GET: TBL_UsuariosCanjes_Estado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_UsuariosCanjes_Estado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_estado,estado")] TBL_UsuariosCanjes_Estado tBL_UsuariosCanjes_Estado)
        {
            if (ModelState.IsValid)
            {
                db.TBL_UsuariosCanjes_Estado.Add(tBL_UsuariosCanjes_Estado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_UsuariosCanjes_Estado);
        }

        // GET: TBL_UsuariosCanjes_Estado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_UsuariosCanjes_Estado tBL_UsuariosCanjes_Estado = db.TBL_UsuariosCanjes_Estado.Find(id);
            if (tBL_UsuariosCanjes_Estado == null)
            {
                return HttpNotFound();
            }
            return View(tBL_UsuariosCanjes_Estado);
        }

        // POST: TBL_UsuariosCanjes_Estado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_estado,estado")] TBL_UsuariosCanjes_Estado tBL_UsuariosCanjes_Estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_UsuariosCanjes_Estado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_UsuariosCanjes_Estado);
        }

        // GET: TBL_UsuariosCanjes_Estado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_UsuariosCanjes_Estado tBL_UsuariosCanjes_Estado = db.TBL_UsuariosCanjes_Estado.Find(id);
            if (tBL_UsuariosCanjes_Estado == null)
            {
                return HttpNotFound();
            }
            return View(tBL_UsuariosCanjes_Estado);
        }

        // POST: TBL_UsuariosCanjes_Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_UsuariosCanjes_Estado tBL_UsuariosCanjes_Estado = db.TBL_UsuariosCanjes_Estado.Find(id);
            db.TBL_UsuariosCanjes_Estado.Remove(tBL_UsuariosCanjes_Estado);
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
