using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237_assignment6.Models;

namespace cis237_assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageContext db = new BeverageContext();

        // GET: Beverages
        public ActionResult Index()
        {
            // Setup a variable to hold the Beverage data.
            DbSet<Beverage> BeveragesToFilter = db.Beverages;

            // Setup some strings to hold the data that might be 
            // in the session. If there is nothing in the session
            // we can still use these variables as a default value.
            string filterName = "";
            string filterMin = "";
            string filterMax = "";
            string filterPack = "";

            // Define a min and max for the filter price
            decimal min = 0;
            decimal max = 100;

            // Check to see if there is a value in the session,
            // and if there is, assign it to the variable that
            // we setup to hold the value.
            if (!string.IsNullOrWhiteSpace(
                (string)Session["session_name"]
            ))
            {
                filterName = (string)Session["session_name"];
            }

            if (!string.IsNullOrWhiteSpace(
                (string)Session["session_min"]
            ))
            {
                if (Convert.ToDecimal((string)Session["session_min"]) < min)
                {

                    RedirectToAction("Index");
                }
                else
                {
                    filterMin = (string)Session["session_min"];
                    min = decimal.Parse(filterMin);
                }
            }

            if (!string.IsNullOrWhiteSpace(
                (string)Session["session_max"]
            ))
            {
                if (Convert.ToDecimal((string)Session["session_max"]) > max)
                {

                    RedirectToAction("Index");
                }
                else
                {
                    filterMax = (string)Session["session_max"];
                    max = decimal.Parse(filterMax);
                }
            }

            if (!string.IsNullOrWhiteSpace(
                (string)Session["session_pack"]
            ))
            {
                filterPack = (string)Session["session_pack"];
            }

            // Use the BeveragesToFilter on the Dataset.
            // Use lambda expressions to narrow the filter.
            // There are default values for each of the 
            // filter parameters, min, max, filterName, and filterPack 
            // so this should always run with no errors.
            IList<Beverage> finalFiltered = BeveragesToFilter.Where(
                beverage => beverage.price >= min &&
                beverage.price <= max &&
                beverage.name.Contains(filterName) &&
                beverage.pack.Contains(filterPack)
            ).ToList();

            // Place the string representation of the values 
            // that are in the session into the viewbag so
            // that they can be retrieved and displayed on the view.
            ViewBag.filterName = filterName;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;
            ViewBag.filterPack = filterPack;

            // Return the view with the filtered selection of beverages.
            return View(finalFiltered);
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Beverages/Filter/6
        // Method is Post because it is reached
        // using submit.
        // Validate the antiforgery token.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            // Get the form data that we sent out of the request object.
            // The string that is used as a key to get the data matches
            // the name property of the form control
            // These strings hold the data from the form objects
            string name = Request.Form.Get("name");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");
            string pack = Request.Form.Get("pack");

            // The data is pulled out from the request object, now it is
            // put into the session so other methods can access it.
            Session["session_name"] = name;
            Session["session_min"] = min;
            Session["session_max"] = max;
            Session["session_pack"] = pack;

            // Redirect to index page
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
