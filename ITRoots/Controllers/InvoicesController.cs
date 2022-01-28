using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ITRoots.Models;

namespace ITRoots.Controllers
{
    public class InvoicesController : BaseController
    {
        private db_a79052_rootdbEntities _context;

        private readonly IMapper mapper;

        public InvoicesController()
        {
            _context = new db_a79052_rootdbEntities();
            mapper = AutoMapperConfig.Mapper;
        }

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = _context.Invoices.Include(i => i.Users).ToList();
            var invoicesVM = mapper.Map<List<InvoiceVM>>(invoices);
            foreach (var item in invoicesVM)
            {
                decimal total = 0;

                foreach (var product in item.Products)
                {
                    total += product.Price * product.Quantity;
                }
                item.TotalPrice = total;
            }
          //  invoicesVM.ForEach(item =>item.TotalPrice = item.pr)
            return View(invoicesVM);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Invoices invoice = _context.Invoices.Find(id);
            if (invoice == null)
                return HttpNotFound();

            ViewBag.UserID = new SelectList(_context.Users, "ID", "FullName", invoice.UserID);
            var invoiceVM = mapper.Map<InvoiceVM>(invoice);
            return View(invoiceVM);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(_context.Users, "ID", "FullName");
            InvoiceVM invoiceVM = new InvoiceVM()
            {
                CreatedDate = DateTime.Now,
                Products = new List<ProductVM>()
            };
            return View(invoiceVM);
        }

        // POST: Invoices/Create
        [HttpPost]
        public ActionResult Create(InvoiceVM invoiceVM)
        {
            if (ModelState.IsValid)
            {
                Invoices invoice = mapper.Map<Invoices>(invoiceVM);
                invoice.CreatedDate = DateTime.Now;
                _context.Invoices.Add(invoice);
                _context.SaveChanges();
                return Json(true);
            }

            ViewBag.UserID = new SelectList(_context.Users, "ID", "FullName", invoiceVM.UserID);
            return View(invoiceVM);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Invoices invoice = _context.Invoices.Find(id);
            if (invoice == null)
                return HttpNotFound();
            
            ViewBag.UserID = new SelectList(_context.Users, "ID", "FullName", invoice.UserID);
            var invoiceVM = mapper.Map<InvoiceVM>(invoice);
            return View(invoiceVM); 
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        public ActionResult Edit(InvoiceVM invoiceVM)
        {
            if (ModelState.IsValid)
            {
                Invoices invoiceDB = _context.Invoices.Find(invoiceVM.ID);
                if (invoiceDB == null)
                    return HttpNotFound();
                if(invoiceDB.UserID != invoiceVM.UserID)
                {
                    invoiceDB.UserID = invoiceVM.UserID;
                    _context.Entry(invoiceDB).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                Invoices invoice = mapper.Map<Invoices>(invoiceVM);
                var removedList = new List<Products>();
                foreach (var product in invoiceDB.Products.ToList())
                {
                    var pp = invoiceVM.Products.Select(a => a.ID).ToList();

                    if (!pp.Contains(product.ID))
                    {
                        removedList.Add(product);
                    } 
                }
                _context.Products.RemoveRange(removedList);

              //  if(removedList)
              var newProducts = invoice.Products.Where(a => a.ID == 0).ToList();
                _context.Products.AddRange(newProducts);

                if(removedList.Count() > 0 || newProducts.Count()>0)
                _context.SaveChanges();

                return Json(true);
            }
            ViewBag.UserID = new SelectList(_context.Users, "ID", "FullName", invoiceVM.UserID);
            return View(invoiceVM);
        }
        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Invoices invoice = _context.Invoices.Find(id);
            if (invoice == null)
                return HttpNotFound();
            
            return View( invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoices invoices = _context.Invoices.Find(id);
            _context.Products.RemoveRange(invoices.Products);
            _context.Invoices.Remove(invoices);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
