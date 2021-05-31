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
    public class TBL_DetalleSAPController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: TBL_DetalleSAP
        public ActionResult Index()
        {
            return View(db.TBL_DetalleSAP.ToList());
        }

        // GET: TBL_DetalleSAP/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_DetalleSAP tBL_DetalleSAP = db.TBL_DetalleSAP.Find(id);
            if (tBL_DetalleSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_DetalleSAP);
        }

        // GET: TBL_DetalleSAP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_DetalleSAP/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Consecutivo,Documento,Cod_Prod_SAP,Nombre_Segun_Proveedor,Cantidad_Solicitada,Saldo_SAP,Cantidad_Enviar")] TBL_DetalleSAP tBL_DetalleSAP)
        {
            if (ModelState.IsValid)
            {
                db.TBL_DetalleSAP.Add(tBL_DetalleSAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_DetalleSAP);
        }

        // GET: TBL_DetalleSAP/Edit/5
        public ActionResult Edit(string Documento)
        {
            if (Documento == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_DetalleSAP tBL_DetalleSAP = db.TBL_DetalleSAP.Find(Documento);
            if (tBL_DetalleSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_DetalleSAP);
        }

        // POST: TBL_DetalleSAP/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Consecutivo,Documento,Cod_Prod_SAP,Nombre_Segun_Proveedor,Cantidad_Solicitada,Saldo_SAP,Cantidad_Enviar")] TBL_DetalleSAP tBL_DetalleSAP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_DetalleSAP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_DetalleSAP);
        }

        // GET: TBL_DetalleSAP/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_DetalleSAP tBL_DetalleSAP = db.TBL_DetalleSAP.Find(id);
            if (tBL_DetalleSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_DetalleSAP);
        }

        // POST: TBL_DetalleSAP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TBL_DetalleSAP tBL_DetalleSAP = db.TBL_DetalleSAP.Find(id);
            db.TBL_DetalleSAP.Remove(tBL_DetalleSAP);
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
