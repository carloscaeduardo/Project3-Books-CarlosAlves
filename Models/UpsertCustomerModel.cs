using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3_Books_CarlosAlves.Models
{
    public class UpsertCustomerModel
    {
        public Customer Customer { get; set; }
        public List<State> States { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}