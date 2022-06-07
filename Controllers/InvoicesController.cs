using Project3_Books_CarlosAlves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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


                if (int.TryParse(id, out idLookup))
                {
                    invoice = invoice.Where(c =>
                                                    c.InvoiceID == idLookup
                                                ).ToList();
                }

                else
                {
                    invoice = invoice.Where(c =>
                                                c.Name.ToLower().Contains(id) ||
                                                c.State.ToLower().Contains(id) ||
                                                c.City.ToLower().Contains(id) ||
                                                c.Address.ToLower().Contains(id) ||
                                                c.ZipCode.ToLower().Contains(id)
                                                        ).ToList();
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
            Customer customer = context.Customers.Where(c => c.CustomerID == id).FirstOrDefault() ?? new Customer();
            List<State> states = context.States.ToList();
            List<Invoice> invoices = context.Invoices.ToList();
            UpsertCustomerModel viewModel = new UpsertCustomerModel()
            {
                Customer = customer,
                States = states,
                Invoices = invoices
            };





            return View(viewModel);
        }

        /// <summary>
        /// HttpPost to Create or Update a Customer instance 
        /// </summary>
        /// <param name="model"> the model used to create the view</param>
        /// <returns> Redirect to the AllCustomers page after the creation or edit.</returns>
        [HttpPost]
        public ActionResult UpsertInvoice(UpsertCustomerModel model)
        {
            Customer newCustomer = model.Customer;
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.Customers.Where(c => c.CustomerID == newCustomer.CustomerID).Count() > 0)
                {
                    var customerToSave = context.Customers.Where(c => c.CustomerID == newCustomer.CustomerID).FirstOrDefault();

                    customerToSave.Name = newCustomer.Name;
                    customerToSave.Address = newCustomer.Address;
                    customerToSave.City = newCustomer.City;
                    customerToSave.State = newCustomer.State;
                    customerToSave.ZipCode = newCustomer.ZipCode;
                    customerToSave.deleted = newCustomer.deleted;

                }
                else
                {
                    context.Customers.Add(newCustomer);
                }
                context.SaveChanges();




            }
            catch (System.Exception)
            {
                throw;
                // log the exception in the error log, and send an automatic email to IT Support
                //return RedirectToAction("Error");
            }
            return RedirectToAction("AllCustomers");
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
            int customerId = 0;
            if (int.TryParse(id, out customerId))
            {
                try
                {
                    Customer customer = context.Customers.Where(c => c.CustomerID == customerId).FirstOrDefault();
                    context.Customers.Remove(customer);

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
            return RedirectToAction("AllCustomers");
        }
    }
}