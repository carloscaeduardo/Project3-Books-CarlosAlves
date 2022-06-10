using Project3_Books_CarlosAlves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace Project3_Books_CarlosAlves.Controllers
{
    public class InvoiceLineItemsController : Controller
    {
        /// <summary>
        /// HTTP GET manages the request for the upsert Invoice Line Items
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="productCode"></param>
        /// <returns>Returns line item view</returns>
        [HttpGet]
        public ActionResult UpsertInvoiceLineItems(int invoiceId, string productCode)
        {
            BooksEntities context = new BooksEntities();
            InvoiceLineItem lineItem = context.InvoiceLineItems.Where(i => i.InvoiceID == invoiceId && i.ProductCode == productCode).FirstOrDefault(); 

            if (lineItem == null)
            {
                lineItem = new InvoiceLineItem();
                lineItem.InvoiceID = invoiceId;
            }


            return View(lineItem);
        }

        /// <summary>
        /// Http Post rquest to edit or add new Invoice Line Item
        /// </summary>
        /// <param name="invoiceLineItem"></param>
        /// <returns>Redirects to the invoice upsert view</returns>
        [HttpPost]
        public ActionResult UpsertInvoiceLineItems(InvoiceLineItem invoiceLineItem)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                invoiceLineItem.ItemTotal = invoiceLineItem.Quantity * invoiceLineItem.UnitPrice;
                context.InvoiceLineItems.AddOrUpdate(invoiceLineItem);
                context.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }

            return Redirect("/Invoices/UpsertInvoice/" + invoiceLineItem.InvoiceID.ToString());
        }
    }
}