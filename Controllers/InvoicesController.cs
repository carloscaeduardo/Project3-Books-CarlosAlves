using Project3_Books_CarlosAlves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace Project3_Books_CarlosAlves.Controllers
{
    public class InvoicesController : Controller
    {
        // GET: Customer
        /// <summary>
        /// Returns the AllCustomers view
        /// </summary>
        /// <param name="id"> The url used in the search feature</param>
        /// <param name="sortBy"> Which column of the Customer table to sort by the resulting table</param>
        /// <param name="isDesc"> parameter that shows if the sort is in the Ascending or Descending order</param>
        /// <returns></returns>
        public ActionResult AllInvoices(string id, int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities();
            List<Invoice> invoice = context.Invoices.ToList();


            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.CustomerID).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.CustomerID).ToList();
                        }


                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.InvoiceDate).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.InvoiceDate).ToList();
                        }

                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.ProductTotal).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.ProductTotal).ToList();
                        }

                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.SalesTax).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.SalesTax).ToList();
                        }

                        break;
                    }
                case 5:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.Shipping).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.Shipping).ToList();
                        }

                        break;
                    }
                case 6:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.InvoiceTotal).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.InvoiceTotal).ToList();
                        }

                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                        {
                            invoice = context.Invoices.OrderByDescending(c => c.InvoiceID).ToList();
                        }
                        else
                        {
                            invoice = context.Invoices.OrderBy(c => c.InvoiceID).ToList();
                        }

                        break;
                    }


            }


            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                int idLookup;
                decimal idDecimalLookup;
                DateTime idDateTimeLookup = DateTime.Today;


                if (int.TryParse(id, out idLookup))
                {
                    invoice = invoice.Where(c =>
                                                    c.InvoiceID == idLookup ||
                                                    c.CustomerID == idLookup
                                                ).ToList();
                }
                else if(decimal.TryParse(id, out idDecimalLookup))
                {
                    invoice = (List<Invoice>)invoice.Where(c =>
                                                    c.ProductTotal == idDecimalLookup ||
                                                    c.SalesTax == idDecimalLookup ||
                                                    c.Shipping == idDecimalLookup ||
                                                    c.InvoiceTotal == idDecimalLookup

                    ).ToList();
                }
                else
                {
                    invoice = invoice.Where(c =>
                                                c.InvoiceDate == idDateTimeLookup).ToList();
                }

            }

            return View(invoice);

        }

        /// <summary>
        /// HttpGet to Return a Customer to edit or Create
        /// </summary>
        /// <param name="id">[Optional] The Id of the customer to edit</param>
        /// <returns>The Customer Entity or a new Customer Entity</returns>
        [HttpGet]
        public ActionResult UpsertInvoice(int id = 0)
        {
            BooksEntities context = new BooksEntities();
            // If no customer in the DB, Return a new instance of Customer object
            Invoice invoice = context.Invoices.Where(c => c.InvoiceID == id).FirstOrDefault() ?? new Invoice();
            List<Customer> customers = context.Customers.ToList();
            List<InvoiceLineItem> invoiceLineItems = context.InvoiceLineItems.ToList();
            List<Product> products = context.Products.ToList();
            UpsertInvoiceModel viewModel = new UpsertInvoiceModel()
            {
               
         
                Invoice = invoice,
                Customers = customers,
                InvoiceLineItems = invoiceLineItems,
                Products = products
            };





            return View(viewModel);
        }

        /// <summary>
        /// HttpPost to Create or Update a Customer instance 
        /// </summary>
        /// <param name="model"> the model used to create the view</param>
        /// <returns> Redirect to the AllCustomers page after the creation or edit.</returns>
        [HttpPost]
        public ActionResult UpsertInvoice(UpsertInvoiceModel model, string customerId)
        {

            Invoice newInvoice = model.Invoice;
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.Invoices.Where(c => c.InvoiceID == newInvoice.InvoiceID).Count() > 0)
                {
                    var invoiceToSave = context.Invoices.Where(c => c.InvoiceID == newInvoice.InvoiceID).FirstOrDefault();



                    invoiceToSave.CustomerID = newInvoice.CustomerID;
                    invoiceToSave.InvoiceDate = newInvoice.InvoiceDate;
                    invoiceToSave.ProductTotal = newInvoice.ProductTotal;
                    invoiceToSave.SalesTax = newInvoice.SalesTax;
                    invoiceToSave.Shipping = newInvoice.Shipping;
                    invoiceToSave.InvoiceTotal = CalculateInvoiceTotals(invoiceToSave).InvoiceTotal;

                }
                else
                {
                    newInvoice.InvoiceTotal = CalculateInvoiceTotals(newInvoice).InvoiceTotal;

                    context.Invoices.Add(newInvoice);
                }
                context.SaveChanges();




            }
            catch (System.Exception)
            {
                throw;
                // log the exception in the error log, and send an automatic email to IT Support
                //return RedirectToAction("Error");
            }
            return RedirectToAction("AllInvoices");
        }

        public Invoice CalculateInvoiceTotals(Invoice invoice)
        {
            BooksEntities context = new BooksEntities();

            List<InvoiceLineItem> lineItems = context.InvoiceLineItems.Where(i => i.InvoiceID == invoice.InvoiceID).ToList();

            invoice.InvoiceTotal = 0;
            foreach (var lineItem in lineItems)
            {
                invoice.ProductTotal += lineItem.ItemTotal;
            }
            invoice.InvoiceTotal = invoice.ProductTotal + invoice.Shipping + invoice.SalesTax;
            
            return invoice;
        }

        /// <summary>
        /// HttpGet to Delete an instance of a Customer.
        /// </summary>
        /// <param name="id"> Customer pk used to delete the Customer</param>
        /// <returns>Redirects to the AllCustomer page after deleting an existing instance of a Customer</returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            int invoiceId = 0;
            if (int.TryParse(id, out invoiceId))
            {
                try
                {
                    Invoice invoice = context.Invoices.Where(c => c.InvoiceID == invoiceId).FirstOrDefault();
                    context.Invoices.Remove(invoice);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //parse   
            }
            return RedirectToAction("AllInvoices");
        }

        [HttpPost]
        public ActionResult UpsertLineItem(InvoiceLineItem lineItem)
        {
            BooksEntities context = new BooksEntities();
            var invoicelineItem = context.InvoiceLineItems.Where(i => i.ProductCode == lineItem.ProductCode && i.InvoiceID == lineItem.InvoiceID).FirstOrDefault();

            //context.InvoiceLineItems.AddOrUpdate(InvoiceLineItem);
            return Json("yes");
        }
    }
}