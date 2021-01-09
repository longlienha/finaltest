using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        quanlydogoEntities db = new quanlydogoEntities();
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var li = (from pro in db.products select pro).ToList();
            return View(li);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Insert()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Insert(string name, int price, int pricelive, string product_type, string info, string design_type, string size)
        {
            product pro = new product();
            pro.name = name;
            pro.price = price;
            pro.pricelive = pricelive;
            pro.product_type = product_type;
            pro.info = info;
            pro.design_type = design_type;
            pro.size = size;
            db.products.Add(pro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id) {

            var li = (from pro in db.products select pro).SingleOrDefault(x => x.id == id);
            return View(li);
        }
        [HttpPost]
        public ActionResult Edit(int id, string name, float price, float pricelive, string product_type, string info, string design_type, string size)
        {

            var li = (from pro in db.products select pro).SingleOrDefault(x => x.id == id);
            li.name = name;
            li.price = (int)price;
            li.pricelive = (int)pricelive;
            li.product_type = product_type;
            li.info = info;
            li.design_type = design_type;
            li.size = size;
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            product prod = db.products.Find(id);
            db.products.Remove(prod);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Store(int? id)
        {
            product onePro = db.products.Find(id);
            ViewBag.UserName = onePro.name;
            List<stock> li = db.stocks.ToList();
            List<int> checkItem = new List<int>();
            List<int> checkQuantity = new List<int>();
            foreach (var item in li) {
                var isItem = (from PS in db.stock_product where PS.productID == id && PS.stockId == item.id select PS).FirstOrDefault();
                if (isItem != null) {
                    checkItem.Add(item.id);
                    checkQuantity.Add(isItem.quantity);
                }
            }
            ViewBag.CheckList = checkItem;
            ViewBag.CheckQuantity = checkQuantity;
            return View(li);
        }
        [HttpPost]
        public ActionResult Store(int id,int[] check,int[] all,int[] quantity) {
            int[] abc = check;
            int[] cdf = all;
            int[] qwe = quantity;
            WebApplication2.Models.product oneUser = db.products.Find(id);
            ViewBag.UserName = oneUser.name;
            List<quantity_stockModel> qs_Model = new List<quantity_stockModel>();
            foreach (int StockId in cdf)
            {
                quantity_stockModel qs = new quantity_stockModel();
                qs.stockID = StockId;
                qs.quantity = qwe[0];
                qwe = qwe.Where(t => t != qwe[0]).ToArray();
                qs_Model.Add(qs);
            }
            if (abc != null)
            {
                foreach (int StockId in all)
                {
                    if (check.Contains(StockId))
                    {
                        if (db.stock_product.Where(t => t.stockId == StockId && t.productID == id).FirstOrDefault() == null)
                        {
                            stock_product StPr = new stock_product();
                            StPr.productID = id;
                            StPr.stockId = StockId;
                            quantity_stockModel qs = (from model in qs_Model where model.stockID == StockId select model).FirstOrDefault();
                            StPr.quantity = qs.quantity;
                            StPr.timeStamp = DateTime.Now.AddDays(30).ToString("yyyyMMdd");
                            db.stock_product.Add(StPr);
                            db.SaveChanges();

                        }

                    }
                    else
                    {
                        stock_product roleUser = (from t in db.stock_product where t.productID == id && t.stockId == StockId select t).FirstOrDefault();
                        if (roleUser != null)
                        {
                            db.stock_product.Remove(roleUser);
                            db.SaveChanges();
                        }
                    }

                }
            }
            else
            {
                foreach (int StockId in all)
                {
                    stock_product roleUser = (from t in db.stock_product where t.productID == id && t.stockId == StockId select t).FirstOrDefault();
                    if (roleUser != null)
                    {
                        db.stock_product.Remove(roleUser);
                        db.SaveChanges();
                    }
                }
            }

            List<stock> li = db.stocks.ToList();
            List<int> checkItem = new List<int>();
            List<int> checkQuantity = new List<int>();
            foreach (var item in li)
            {
                var isInItem = (from ur in db.stock_product where ur.productID == id && ur.stockId == item.id select ur).FirstOrDefault();
                if (isInItem != null)
                {
                    checkItem.Add(item.id);
                    checkQuantity.Add(isInItem.quantity);
                }
            }
            ViewBag.CheckList = checkItem;
            ViewBag.CheckQuantity = checkQuantity;

            return View(li);
        }

    }
}