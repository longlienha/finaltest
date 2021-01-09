using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        quanlydogoEntities db = new quanlydogoEntities();
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<customer> li = (from cus in db.customers select cus).ToList();
            return View(li);
        }
        [HttpPost]
        public ActionResult Index(string keyword = "")
        {
            List<customer> li = (from cus in db.customers where cus.name.Contains(keyword) && cus.name != "" select cus).ToList();

            return View(li);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(string name, string adress, string phone)
        {
            customer cus = new customer();
            cus.name = name;
            cus.adress = adress;
            cus.phone = phone;
            db.customers.Add(cus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            customer cus = db.customers.Find(id);

            return View(cus);
        }
        [HttpPost]
        public ActionResult Edit(int id, string name, string adress, string phone)
        {
            customer cus = db.customers.Find(id);
            cus.name = name;
            cus.adress = adress;
            cus.phone = phone;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            customer cus = db.customers.Find(id);
            db.customers.Remove(cus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}