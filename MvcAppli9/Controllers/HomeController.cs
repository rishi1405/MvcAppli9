using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAppli9.Models;
using PagedList.Mvc;
using PagedList;

namespace MvcAppli9.Controllers
{
    public class HomeController : Controller
    {
        private hdfcDBContext db = new hdfcDBContext();

        //
        // GET: /Home/

        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.GenderSort = sortBy == "Gender" ? "Gender desc" : "Gender";

            var employees = db.Emploes.AsQueryable();

            if(searchBy == "Gender")
            {
                employees = employees.Where(x => x.Gender == search || search == null);
            }
            else
            {
                employees = employees.Where(x => x.Name.StartsWith(search) || search == null);
            }
            switch (sortBy)
            {
                case "Name desc":
                    employees = employees.OrderByDescending(x => x.Name);
                    break;
                case "Gender desc":
                    employees = employees.OrderByDescending(x => x.Gender);
                    break;
                case "Gender":
                    employees = employees.OrderBy(x => x.Gender);
                    break;
                default:
                    employees = employees.OrderBy(x => x.Name);
                    break;
            }

            return View(employees.ToPagedList(page ?? 1, 3));
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id = 0)
        {
            Emplo emplo = db.Emploes.Find(id);
            if (emplo == null)
            {
                return HttpNotFound();
            }
            return View(emplo);
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(Emplo emplo)
        {
            if (ModelState.IsValid)
            {
                db.Emploes.Add(emplo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplo);
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Emplo emplo = db.Emploes.Find(id);
            if (emplo == null)
            {
                return HttpNotFound();
            }
            return View(emplo);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(Emplo emplo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emplo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplo);
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Emplo emplo = db.Emploes.Find(id);
            if (emplo == null)
            {
                return HttpNotFound();
            }
            return View(emplo);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Emplo emplo = db.Emploes.Find(id);
            db.Emploes.Remove(emplo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}