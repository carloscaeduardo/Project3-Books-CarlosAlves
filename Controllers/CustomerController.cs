using Project3_Books_CarlosAlves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Books_CarlosAlves.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult AllCustomers(string id, int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities();
            List<Customer> customer = context.Customers.ToList();


            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.Name).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.Name).ToList();
                        }


                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.Address).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.Address).ToList();
                        }

                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.City).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.City).ToList();
                        }

                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.State).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.State).ToList();
                        }

                        break;
                    }
                case 5:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.ZipCode).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.ZipCode).ToList();
                        }

                        break;
                    }
                case 6:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.deleted).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.deleted).ToList();
                        }

                        break;
                    }
                case 7:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.State).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.State).ToList();
                        }

                        break;
                    }

                case 0:
                default:
                    {
                        if (isDesc)
                        {
                            customer = context.Customers.OrderByDescending(c => c.CustomerID).ToList();
                        }
                        else
                        {
                            customer = context.Customers.OrderBy(c => c.CustomerID).ToList();
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
                    customer = customer.Where(c =>
                                                    c.CustomerID == idLookup
                                                ).ToList();
                }
                
                else
                {
                    customer = customer.Where(c =>
                                                c.Name.ToLower().Contains(id) ||
                                                c.State.ToLower().Contains(id) ||
                                                c.City.ToLower().Contains(id) ||
                                                c.Address.ToLower().Contains(id) ||
                                                c.ZipCode.ToLower().Contains(id)
                                                        ).ToList();
                }

            }

            return View(customer);
         
        }
        
        /// <summary>
        /// HttpGet to Return a Customer to edit or Create
        /// </summary>
        /// <param name="id">[Optional] The Id of the customer to edit</param>
        /// <returns>The Customer Entity or a new Customer Entity</returns>
        [HttpGet]
        public ActionResult UpsertCustomer(int id = 0)
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

        [HttpPost]
        public ActionResult UpsertCustomer(UpsertCustomerModel model)
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