using Project3_Books_CarlosAlves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Books_CarlosAlves.Controllers
{
    public class ProductController : Controller
    {
        /// <summary>
        /// Get the All product view, with features to sort and seach within the table columns
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns>Return the products view</returns>
        public ActionResult AllProducts(string id, int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities();
            List<Product> products = context.Products.ToList();

            switch (sortBy)
            {

                case 1:
                    {
                        if (isDesc)
                        {
                            products = context.Products.OrderByDescending(s => s.Description).ToList();
                        }
                        else
                        {
                            products = context.Products.OrderBy(s => s.Description).ToList();
                        }

                        break;
                    }
                case 2:
                    {
                        
                        if (isDesc)
                        {
                            products = context.Products.OrderByDescending(s => s.UnitPrice).ToList();
                        }
                        else
                        {
                            products = context.Products.OrderBy(s => s.UnitPrice).ToList();
                        }

                        break;
                    }
                case 3:
                    {
                        
                        if (isDesc)
                        {
                            products = context.Products.OrderByDescending(s => s.OnHandQuantity).ToList();
                        }
                        else
                        {
                            products = context.Products.OrderBy(s => s.OnHandQuantity).ToList();
                        }

                        break;
                    }

                case 0:
                default:
                    {
                        if (isDesc)
                        {
                            products = context.Products.OrderByDescending(s => s.ProductCode).ToList();
                        }
                        else
                        {
                            products = context.Products.OrderBy(s => s.ProductCode).ToList();
                        }


                        break;
                    }


            }


            if (!string.IsNullOrWhiteSpace(id))
            {
                int idLookup;
                id = id.Trim().ToLower();
                if (int.TryParse(id, out idLookup))
                {
                    products = products.Where(c =>
                                                    c.UnitPrice == idLookup ||
                                                    c.OnHandQuantity == idLookup
                                                ).ToList();
                }
                else
                {
                    products = products.Where(s =>
                                      s.ProductCode.ToLower().Contains(id) ||
                                      s.Description.ToLower().Contains(id)).ToList();

                }


            }

            return View(products);
        }


        /// <summary>
        /// Http Get request to view that updates or adds new product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns the upsert view for product</returns>
        [HttpGet]
        public ActionResult UpsertProduct(string id)
        {
            BooksEntities context = new BooksEntities();
            var productToSave = context.Products.Where(s => s.ProductCode == id).FirstOrDefault();
            if (productToSave == null)
            {
                productToSave = new Product();
            }
            return View(productToSave);
        }
        /// <summary>
        /// Http Post request to upsate or add new product to list
        /// </summary>
        /// <param name="newProduct"></param>
        /// <returns>redirects to Allproducts page</returns>
        [HttpPost]
        public ActionResult UpsertProduct(Product newProduct)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.Products.Where(s => s.ProductCode == newProduct.ProductCode).Count() > 0)
                {
                    var productToSave = context.Products.Where(s => s.ProductCode == newProduct.ProductCode).FirstOrDefault();

                    productToSave.Description = newProduct.Description;
                    productToSave.UnitPrice = newProduct.UnitPrice;
                    productToSave.OnHandQuantity = newProduct.OnHandQuantity;
                 


                }
                else
                {
                    context.Products.Add(newProduct);
                }
                context.SaveChanges();




            }
            catch (System.Exception)
            {
                throw;
                // log the exception in the error log, and send an automatic email to IT Support
                //return RedirectToAction("Error");
            }
            return RedirectToAction("AllProducts");
        }

        /// <summary>
        /// Http Get request to delete a row on the products table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>redirects to the AllProducts view</returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            string productCode = id;

            try
            {
                Product product = context.Products.Where(s => s.ProductCode == productCode).FirstOrDefault();
                context.Products.Remove(product);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AllProducts");
        }
    }
}