using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3_Books_CarlosAlves.Models
{
    public class UpsertInvoiceModel
    {
        public Invoice Invoice { get; set; }
        public List<Customer> Customers { get; set; }
        public List<InvoiceLineItem> InvoiceLineItems { get; set; }

        public List<Product> Products { get; set; }
    }
}